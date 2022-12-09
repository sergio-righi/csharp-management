
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.dialog = factory();
    }

}(this, function () {

    const Key = {
        esc: 27
    };

    let isBusy = false;

    function Get(element) {

        return dataStorage.get(element, 'dialog');
    }

    function Dialog(element, options) {

        this.index = 0;
        this.code = null;
        this.allowed = true;

        let defaults = {
            onOpen: null,
            onNext: null,
            onClose: null,
            onCancel: null,
            onConfirm: null,
            beforeOpen: null,
            beforeClose: null,
            closeMethods: ['overlay', 'button', 'escape']
        };

        this.element = element;

        this.options = extend({}, defaults, options);

        this.init();
    }

    Dialog.prototype.init = function () {

        if (this.element === null) return;

        this.hasCancel = this.element.dataset.cancel !== undefined ? true : false;
        this.hasConfirm = this.element.dataset.confirm !== undefined ? true : false;
        this.hasCheckbox = this.element.dataset.checkbox !== undefined ? true : false;
        this.hasControl = this.hasCancel || this.hasConfirm || this.hasCheckbox ? true : false;

        this.lblCancel = this.hasCancel ? this.element.dataset.cancel : '';
        this.lblConfirm = this.hasConfirm ? this.element.dataset.confirm : '';
        this.lblNext = this.element.dataset.next !== undefined ? this.element.dataset.next : '';
        this.lblCheckbox = this.element.dataset.checkbox !== undefined ? this.element.dataset.checkbox : '';

        this.items = this.element.querySelectorAll('[data-item]');

        this.hasNext = this.items.length > 1 ? true : false;

        _build.call(this);

        _bindEvent.call(this);

        dataStorage.put(this.element, 'dialog', this);

        document.body.insertBefore(this.dialog, document.body.firstChild);

        return this;
    };

    Dialog.prototype._busy = function (state) {

        isBusy = state;
    };

    Dialog.prototype._isBusy = function () {

        return isBusy;
    };

    Dialog.prototype.destroy = function () {

        if (this.dialog === null) return;

        if (this.isOpen()) {

            this.close(true);
        }

        _unbindEvent.call(this);

        this.dialog.parentNode.removeChild(this.dialog);

        this.dialog = null;
    };

    Dialog.prototype.isOpen = function () {

        return !!this.dialog.classList.contains('active');
    };

    Dialog.prototype.open = function () {

        if (this._isBusy()) return;

        this._busy(true);

        if (typeof this.options.beforeOpen === 'function') {

            this.options.beforeOpen();
        }

        if (this.hasNext)
            _select.call(this, 0);

        document.body.classList.add('o-hidden');

        this.dialog.classList.add('active');

        this.popup.classList.add('animation-open');
        this.popup.addEventListener('animationend', this._events.animationEnd);

        if (typeof this.options.onOpen === 'function') {

            this.options.onOpen.call(this);

        } else {

            this.element.dispatchEvent(new CustomEvent('dialog.open', { detail: this }));
        }

        this._busy(false);

        return this;
    };

    Dialog.prototype.close = function (force) {

        if (this._isBusy()) return;

        this._busy(true);

        force = force || false;

        if (!this.hasConfirm || force) {

            if (this.items.length > 0 && !this.allowed) {

                this.next();

            } else {

                if (typeof this.options.beforeClose === 'function') {
                    let close = this.options.beforeClose.call(this);
                    if (!close) {
                        this._busy(false);
                        return;
                    }
                }

                document.body.classList.remove('o-hidden');

                if (typeof this.options.onClose === 'function') {

                    this.options.onClose.call(this);

                } else {

                    this.element.dispatchEvent(new CustomEvent('dialog.close', { detail: this }));
                }

                this.popup.classList.add('animation-close');
                this.popup.addEventListener('animationend', this._events.animationEnd);
            }

        } else {

            this.popup.classList.add('animation-block');
            this.popup.addEventListener('animationend', this._events.animationEnd);
        }

        this._busy(false);
    };

    Dialog.prototype.set = function (header, content) {

        this.setHeader(header);
        this.setContent(content);
    };

    Dialog.prototype.setHeader = function (header) {

        if (header !== undefined) {

            this.header.innerHTML = header;
        }
    };

    Dialog.prototype.setContent = function (content) {

        if (content !== undefined) {

            this.content.innerHTML = content;
        }
    };

    Dialog.prototype.next = function () {

        this.element.dispatchEvent(new CustomEvent('dialog.next', { detail: this }));

        _select.call(this, this.index === this.items.length - 1 ? this.index : this.index + 1);
    };

    /* ----------------------------------------------------------- */
    /* == private methods */
    /* ----------------------------------------------------------- */

    function _build() {

        this.dialog = document.createElement('div');
        this.dialog.className = 'dialog';

        this.popup = document.createElement('div');
        this.popup.className = 'dialog-popup';

        let footer = document.createElement('div');
        footer.className = 'dialog-footer';

        if (this.hasControl) {

            if (this.hasCheckbox) {

                let uniqueId = Math.random().toString(36).substr(2, 16);

                this.checkboxContainer = document.createElement('div');
                this.checkboxContainer.className = 'checkbox';

                this.checkbox = document.createElement('input');
                this.checkbox.setAttribute('type', 'checkbox');
                this.checkbox.id = uniqueId;

                let lblCheckbox = document.createElement('label');
                lblCheckbox.setAttribute('for', uniqueId);
                lblCheckbox.textContent = this.lblCheckbox;

                this.checkboxContainer.appendChild(this.checkbox);
                this.checkboxContainer.appendChild(lblCheckbox);

                footer.appendChild(this.checkboxContainer);
            }

            if (this.hasNext) {

                this.nextButton = document.createElement('button');
                this.nextButton.setAttribute('type', 'button');
                this.nextButton.className = 'button mr-2';
                this.nextButton.textContent = this.lblNext;

                footer.appendChild(this.nextButton);
            }

            if (this.hasCancel) {

                this.cancelButton = document.createElement('button');
                this.cancelButton.setAttribute('type', 'button');
                this.cancelButton.className = 'button mr-2';
                this.cancelButton.textContent = this.lblCancel;

                footer.appendChild(this.cancelButton);
            }

            if (this.hasConfirm) {

                this.confirmButton = document.createElement('button');
                this.confirmButton.setAttribute('type', 'button');
                this.confirmButton.className = 'button button-send button-plain';
                this.confirmButton.textContent = this.lblConfirm;

                footer.appendChild(this.confirmButton);
            }
        }

        this.header = document.createElement('div');
        this.header.className = 'dialog-header';

        this.content = document.createElement('div');
        this.content.className = 'dialog-content';

        _config.call(this, 0);

        this.popup.appendChild(this.header);
        this.popup.appendChild(this.content);
        this.popup.appendChild(footer);

        this.dialog.appendChild(this.popup);

        this.element.innerHTML = this.dialog.outerHTML;
    }

    function _bindEvent() {

        this._events = {
            clickNext: _handleClickNext.bind(this),
            keyboardNav: _handleKeyboardNav.bind(this),
            clickCancel: _handleClickCancel.bind(this),
            clickConfirm: _handleClickConfirm.bind(this),
            clickOverlay: _handleClickOutside.bind(this),
            animationEnd: _handleAnimationEnd.bind(this),
            triggerButton: _handleTriggerButton.bind(this)
        };

        if (this.options.closeMethods.indexOf('button') !== -1) {

            if (this.hasCancel) {

                this.cancelButton.addEventListener('click', this._events.clickCancel.bind(true));
            }

            if (this.hasConfirm) {

                this.confirmButton.addEventListener('click', this._events.clickConfirm.bind(true));
            }

            if (this.hasNext) {

                this.nextButton.addEventListener('click', this._events.clickNext.bind(true));
            }
        }

        this.dialog.addEventListener('mousedown', this._events.clickOverlay);

        document.addEventListener('keydown', this._events.keyboardNav);

        this.buttons = document.querySelectorAll(`[data-dialog="#${this.element.id}"]`);

        for (let i = 0; i < this.buttons.length; i++) {

            this.buttons[i].addEventListener('click', this._events.triggerButton);
        }
    }

    function _handleKeyboardNav(event) {

        if (this.options.closeMethods.indexOf('escape') !== -1 && event.which === Key.esc && this.isOpen()) {

            this.close();
        }
    }

    function _handleClickOutside(event) {

        if (this.options.closeMethods.indexOf('overlay') !== -1 && !_findAncestor(event.target, 'dialog') && event.clientX < this.dialog.clientWidth) {

            this.close();
        }
    }

    function _handleClickNext(event) {

        this.close.call(this, false);

        if (typeof this.options.onNext === 'function') {

            this.options.onNext.call(this);
        }
    }

    function _handleClickCancel(event) {

        this.close.call(this, true);

        if (typeof this.options.onCancel === 'function') {

            this.options.onCancel.call(this);
        }
    }

    function _handleClickConfirm(event) {

        this.close.call(this, true);

        if (typeof this.options.onConfirm === 'function') {

            this.options.onConfirm.call(this);

        } else {

            this.element.dispatchEvent(new CustomEvent('dialog.confirm', { detail: this }));
        }
    }

    function _handleTriggerButton(event) {

        if (event.target && event.target.hasAttribute('data-dialog')) {

            if (event.target.getAttribute('disabled') === null) {

                if (event.target.href !== undefined) {

                    event.preventDefault();
                    event.stopImmediatePropagation();

                    this.options.onConfirm = function () {

                        window.location.href = event.target.href;
                    };
                }

                this.setHeader(event.target.dataset.header);
                this.setContent(event.target.dataset.content);

                this.open();
            }
        }
    }

    function _handleAnimationEnd(event) {

        this.popup.removeEventListener('animationend', this._events.animationEnd);

        this.popup.classList.remove('animation-open');
        this.popup.classList.remove('animation-close');
        this.popup.classList.remove('animation-block');

        if (event.animationName === 'scale_out') {

            this.dialog.classList.remove('active');
        }
    }

    function _findAncestor(element, cls) {
        while ((element = element.parentElement) && !element.classList.contains(cls));
        return element;
    }

    function _unbindEvent() {

        if (this.options.closeMethods.indexOf('button') !== -1) {

            if (this.hasCancel) {

                this.cancelButton.removeEventListener('click', this._events.clickClose);
            }

            if (this.hasConfirm) {

                this.confirmButton.removeEventListener('click', this._events.clickClose);
            }

            if (this.hasNext) {

                this.nextButton.removeEventListener('click', this._events.clickNext);
            }
        }

        this.dialog.removeEventListener('mousedown', this._events.clickOverlay);

        document.removeEventListener('keydown', this._events.keyboardNav);

        if (this.buttons !== undefined) {

            for (let i = 0; i < this.buttons.length; i++) {

                this.buttons[i].removeEventListener('click', this._events.triggerButton);
            }
        }
    }

    function _config(index) {

        if (this.items[index] !== undefined) {

            let header = this.items[index].querySelectorAll('[data-header]')[0];

            if (header !== undefined) {

                this.setHeader(header.innerHTML);
            }

            let content = this.items[index].querySelectorAll('[data-content]')[0];

            if (content !== undefined) {

                this.setContent(content.innerHTML);
            }
        }
    }

    function _select(index) {

        this.index = index;

        let item = this.items[index];

        if (index === this.items.length - 1) {

            if (this.hasCancel)
                this.cancelButton.classList.remove('d-none');

            if (this.hasNext)
                this.nextButton.classList.add('d-none');

            this.allowed = true;

        } else if (index === 0) {

            if (this.hasCancel)
                this.cancelButton.classList.add('d-none');

            if (this.hasNext)
                this.nextButton.classList.remove('d-none');

            this.allowed = false;

        } else {

            if (this.hasCancel)
                this.cancelButton.classList.add('d-none');

            if (this.hasNext)
                this.nextButton.classList.remove('d-none');
        }

        if (item.dataset.code) {

            if (this.hasNext)
                this.checkboxContainer.classList.remove('d-none');

            this.code = parseInt(item.dataset.code, 10);

        } else {

            this.code = null;

            if (this.hasNext)
                this.checkboxContainer.classList.add('d-none');
        }

        if (this.checkbox !== undefined) {

            this.checkbox.checked = false;
        }

        if (this.items.length > 1) {

            this.header.setAttribute('data-text', `${index + 1}/${this.items.length}`);
        }

        _config.call(this, index);
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
        init: Dialog,
        get: Get
    };

}));
