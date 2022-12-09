
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.dropzone = factory();
    }

}(this, function () {

    const Queue = {
        WAITING: 0,
        SENDING: 1,
        SENDED: 2,
        ERROR: 3
    };

    const Unit = {
        BYTE: 'B',
        KILOBYTE: 'KB',
        MEGABYTE: 'MB',
        GIGABYTE: 'GB'
    };

    window.dropzone = {
        _timer: 0,
        _queue: [],
        _quantity: 0,
        _running: false,
        _dropzoneQueue: null,
        _dropzoneMessage: null,
        _dropzoneQueueLabel: null,
        _dropzoneQueueClose: null,
        _dropzoneQueueToggle: null,
        _dropzoneQueueHeader: null,
        _dropzoneQueueContent: null
    };

    /* ----------------------------------------------------------- */
    /* == component */
    /* ----------------------------------------------------------- */

    function Dropzone(element, options) {

        this.windowInitialized = false;
        this.overlayInitialized = false;
        this.dropzoneInitialized = false;

        var defaults = {
            url: '',
            max: null,
            base: 1024,
            parallel: 1,
            accept: '*',
            onFind: null,
            onError: null,
            maxSize: null,
            onCancel: null,
            onUpload: null,
            onRemove: null,
            method: 'POST',
            clickable: null,
            removable: true,
            multiple: false,
            message: {
                click: ' ou clique',
                icon: 'cloud-upload',
                empty: 'Para realizar o envio dos arquivos solte-os nesta área',
                error: {
                    icon: 'error',
                    url: 'Nenhuma URL foi informada',
                    quantity: 'O limite máximo de {quantity} arquivos foi atingido',
                    accept: 'Arquivos do tipo {extension} não são aceitos, somente {accept}',
                    size: 'O arquivo {name} possui tamanho maior que o limite permitido {size}',
                    multiple: 'Não é permitido selecionar/soltar de multiplos arquivos ao mesmo tempo'
                },
                confirm: {
                    remove: 'Você tem certeza?',
                    resend: 'Tentar enviar este arquivo novamente?'
                }
            }
        };

        this.element = element;

        this.options = extend({}, defaults, options);

        this.init();
    }

    Dropzone.prototype.init = function () {

        _build.call(this);

        if (window.dropzone._dropzoneQueue === undefined) {

            _buildQueue.call(this);
            _buildMessage.call(this);
            _getQueueStatus.call(this);
            _bindGlobalEvent.call(this);
        }

        _bindEvent.call(this);

        return this;
    };

    Dropzone.prototype.destroy = function () {

        if (this.drozone === null) return;

        _unbindEvent.call(this);

        this.drozone.parentNode.removeChild(this.drozone);

        window.dropzone._dropzoneQueue.parentNode.removeChild(window.dropzone._dropzoneQueue);
        window.dropzone._dropzoneMessage.parentNode.removeChild(window.dropzone._dropzoneMessage);

        this.dropzone = null;
    };

    /* ----------------------------------------------------------- */
    /* == private methods */
    /* ----------------------------------------------------------- */

    function _build() {

        this.drozone = document.createElement('div');
        this.drozone.className = 'dropzone-overlay';
        this.drozone.setAttribute('data-message', this.options.message.empty);

        if (this.options.clickable !== null) {

            this.dropzoneFileUpload = document.createElement('input');
            this.dropzoneFileUpload.setAttribute('type', 'file');

            if (this.options.multiple) {

                this.dropzoneFileUpload.setAttribute('multiple', true);
            }

            this.dropzoneFileUpload.className = 'dropzone-file-uploader';

            let elementTarget = this.options.clickable === true ? this.element : document.querySelectorAll(this.options.clickable)[0];

            elementTarget.insertBefore(this.dropzoneFileUpload, elementTarget.firstElementChild);
        }

        if (this.dropzoneFileUpload === undefined) {

            this.element.insertBefore(this.drozone, this.element.firstChild);

        } else {

            this.drozone.classList.add('full');
            this.drozone.classList.add('d-none');

            document.body.appendChild(this.drozone);
        }
    }

    function _buildQueue() {

        let dropzoneQueue = document.createElement('div');
        dropzoneQueue.className = 'dropzone-upload d-none';

        let queueHeader = document.createElement('div');
        queueHeader.className = 'upload-header active';

        let queueLabel = document.createElement('div');
        queueLabel.className = 'header-label';
        queueLabel.innerHTML = 'Lista de arquivos';

        let queueCloseIcon = document.createElement('div');
        queueCloseIcon.className = 'header-icon close';

        let queueToggleIcon = document.createElement('div');
        queueToggleIcon.className = 'header-icon arrow';

        queueHeader.appendChild(queueLabel);
        queueHeader.appendChild(queueToggleIcon);
        queueHeader.appendChild(queueCloseIcon);

        let queueContent = document.createElement('div');
        queueContent.className = 'upload-content active';

        dropzoneQueue.appendChild(queueHeader);
        dropzoneQueue.appendChild(queueContent);

        window.dropzone._dropzoneQueue = dropzoneQueue;
        window.dropzone._dropzoneQueueLabel = queueLabel;
        window.dropzone._dropzoneQueueHeader = queueHeader;
        window.dropzone._dropzoneQueueClose = queueCloseIcon;
        window.dropzone._dropzoneQueueContent = queueContent;
        window.dropzone._dropzoneQueueToggle = queueToggleIcon;

        document.body.appendChild(dropzoneQueue);
    }

    function _buildMessage() {

        let dropzoneMessage = document.createElement('div');
        dropzoneMessage.className = 'dropzone-overlay-message d-none';

        window.dropzone._dropzoneMessage = dropzoneMessage;

        document.body.appendChild(dropzoneMessage);
    }

    function _buildQueueItem(name, extension, size) {

        let queueItem = document.createElement('div');
        queueItem.className = 'dropzone-item';

        let containerInfo = document.createElement('div');
        containerInfo.className = 'item-info';

        let informationName = document.createElement('div');
        informationName.className = 'item-name';
        informationName.innerHTML = `<i class="file-icon-${extension.toLowerCase()}"></i> ${name.toLowerCase()}`;

        let informationSize = document.createElement('div');
        informationSize.className = 'item-size';
        informationSize.innerHTML = size;

        containerInfo.appendChild(informationName);
        containerInfo.appendChild(informationSize);

        let containerControl = document.createElement('div');
        containerControl.className = 'item-control';

        containerInfo.appendChild(containerControl);

        let containerProgress = document.createElement('div');
        containerProgress.className = 'progress';

        let containerProgressbar = document.createElement('div');
        containerProgressbar.className = 'bar';

        containerProgress.appendChild(containerProgressbar);

        queueItem.appendChild(containerInfo);
        queueItem.appendChild(containerProgress);

        return queueItem;
    }

    function _bindGlobalEvent() {

        this._g_events = {
            windowSubmit: _handleSubmit.bind(this),
            closeQueue: _handleCloseQueue.bind(this),
            toggleQueue: _handleToggleQueue.bind(this),
            windowBeforeUnload: _handleBeforeUnload.bind(this),
            removeFromQueue: _handleRemoveFromQueue.bind(this)
        };

        window.addEventListener('submit', this._g_events.windowSubmit);
        window.addEventListener('beforeunload', this._g_events.windowBeforeUnload);

        window.dropzone._dropzoneQueueClose.addEventListener('click', this._g_events.closeQueue);
        window.dropzone._dropzoneQueueHeader.addEventListener('click', this._g_events.toggleQueue);
        window.dropzone._dropzoneQueueContent.addEventListener('click', this._g_events.removeFromQueue);
    }

    function _handleSubmit(event) {

        if (window.dropzone._running) event.preventDefault();
    }

    function _handleBeforeUnload(event) {

        if (window.dropzone._running) return '';
    }

    function _handleRemoveFromQueue(event) {

        if (event.target && event.target.classList.contains('item-control')) {

            let element = event.target.closestClass('dropzone-item');

            if (element.classList.contains('error')) {

                _resend.call(this, event.target);

            } else if (element.classList.contains('success')) {

                if (typeof this.options.onFind === 'function') {

                    this.options.onFind.call(this, event.target);
                }

            } else {

                _removeQueue.call(this, event.target);
            }
        }
    }

    function _handleToggleQueue(event) {

        window.dropzone._dropzoneQueueHeader.classList.toggle('active');

        window.dropzone._dropzoneQueueContent.classList.toggle('active');
    }

    function _handleCloseQueue(event) {

        event.preventDefault();

        if (window.dropzone._running) {

            [].forEach.call(window.dropzone._queue, function (item, i) {

                if (item.situation !== Queue.SENDED) {

                    item.request.abort();
                }

            });

            window.dropzone._quantity = 0;

            window.dropzone._queue = [];

            _toggleQueue.call(this);

        } else {

            window.dropzone._dropzoneQueue.classList.add('d-none');
        }
    }

    function _bindEvent() {

        this._events = {
            fileUpload: _handleFileUpload.bind(this),
            windowDrop: _handleWindowDrop.bind(this),
            overlayDrop: _handleOverlayDrop.bind(this),
            windowDragover: _handleWindowDragover.bind(this),
            windowDragleave: _handleWindowDragleave.bind(this),
            overlayDragenter: _handleOverlayDragenter.bind(this),
            overlayDragleave: _handleOverlayDragleave.bind(this)
        };

        window.addEventListener('drop', this._events.windowDrop);
        window.addEventListener('dragover', this._events.windowDragover);
        window.addEventListener('dragleave', this._events.windowDragleave);

        if (this.options.clickable !== null) {

            this.dropzoneFileUpload.addEventListener('change', this._events.fileUpload);
        }

        this.drozone.addEventListener('drop', this._events.overlayDrop);
        this.drozone.addEventListener('dragenter', this._events.overlayDragenter);
        this.drozone.addEventListener('dragleave', this._events.overlayDragleave);
    }

    function _unbindEvent() {

        window.removeEventListener('drop', this._events.windowDrop);
        window.removeEventListener('dragover', this._events.windowDragover);
        window.removeEventListener('dragleave', this._events.windowDragleave);

        if (this.options.clickable !== null) {

            this.dropzoneFileUpload.removeEventListener('change', this._events.fileUpload);
        }

        this.drozone.removeEventListener('drop', this._events.overlayDrop);
        this.drozone.removeEventListener('dragenter', this._events.overlayDragenter);
        this.drozone.removeEventListener('dragleave', this._events.overlayDragleave);
    }

    function _handleFileUpload(event) {

        let self = this;

        [].forEach.call(event.target.files, function (item, i) {

            _upload.call(self, item);

        });
    }

    function _handleWindowDrop(event) {

        event.preventDefault();

        this.windowInitialized = false;
        this.overlayInitialized = false;
        this.dropzoneInitialized = false;

        this.drozone.classList.add('d-none');
    }

    function _handleWindowDragover(event) {

        if (isFile(event)) {

            event.preventDefault();

            if (this.windowInitialized) {

                return;
            }

            this.windowInitialized = true;

            this.drozone.classList.remove('d-none');
        }
    }

    function _handleWindowDragleave(event) {

        if (!this.overlayInitialized && !this.dropzoneInitialized) {

            this.windowInitialized = false;

            this.drozone.classList.add('d-none');
        }
    }

    function _handleOverlayDrop(event) {

        event.preventDefault();

        let files = event.dataTransfer.files;

        if (!this.options.multiple && files.length === 1 || this.options.multiple) {

            let self = this;

            [].forEach.call(files, function (item, i) {

                if (item.type !== '') {

                    _upload.call(self, item);
                }

            });

        } else {

            _showMessage.call(this, this.options.message.error.multiple);
        }

        event.target.classList.remove('hover');
    }

    function _handleOverlayDragenter(event) {

        if (isFile(event)) {

            if (this.overlayInitialized) {

                return;
            }

            event.target.classList.add('hover');

            this.overlayInitialized = true;
        }
    }

    function _handleOverlayDragleave(event) {

        event.target.classList.remove('hover');

        this.overlayInitialized = false;
    }

    function _upload(file) {

        let message = null;

        if (this.options.url.length > 0) {

            // if quantity is less than the limit
            if (this.options.max === null || this.options.max > window.dropzone._quantity) {

                const sizeInMB = file.size / Math.pow(this.options.base, 2);

                const sizeString = file.size.toSizeString(this.options.base);

                // if size is less than the limit
                if (this.options.maxSize === null || sizeInMB <= this.options.maxSize) {

                    const extension = file.name.substr(file.name.lastIndexOf('.') + 1).toLowerCase();

                    // if the extension is accepted
                    if (this.options.accept === '*' || this.options.accept.indexOf(extension) !== -1) {

                        let self = this;

                        window.dropzone._dropzoneQueue.classList.remove('d-none');

                        window.dropzone._quantity++;

                        let reader = new FileReader();

                        reader.onload = (function (file) {

                            return function (event) {

                                let queueItem = _buildQueueItem.call(self, file.name, extension, sizeString);

                                window.dropzone._dropzoneQueueContent.insertBefore(queueItem, window.dropzone._dropzoneQueueContent.firstChild);

                                _addQueue.call(self, queueItem, file);
                            };

                        })(file);

                        reader.readAsDataURL(file);

                    } else {

                        message = this.options.message.error.accept.format({ extension: extension, accept: this.options.accept.join(', ') });
                    }

                } else {

                    message = this.options.message.error.size.format({ name: nomeArquivo, size: (this.options.max * Math.pow(this.options.base, 2)).toSizeString(this.options.base) });
                }

            } else {

                message = this.options.message.error.quantity.format({ quantity: this.options.quantity });
            }
        } else {

            message = this.options.message.error.url;
        }

        _showMessage.call(this, message);
    }

    function _addQueue(element, file) {

        const key = Math.floor(Date.now()); // generate an unique id

        let formData = new FormData();

        formData.append('file', file); // set file to upload

        formData.append('key', key); // set 'key' for future operations

        let xmlHttpRequest = _getRequest.call(this, element);

        let request = {

            key: key,
            data: formData,
            request: xmlHttpRequest,
            situation: Queue.WAITING
        };

        element.setAttribute('data-key', key);

        window.dropzone._queue.push(request);
    }

    function _getRequest(element) {

        let self = this;

        let xmlHttpRequest = new XMLHttpRequest();

        let progressbar = element.querySelectorAll('.bar')[0];

        xmlHttpRequest.open(this.options.method, this.options.url);

        // progressbar
        xmlHttpRequest.upload.addEventListener('progress', function (event) {

            if (event.lengthComputable) {

                progressbar.style.width = parseInt(event.loaded / event.total * 100, 10) + '%';
            }

        }, false);

        // upload done
        xmlHttpRequest.onload = function (result) {

            let object = window.dropzone._queue.getByKey(element.dataset.key);

            if (this.status === 200) {

                element.classList.add('success');

                object.situation = Queue.SENDED;

                if (typeof self.options.onUpload === 'function') {

                    self.options.onUpload.call(self, object);
                }

            } else {

                element.classList.add('error');

                object.situation = Queue.ERROR;

                if (typeof self.options.onError === 'function') {

                    self.options.onError.call(self, object);
                }

                element.querySelectorAll('.item-control')[0].setAttribute('title', this.statusText);
            }
        };

        return xmlHttpRequest;
    }

    function _runQueue() {

        let self = this;

        if (!window.dropzone._running) {

            window.dropzone._running = true;

            window.dropzone._timer = setInterval(function () {

                _setQueueLabel.call(this);

                // there are files waiting for being sended
                if (window.dropzone._queue.filterBySituation(Queue.WAITING).length > 0) {

                    // if there are limit for parallel sending
                    if (window.dropzone._queue.filterBySituation(Queue.SENDING).length < self.options.parallel) {

                        let file = window.dropzone._queue.getNext();

                        file.request.send(file.data);

                        file.situation = Queue.SENDING;
                    }
                }

            }, 1000);
        }
    }

    function _getQueueStatus() {

        let self = this;

        window.dropzone._queue = [];

        setInterval(function () {

            if (window.dropzone._queue.filterBySituation(Queue.WAITING).length > 0) {

                _runQueue.call(self);

            } else {

                window.dropzone._running = false;

                clearInterval(window.dropzone._timer);
            }

        }, 2000);
    }

    function _removeQueue(element) {

        let confirmed = confirm(this.options.message.confirm.remove);

        if (confirmed) {

            let queueItem = element.closestClass('dropzone-item');

            const item = window.dropzone._queue.getByKey(queueItem.dataset.key);

            if (item.index !== -1) {

                let object = item.object;

                if (object.situation === Queue.SENDING) {

                    if (typeof this.options.onRemove === 'function') {

                        this.options.onRemove.call(this, object);
                    }

                } else if (object.situation !== Queue.WAITING) {

                    object.request.abort();

                    if (typeof this.options.onCancel === 'function') {

                        this.options.onCancel.call(this, object);
                    }
                }

                window.dropzone._quantity--;

                window.dropzone._queue.splice(item.index, 1);

                queueItem.parentNode.removeChild(queueItem);

                _toggleQueue.call(this);
            }
        }
    }

    function _resend(element) {

        let confirmed = confirm(this.options.message.confirm.resend);

        if (confirmed) {

            let queueItem = element.closestClass('dropzone-item');

            queueItem.classList.remove('error');
            queueItem.querySelectorAll('.bar')[0].style.width = 0;

            const item = window.dropzone._queue.getByKey(queueItem.dataset.key);

            if (item.index !== -1) {

                let object = item.object;

                object.situation = Queue.WAITING;

                object.request = _getRequest.call(this, queueItem);
            }
        }
    }

    function _toggleQueue() {

        if (window.dropzone._quantity === 0) {

            window.dropzone._dropzoneQueue.classList.add('d-none');

        } else {

            window.dropzone._dropzoneQueue.classList.remove('d-none');
        }
    }

    function _setQueueLabel() {

        let message = null;

        const error = window.dropzone._queue.filterBySituation(Queue.ERROR);

        if (error.length === 0) {

            const sended = window.dropzone._queue.filterBySituation(Queue.SENDED);

            const sending = window.dropzone._queue.filterBySituation(Queue.SENDING);

            const waiting = window.dropzone._queue.filterBySituation(Queue.WAITING);

            if (sended.length + error.length === window.dropzone._queue.length) {

                message = 'Todos os arquivos foram enviados';

            } else if (waiting.length > 0) {

                message = `Existem ${waiting.length} arquivo(s) na fila para envio`;

            } else if (sending.length > 0) {

                message = `Existem ${sending.length} arquivo(s) sendo enviado(s)`;
            }

        } else {

            message = `Ocorreu um error ao tentar enviar ${error.length} arquivo(s)`;
        }

        window.dropzone._dropzoneQueueLabel.innerHTML = message;
    }

    function _hideMessage() {

        if (this.element === null) {

            window.dropzone._dropzoneMessage.classList.add('d-none');

        } else {

            this.drozone.classList.remove('error');
            this.drozone.setAttribute('data-message', this.options.message.empty);
        }
    }

    function _showMessage(message) {

        if (message !== null) {

            if (this.element === null) {

                window.dropzone._dropzoneMessage.innerHTML = message;

                window.dropzone._dropzoneMessage.classList.remove('d-none');

            } else {

                this.drozone.classList.add('error');
                this.drozone.setAttribute('data-message', message);
            }

            setTimeout(_hideMessage.bind(this), 3000);
        }
    }

    /* ----------------------------------------------------------- */
    /* == helpers */
    /* ----------------------------------------------------------- */

    function extend() {

        for (var i = 1; i < arguments.length; i++) {

            for (var key in arguments[i]) {

                if (arguments[i].hasOwnProperty(key)) {

                    arguments[0][key] = arguments[i][key];
                }
            }
        }

        return arguments[0];
    }

    function isFile(event) {
        return event.dataTransfer.types && (event.dataTransfer.types.indexOf ? event.dataTransfer.types.indexOf('Files') !== -1 : event.dataTransfer.types.contains('Files'));
    }

    Element.prototype.closestClass = function (cls) {
        let element = this;
        while ((element = element.parentElement) && !element.classList.contains(cls));
        return element;
    };

    Array.prototype.getNext = function () {

        let next = null;

        [].forEach.call(this, function (item, i) {

            if (item.situation === Queue.WAITING) {

                next = item;

                return;
            }

        });

        return next;
    };

    Array.prototype.getByKey = function (key) {

        let element = { object: null, index: -1 };

        [].forEach.call(this, function (item, i) {

            if (item.key === parseInt(key, 10)) {

                element.index = i;
                element.object = item;

                return;
            }

        });

        return element;
    };

    Array.prototype.filterBySituation = function (situation) {

        let array = Array.isArray(situation);

        return [].filter.call(this, function (item) {

            if (!array && item.situation === situation || array && situation.indexOf(item.situation) !== -1) return item;

        });
    };

    String.prototype.format = function (object) {

        return this.replace(/{([^{}]*)}/g,

            function (a, b) {

                return typeof object[b] === 'string' || typeof object[b] === 'number' ? object[b] : a;
            }
        );
    };

    Number.prototype.toSizeString = function (base) {

        let size = this;

        let pow = Math.pow(base, 3);

        if (size / pow >= 1) { // in GB

            return (size / pow).toFixed(1) + Unit.GIGABYTE;
        }

        pow /= base;

        if (size / pow >= 1) { // in MB

            return (size / pow).toFixed(1) + Unit.MEGABYTE;
        }

        pow /= base;

        if (size / pow >= 1) { // in KB

            return (size / pow).toFixed(1) + Unit.KILOBYTE;
        }

        return size + Unit.BYTE;
    };

    /* ----------------------------------------------------------- */
    /* == return */
    /* ----------------------------------------------------------- */

    return {
        init: Dropzone
    };

}));
