
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.grid = factory();
    }

}(this, function () {

    /* ----------------------------------------------------------- */
    /* == component */
    /* ----------------------------------------------------------- */

    function Grid(element, options) {

        this.height = [];
        this.quantity = [];

        this.row = 0;
        this.column = 0;
        this.columns = 0;
        this.gridWidth = 0;
        this.columnWidth = 0;

        var defaults = {
            margin: 16,
            columns: 4,
            selector: '',
            sorted: false,
            responsive: []
        };

        this.grid = element;

        this.options = extend({}, defaults, options);

        this.init();
    }

    Grid.prototype.init = function () {

        if (this.grid === undefined) return;

        if (_getProperty(this.grid, 'position') !== 'relative') this.grid.style.position = 'relative';

        _config.call(this);

        if (this.options.responsive.length > 0) {

            _responsiveManagement.call(this);
        }

        _build.call(this);

        _bindEvent.call(this);

        return this;
    };

    Grid.prototype.destroy = function () {

        if (this.grid === null) return;

        _unbindEvent.call(this);

        this.grid.parentNode.removeChild(this.grid);

        this.grid = null;
    };

    Grid.prototype.add = function (element) {

        if (_getProperty(element, 'position') !== 'absolute') element.style.position = 'absolute';

        element.style.width = this.columnWidth + 'px';

        element.style.top = this.height[this.column] + this.row * this.options.margin + 'px';

        element.style.left = this.column * this.columnWidth + this.column * this.options.margin + 'px';

        this.height[this.column] += element.clientHeight;

        if (this.options.sorted) {

            if (this.column === this.columns - 1) {

                this.row++;

                this.column = 0;

            } else {

                this.column++;
            }

        } else {

            this.quantity[this.column]++;

            this.column = this.height.indexOf(Math.min.apply(null, this.height));

            this.row = this.quantity[this.column];
        }

        this.grid.style.height = Math.max.apply(null, this.height) + this.options.margin * Math.max.apply(null, this.quantity) + 'px';

        return this;
    };

    /* ----------------------------------------------------------- */
    /* == private methods */
    /* ----------------------------------------------------------- */

    function _build() {

        let self = this;

        [].forEach.call(this.grid.querySelectorAll(this.options.selector), function (item, i) {

            let image = new Image();

            if (item.firstElementChild !== null && item.firstElementChild.nodeName === 'IMG') {

                image.src = item.firstElementChild.src;

                image.onload = function () {

                    self.add(item);
                };

            } else {

                self.add(item);
            }

        });
    }

    function _bindEvent() {

        this._events = {
            windowResize: _handleWindowResize.bind(this)
        };

        if (this.options.responsive.length > 0) {

            window.addEventListener('resize', this._events.windowResize);
        }
    }

    function _unbindEvent() {

        if (this.options.responsive.length > 0) {

            window.removeEventListener('resize', this._events.windowResize);
        }
    }

    function _handleWindowResize(event) {

        _responsiveManagement.call(this);

        _config.call(this);

        _build.call(this);
    }

    function _config() {

        this.row = 0;

        this.column = 0;

        this.columns = this.columns === 0 ? this.options.columns : this.columns;

        this.height = Array(this.columns).fill(0);

        this.quantity = this.height.slice(0);

        this.gridWidth = this.grid.offsetWidth;

        this.columnWidth = this.gridWidth / this.columns - this.options.margin * (this.columns - 1) / this.columns;
    }

    function _responsiveManagement() {

        let responsive = this.options.responsive;

        for (let i = 0; i < responsive.length; i++) {

            if (this.gridWidth < responsive[i][0]) {

                this.columns = responsive[i][1];

                break;

            } else this.columns = 0;
        }
    }

    function _getProperty(element, property) {

        return window.getComputedStyle(element).getPropertyValue(property);
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
        init: Grid
    };

}));
