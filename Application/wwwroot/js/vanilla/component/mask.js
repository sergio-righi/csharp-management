
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.mask = factory();
    }

}(this, function () {

    const Default = {
        time: '99:99',
        string: 'A^50',
        decimal: '9^10,99',
        date: '99/99/9999',
        zipcode: '99999-999',
        cpf: '999.999.999-99',
        phone: '(99) 9 9999-9999',
        cnpj: '99.999.999/9999-99'
    };

    const Key = {
        refresh: 'F5',
        enter: 'ENTER',
        delete: 'DELETE',
        control: 'CONTROL',
        backspace: 'BACKSPACE'
    };

    /* ----------------------------------------------------------- */
    /* == component */
    /* ----------------------------------------------------------- */

    function Mask(element, options) {

        this.rule = null;
        this.empty = null;

        var defaults = {
        };

        this.element = element;

        this.options = extend({}, defaults, options);

        this.init();
    }

    Mask.prototype.init = function () {

        if (!this.element) return;

        _config.call(this);

        return this;
    };

    Mask.prototype.destroy = function () {

        if (this.element === null) return;

        _unbindEvent.call(this);

        this.element = null;
    };

    Mask.prototype.set = function (value) {

        let caret = 0;

        if (value.length > 0) {

            let mask = this.empty;

            let buffer = value.split('').map(x => x.trim()).filter(Boolean);

            for (var i = 0; i < buffer.length; i++) {

                caret = mask.nextIndexOf(caret, /_/g);

                if (_allowReplacement.call(this, buffer[i], caret)) {

                    mask = mask.replaceAt(caret, buffer[i]);
                    caret++;
                }
            }

            this.element.value = mask;
            this.element.setCaret(caret);

        } else {

            this.element.value = this.empty;
        }
    };

    /* ----------------------------------------------------------- */
    /* == private methods */
    /* ----------------------------------------------------------- */

    function _config() {

        if (this.element.dataset.mask !== undefined) {

            this.rule = Default[this.element.dataset.mask];

            if (this.rule !== null) {

                _bindEvent.call(this);

                if (this.rule.length === 1) {

                    this.empty = '';

                } else {

                    this.empty = this.rule.replace(/[A,9]/g, '_');
                }
            }
        }
    }

    function _bindEvent() {

        this._events = {
            onLoad: _handleLoad.bind(this),
            onBlur: _handleBlur.bind(this),
            onFocus: _handleFocus.bind(this),
            onPaste: _handlePaste.bind(this),
            onKeyUp: _handleKeyUp.bind(this),
            onMouseUp: _handleMouseUp.bind(this),
            onKeyDown: _handleKeyDown.bind(this),
            onKeyPress: _handleKeyPress.bind(this)
        };

        this.element.addEventListener('load', this._events.onLoad);
        this.element.addEventListener('blur', this._events.onBlur);
        this.element.addEventListener('focus', this._events.onFocus);
        this.element.addEventListener('paste', this._events.onPaste);
        this.element.addEventListener('keyup', this._events.onKeyUp);
        this.element.addEventListener('mouseup', this._events.onMouseUp);
        this.element.addEventListener('keydown', this._events.onKeyDown);
        this.element.addEventListener('keypress', this._events.onKeyPress);
    }

    function _unbindEvent() {

        this.element.removeEventListener('load', this._events.onLoad);
        this.element.removeEventListener('blur', this._events.onBlur);
        this.element.removeEventListener('focus', this._events.onFocus);
        this.element.removeEventListener('paste', this._events.onPaste);
        this.element.removeEventListener('keyup', this._events.onKeyUp);
        this.element.removeEventListener('mouse', this._events.onMouseUp);
        this.element.removeEventListener('keydown', this._events.onKeyDown);
        this.element.removeEventListener('keypress', this._events.onKeyPress);
    }

    function _handleLoad(event) {

        this.set(this.element.value.unmasked());
    }

    function _handleBlur(event) {

        if (/_/g.test(this.element.value)) {

            this.element.value = '';
        }
    }

    function _handleFocus(event) {

        let value = this.element.value;

        if (value.length > 0) {

            this.set(value.unmasked());

        } else {

            this.element.value = this.empty;
            this.element.setCaret(this.empty.indexOf('_'));
        }
    }

    function _handleMouseUp(event) {

        if (this.element.value === this.empty) {

            this.element.setCaret(this.empty.indexOf('_'));
        }
    }

    function _handlePaste(event) {

        event.preventDefault();
        event.stopPropagation();

        this.set((event.clipboardData || window.clipboardData).getData('Text').unmasked());
    }

    function _handleKeyDown(event) {

        let key = event.key.toUpperCase();

        if (!event.ctrlKey && [Key.enter, Key.refresh, Key.control].indexOf(key) === -1 && [Key.backspace, Key.delete].indexOf(key) !== -1) {

            if (this.element.value.unmasked().length === 0) {

                event.preventDefault();

            } else {

                let caret = this.element.getCaret();

                switch (key) {
                    case Key.backspace:
                        caret--;
                        break;
                    case key.delete:
                        caret++;
                        break;
                }

                if (/[^A-Za-z0-9]/g.test(this.element.value[caret])) {

                    caret = this.empty.prevIndexOf(caret, /_/g);

                    if (caret === -1) {

                        event.preventDefault();

                    } else {

                        this.set(this.element.value.unmasked());
                    }
                }
            }
        }
    }

    function _handleKeyUp(event) {

        let key = event.key.toUpperCase();

        if (!event.ctrlKey && [Key.enter, Key.refresh, Key.control].indexOf(key) === -1 && [Key.backspace, Key.delete].indexOf(key) !== -1) {

            event.preventDefault();

            let value = this.element.value.unmasked();

            if (value.length > 0) {

                let range = this.element.getRange();
                let caret = this.empty.prevIndexOf(this.element.getCaret(), /_/g);

                if (range.length === 0) {

                    this.element.value = value.replaceAt(caret, '');

                } else {

                    this.element.value = value.replaceRangeAt(range.start, range.end, '');
                }

                this.set(this.element.value.unmasked());

            } else {

                this.element.value = this.empty;
                this.element.setCaret(this.empty.indexOf('_'));
            }
        }
    }

    function _handleKeyPress(event) {

        let key = event.key.toUpperCase();

        if (!event.ctrlKey && [Key.enter, Key.refresh, Key.control, Key.backspace, Key.delete].indexOf(key) === -1) {

            let value = this.element.value;

            if (value.length === 0) {

                this.element.value = this.empty;
                this.element.setCaret(this.empty.indexOf('_'));

            } else {

                event.preventDefault();

                if (key.length === 1 && /[A-Z0-9]/g.test(key)) {

                    let caret = this.empty.nextIndexOf(this.element.getCaret(), /_/g);

                    if (_allowReplacement.call(this, event.key, caret)) {

                        this.element.value = value.replaceAt(caret, event.key);
                        this.element.setCaret(caret + 1);
                    }
                }
            }
        }
    }

    function _allowReplacement(character, caret) {

        if (!isNaN(character)) {

            if (/[0-9]/g.test(this.rule[caret])) {

                return true;
            }

        } else {

            if (/[a-zA-Z]/g.test(this.rule[caret])) {

                return true;
            }
        }

        return false;
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

    Element.prototype.getCaret = function () {

        return this.selectionStart;
    };

    Element.prototype.setCaret = function (index) {

        if (this.setSelectionRange) {
            this.focus();
            this.setSelectionRange(index, index);

        } else if (this.createTextRange) {
            let range = this.createTextRange();
            range.collapse(true);
            range.moveEnd('character', index);
            range.moveStart('character', index);
            range.select();
        }
    };

    Element.prototype.getRange = function () {

        return { start: this.selectionStart, end: this.selectionEnd, length: this.selectionEnd - this.selectionStart };
    };

    String.prototype.replaceRangeAt = function (start, end, replacement) {


        let array = this.split('');

        do {

            array[start++] = replacement;

        } while (end > start);

        return array.join('');
    };

    String.prototype.replaceAt = function (start, replacement) {

        let array = this.split('');
        array[start] = replacement;
        return array.join('');
    };

    String.prototype.prevIndexOf = function (index, regex) {

        let substring = this.substr(0, index);
        let match = substring.match(regex);
        return match ? substring.lastIndexOf(match[0]) : -1;
    };

    String.prototype.nextIndexOf = function (index, regex) {

        let substring = this.substr(index, this.length);
        let match = substring.match(regex);
        return match ? substring.indexOf(match[0]) + index : -1;
    };

    String.prototype.unmasked = function () {

        return this.replace(/[^A-Za-z0-9]/g, '');
    };

    /* ----------------------------------------------------------- */
    /* == return */
    /* ----------------------------------------------------------- */

    return {
        init: Mask
    };

}));
