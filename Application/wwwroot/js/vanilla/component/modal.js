
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.modal = factory();
    }

}(this, function () {

    const Key = {
        esc: 27
    };

    let isBusy = false;

    function Get(element) {

        return dataStorage.get(element, 'modal');
    }

    /* ----------------------------------------------------------- */
    /* == component */
    /* ----------------------------------------------------------- */

    function Modal(element, options) {

        var defaults = {
            onOpen: null,
            onClose: null,
            beforeOpen: null,
            beforeClose: null,
            closeMethods: ['overlay', 'button', 'escape']
        };

        this.trigger = null;

        this.modal = element;

        this.options = extend({}, defaults, options);

        this.init();
    }

    Modal.prototype.init = function () {

        _build.call(this);

        _bindEvent.call(this);

        dataStorage.put(this.modal, 'modal', this);

        return this;
    };

    Modal.prototype._busy = function (state) {

        isBusy = state;
    };

    Modal.prototype._isBusy = function () {

        return isBusy;
    };

    Modal.prototype.destroy = function () {

        if (this.modal === null) return;

        if (this.isOpen()) {

            this.close(true);
        }

        _unbindEvent.call(this);

        this.modal.parentNode.removeChild(this.modal);

        this.modal = null;
    };

    Modal.prototype.isOpen = function () {

        return !!this.modal.classList.contains('active');
    };

    Modal.prototype.open = function () {

        if (this._isBusy()) return;

        this._busy(true);

        if (typeof this.options.beforeOpen === 'function') {

            this.options.beforeOpen();
        }

        document.body.classList.add('o-hidden');

        this.modal.classList.add('active');

        if (typeof this.options.onOpen === 'function') {

            this.options.onOpen.call(self);
        }

        this._busy(false);

        return this;
    };

    Modal.prototype.close = function () {

        if (this._isBusy()) return;

        this._busy(true);

        if (typeof this.options.beforeClose === 'function') {
            let close = this.options.beforeClose.call(this);
            if (!close) {
                this._busy(false);
                return;
            }
        }

        document.body.classList.remove('o-hidden');

        this.modal.classList.remove('active');

        if (typeof this.options.onClose === 'function') {

            this.options.onClose.call(this);
        }

        this._busy(false);
    };

    Modal.prototype.setHeader = function (header) {

        if (this.header === undefined) {

            this.header = document.createElement('div');
            this.header.className = 'modal-header';

            if (this.popup.childNodes.length === 0) {

                this.popup.appendChild(this.header);

            } else {

                this.popup.insertBefore(this.header, this.popup.firstElementChild);
            }
        }

        if (header !== undefined) {

            this.header.innerHTML = header;
        }

        return this;
    };

    Modal.prototype.setContent = function (content) {

        if (this.content === undefined) {

            this.content = document.createElement('div');
            this.content.className = 'modal-content';

            if (this.header === undefined) {

                this.popup.appendChild(this.content);

            } else {

                this.header.parentNode.insertBefore(this.content, this.header.nextSibling);
            }
        }

        if (content !== undefined) {

            this.content.innerHTML = content;
        }

        return this;
    };

    Modal.prototype.setFooter = function (footer) {

        if (this.footer === undefined) {

            this.footer = document.createElement('div');
            this.footer.className = 'modal-footer';

            this.popup.appendChild(this.footer);
        }
        
        if (footer !== undefined) {

            this.footer.innerHTML = footer;
        }

        return this;
    };

    /* ----------------------------------------------------------- */
    /* == private methods */
    /* ----------------------------------------------------------- */

    function _build() {

        this.popup = document.createElement('div');
        this.popup.className = 'modal-popup';

        this.closeButton = document.createElement('button');
        this.closeButton.setAttribute('type', 'button');
        this.closeButton.className = 'modal-close';

        _config.call(this);

        this.modal.innerHTML = '';
        this.modal.appendChild(this.closeButton);
        this.modal.appendChild(this.popup);
    }

    function _bindEvent() {

        this._events = {
            clickClose: _handleClickClose.bind(this),
            keyboardNav: _handleKeyboardNav.bind(this),
            clickOverlay: _handleClickOutside.bind(this),
            triggerButton: _handleTriggerButton.bind(this)
        };

        this.modal.addEventListener('mousedown', this._events.clickOverlay);

        document.addEventListener('keydown', this._events.keyboardNav);

        this.closeButton.addEventListener('mouseup', this._events.clickClose);

        this.buttons = document.querySelectorAll(`[data-modal="#${this.modal.id}"]`);

        for (let i = 0; i < this.buttons.length; i++) {

            this.buttons[i].addEventListener('click', this._events.triggerButton);
        }
    }

    function _unbindEvent() {

        this.modal.removeEventListener('mousedown', this._events.clickOverlay);

        document.removeEventListener('keydown', this._events.keyboardNav);

        this.closeButton.removeEventListener('mouseup', this._events.clickClose.bind(true));

        if (this.buttons !== undefined) {

            for (let i = 0; i < this.buttons.length; i++) {

                this.buttons[i].removeEventListener('click', this._events.triggerButton);
            }
        }
    }

    function _handleKeyboardNav(event) {

        if (this.options.closeMethods.indexOf('escape') !== -1 && event.which === Key.esc && this.isOpen()) {

            this.close();
        }
    }

    function _handleClickOutside(event) {

        if (this.options.closeMethods.indexOf('overlay') !== -1 && !_findAncestor(event.target, 'modal') && event.clientX < this.modal.clientWidth) {

            this.close();
        }
    }

    function _handleClickClose(event) {

        if (this.options.closeMethods.indexOf('button') !== -1) {

            this.close();
        }
    }

    function _handleTriggerButton(event) {

        if (event.target && event.target.closestAttribute('data-modal')) {

            let element = event.target.closestAttribute('data-modal');

            if (element.getAttribute('disabled') === null) {

                this.trigger = element;
                this.open();
            }

            this.setHeader(element.dataset.header);
            this.setContent(element.dataset.content);
            this.setFooter(element.dataset.footer);
        }
    }

    function _config() {

        let header = this.modal.querySelectorAll('[data-header]')[0];

        if (header !== undefined) {

            this.setHeader(header.innerHTML);
        }

        let content = this.modal.querySelectorAll('[data-content]')[0];

        if (content !== undefined) {

            this.setContent(content.innerHTML);
        }

        let footer = this.modal.querySelectorAll('[data-footer]')[0];

        if (footer !== undefined) {

            this.setFooter(footer.innerHTML);
        }
    }

    function _findAncestor(element, cls) {
        while ((element = element.parentElement) && !element.classList.contains(cls));
        return element;
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

    /* ----------------------------------------------------------- */
    /* == return */
    /* ----------------------------------------------------------- */

    return {
        init: Modal,
        get: Get
    };

}));
