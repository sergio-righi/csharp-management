
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.number = factory();
    }

}(this, function () {

    /* ----------------------------------------------------------- */
    /* == component */
    /* ----------------------------------------------------------- */

    function Number(element) {

        this.timer = null;

        this.element = element;

        this.init();
    }

    Number.prototype.init = function () {

        if (this.element === undefined) return;

        if (!this.element.hasAttribute('disabled') && !this.element.hasAttribute('readonly')) {

            _config.call(this);

            _build.call(this);

            _bindEvent.call(this);
        }

        return this;
    };

    Number.prototype.destroy = function () {

        if (this.number === null) return;

        _unbindEvent.call(this);

        this.number.parentNode.removeChild(this.number);

        this.number = null;
    };

    Number.prototype.getValue = function () {

        return parseFloat(this.element.value, 10);
    };

    Number.prototype.setValue = function (value) {

        if (value >= this.options.min && value <= this.options.max) {

            this.field.value = value;
            this.element.value = value;
        }

        return this;
    };

    Number.prototype.decrease = function () {

        let value = this.getValue() - this.options.step;

        if (this.getValue() === this.options.max) {

            this.increaseButton.setAttribute('disabled', false);

        } else if (value === this.options.min) {

            this.decreaseButton.setAttribute('disabled', true);

            clearInterval(this.timer);
        }

        this.setValue(value);
    };

    Number.prototype.increase = function () {

        let value = parseFloat(this.getValue() + this.options.step, 10);

        if (this.getValue() === this.options.min) {

            this.decreaseButton.setAttribute('disabled', false);

        } else if (value === this.options.max) {

            this.increaseButton.setAttribute('disabled', true);

            clearInterval(this.timer);
        }

        this.setValue(value);
    };

    /* ----------------------------------------------------------- */
    /* == private methods */
    /* ----------------------------------------------------------- */

    function _build() {

        this.element.classList.add('d-none');

        this.number = document.createElement('div');
        this.number.className = 'input-group';

        this.field = document.createElement('input');
        this.field.setAttribute('type', 'text');
        this.field.className = 'a-center';

        this.setValue.call(this, this.getValue());

        this.decreaseButton = document.createElement('div');
        this.decreaseButton.className = 'button';
        this.decreaseButton.innerHTML = '<i class="mdi mdi-minus"></i>';

        this.increaseButton = document.createElement('div');
        this.increaseButton.className = 'button';
        this.increaseButton.innerHTML = '<i class="mdi mdi-plus"></i>';

        this.number.appendChild(this.decreaseButton);
        this.number.appendChild(this.field);
        this.number.appendChild(this.increaseButton);

        this.element.parentNode.insertBefore(this.number, this.element.nextSibling);
    }

    function _bindEvent() {

        this._events = {
            fieldKeyup: _handleFieldKeyup.bind(this),
            fieldKeydown: _handleFieldKeydown.bind(this),
            decreaseMousedown: _handleDecreaseMousedown.bind(this),
            increaseMousedown: _handleIncreaseMousedown.bind(this),
            decreaseMouseUpLeave: _handleDecreaseMouseUpLeave.bind(this),
            increaseMouseUpLeave: _handleIncreaseMouseUpLeave.bind(this)
        };

        this.field.addEventListener('keyup', this._events.fieldKeyup);
        this.field.addEventListener('keydown', this._events.fieldKeydown);

        this.increaseButton.addEventListener('mousedown', this._events.increaseMousedown);
        this.increaseButton.addEventListener('mouseup', this._events.increaseMouseUpLeave);
        this.increaseButton.addEventListener('mouseleave', this._events.increaseMouseUpLeave);

        this.decreaseButton.addEventListener('mousedown', this._events.decreaseMousedown);
        this.decreaseButton.addEventListener('mouseup', this._events.decreaseMouseUpLeave);
        this.decreaseButton.addEventListener('mouseleave', this._events.decreaseMouseUpLeave);
    }

    function _unbindEvent() {

        this.field.removeEventListener('keyup', this._events.fieldKeyup);
        this.field.removeEventListener('keydown', this._events.fieldKeydown);

        this.increaseButton.removeEventListener('mousedown', this._events.increaseMousedown);
        this.increaseButton.removeEventListener('mouseup', this._events.increaseMouseUpLeave);
        this.increaseButton.removeEventListener('mouseleave', this._events.increaseMouseUpLeave);

        this.decreaseButton.removeEventListener('mousedown', this._events.decreaseMousedown);
        this.decreaseButton.removeEventListener('mouseup', this._events.decreaseMouseUpLeave);
        this.decreaseButton.removeEventListener('mouseleave', this._events.decreaseMouseUpLeave);
    }

    function _handleDecreaseMousedown(event) {

        this.decrease();

        _setDecreaseInterval.call(this, true);
    }

    function _handleIncreaseMousedown(event) {

        this.increase();

        _setIncreaseInterval.call(this, true);
    }

    function _handleDecreaseMouseUpLeave(event) {

        _setDecreaseInterval.call(this, false);
    }

    function _handleIncreaseMouseUpLeave(event) {

        _setIncreaseInterval.call(this, false);
    }

    function _handleFieldKeyup(event) {

        if (parseFloat(this.field.value, 10) >= this.options.max) {

            this.setValue(this.options.max);
        }
    }

    function _handleFieldKeydown(event) {

        if (event.key.length === 1 && /[a-zA-Z]/.test(event.key)) {

            event.preventDefault();
        }
    }

    function _setIncreaseInterval(start) {

        if (start) {

            let self = this;

            this.timer = setInterval(function () {

                self.increase();

            }, 100);

        } else {

            clearInterval(this.timer);
        }
    }

    function _setDecreaseInterval(start) {

        if (start) {

            let self = this;

            this.timer = setInterval(function () {

                self.decrease();

            }, 100);

        } else {

            clearInterval(this.timer);
        }
    }

    function _config() {

        let min = this.element.min !== undefined ? parseFloat(this.element.min, 10) : 0;
        let max = this.element.max !== undefined ? parseFloat(this.element.max, 10) : 0;
        let step = this.element.step !== undefined ? parseFloat(this.element.step, 10) : 0;

        this.options = { min: min, max: max, step: step };
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
        init: Number
    };

}));
