
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.stepper = factory();
    }

}(this, function () {

    /* ----------------------------------------------------------- */
    /* == component */
    /* ----------------------------------------------------------- */

    function Stepper(element, options) {

        this.step = 0;

        var defaults = {
            onClick: null,
            onAfterMove: null,
            onBeforeMove: null
        };

        this.stepper = element;

        this.options = extend({}, defaults, options);

        this.init();
    }

    Stepper.prototype.init = function () {

        this.moveTo(this.getStep());

        this.items = Array.prototype.slice.call(this.stepper.querySelectorAll('.step'), 0);


        this.linear = this.stepper.dataset.linear !== undefined;

        this.locked = this.stepper.dataset.locked !== undefined;


        _bindEvent.call(this);


        return this;
    };

    Stepper.prototype.destroy = function () {

        if (this.stepper === undefined) return;

        _unbindEvent.call(this);

        this.stepper.parentNode.removeChild(this.stepper);

        this.stepper = null;
    };

    Stepper.prototype.getStep = function () {

        const value = this.stepper.getAttribute('data-step');

        return value === null ? 0 : parseInt(value, 10);
    };

    Stepper.prototype.setStep = function (step) {

        step = parseInt(step === null ? 0 : step, 10);

        this.stepper.setAttribute('data-step', step);
    };

    Stepper.prototype.setStatus = function (step, status) {

        let index = this.getStep();

        if (this.items[step] !== undefined) {

            this.items[step].classList.add(status);
        }
    };

    Stepper.prototype.prev = function () {

        let step = this.getStep();

        if (step - 1 >= 0) {

            this.moveTo(step - 1);
        }

        return this;
    };

    Stepper.prototype.next = function () {

        let step = this.getStep();

        if (step + 1 <= this.items.length - 1) {

            this.items[step].classList.add('active');

            this.moveTo(step + 1);
        }

        return this;
    };

    Stepper.prototype.moveTo = function (index) {

        let step = this.getStep();

        if (step !== index) {

            let stepChanged = false;

            if (typeof this.options.onBeforeMove === 'function') {

                this.options.onBeforeMove.call(this);
            }

            let item = this.items[index];

            if (this.linear && index === step + 1) {

                this.items[step].className = 'step success';

                stepChanged = true;
            }

            let previousElement = item.previousElementSibling === null ? item : item.previousElementSibling;

            if (!this.linear || (this.linear && (previousElement.classList.contains('success') || item.classList.contains('success')))) {

                item.classList.add('active');

                this.items[step].classList.remove('active');

                this.setStep(index);

                stepChanged = true;
            }

            if (stepChanged && typeof this.options.onAfterMove === 'function') {

                this.options.onAfterMove.call(this);
            }
        }
    };

    /* ----------------------------------------------------------- */
    /* == private methods */
    /* ----------------------------------------------------------- */

    function _bindEvent() {

        this._events = {
            headerClick: _handleHeaderClick.bind(this)
        };

        if (!this.locked) {

            this.stepper.addEventListener('click', this._events.headerClick);
        }
    }

    function _unbindEvent() {

        if (!this.locked) {

            this.stepper.removeEventListener('click', this._events.headerClick);
        }
    }

    function _handleHeaderClick(event) {

        if (event.target && event.target.classList.contains('step-header')) {

            let item = _findAncestor(event.target, 'step');

            if (item !== undefined) {

                let index = Array.from(this.items).indexOf(item);

                if (index !== -1 && index !== this.getStep() && !item.classList.contains('disabled')) {

                    this.moveTo(index);
                }
            }
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
        init: Stepper
    };

}));
