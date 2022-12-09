
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.range = factory();
    }

}(this, function () {

    /* ----------------------------------------------------------- */
    /* == component */
    /* ----------------------------------------------------------- */

    function Range(element) {

        this.timer = null;

        this.element = element;

        this.init();
    }

    Range.prototype.init = function () {

        if (this.element === undefined) return;

        _config.call(this);

        _build.call(this);

        if (!this.element.hasAttribute('disabled') && !this.element.hasAttribute('readonly')) {

            _bindEvent.call(this);
        }

        return this;
    };

    Range.prototype.destroy = function () {

        if (this.range === null) return;

        _unbindEvent.call(this);

        this.range.parentNode.removeChild(this.range);

        this.range = null;
    };

    Range.prototype.setValue = function (value) {

        _calculateDistance.call(this, this.range.clientWidth * (value / this.options.max));
    };

    /* ----------------------------------------------------------- */
    /* == private methods */
    /* ----------------------------------------------------------- */

    function _build() {

        this.element.classList.add('d-none');

        this.range = document.createElement('div');
        this.range.className = 'range';

        this.bar = document.createElement('div');
        this.bar.className = 'bar';

        this.pointer = document.createElement('div');
        this.pointer.className = 'pointer';

        this.bar.appendChild(this.pointer);
        this.range.appendChild(this.bar);

        this.element.parentNode.insertBefore(this.range, this.element.nextSibling);

        this.setValue(this.element.value);
    }

    function _bindEvent() {

        this._events = {
            windowMouseup: _handleRangeMouseup.bind(this),
            rangeMousedown: _handleRangeMousedown.bind(this),
            windowMousemove: _handleRangeMousemove.bind(this)
        };

        this.range.addEventListener('mousedown', this._events.rangeMousedown);

        window.addEventListener('mouseup', this._events.windowMouseup);
        window.addEventListener('mousemove', this._events.windowMousemove);
    }

    function _unbindEvent() {

        this.range.removeEventListener('mousedown', this._events.rangeMousedown);

        window.removeEventListener('mouseup', this._events.windowMouseup);
        window.removeEventListener('mousemove', this._events.windowMousemove);
    }

    function _handleRangeMouseup(event) {

        if (this.range.classList.contains('active')) {

            this.range.classList.remove('active');

            _focusOut.call(this);
        }
    }

    function _handleRangeMousedown(event) {

        event.preventDefault();

        _focusOut.call(this);

        let pageX = event.pageX || event.touches[0].pageX;

        let total = pageX - this.range.offset().left;

        _calculateDistance.call(this, total);

        this.range.setAttribute('data-distance', pageX);

        this.range.classList.add('active');
    }

    function _handleRangeMousemove(event) {

        if (this.range.classList.contains('active')) {

            _focusOut.call(this);

            let pageX = event.pageX || event.touches[0].pageX;

            let start = parseFloat(this.range.dataset.distance, 10);

            let total = pageX - start + this.bar.clientWidth;

            _calculateDistance.call(this, total);

            this.range.setAttribute('data-distance', pageX);
        }
    }

    function _calculateDistance(distance) {

        let width = this.range.clientWidth;

        distance = Math.max(distance >= width ? width : distance, 0);

        let percentage = distance / width;

        this.bar.style.width = `${percentage * 100}%`;

        this.element.value = percentage * parseFloat(this.options.max, 10);

        this.pointer.setAttribute('data-value', this.element.value);
    }

    function _focusOut() {

        if (document.selection) {

            document.selection.empty();

        } else {

            window.getSelection().removeAllRanges();
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

    Element.prototype.offset = function () {

        let rect = this.getBoundingClientRect(),
            scrollLeft = window.pageXOffset || document.documentElement.scrollLeft,
            scrollTop = window.pageYOffset || document.documentElement.scrollTop;

        return { top: rect.top + scrollTop, left: rect.left + scrollLeft }
    };

    /* ----------------------------------------------------------- */
    /* == return */
    /* ----------------------------------------------------------- */

    return {
        init: Range
    };

}));
