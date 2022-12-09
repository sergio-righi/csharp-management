
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.customized = factory();
    }

}(this, function () {

    /* ----------------------------------------------------------- */
    /* == component */
    /* ----------------------------------------------------------- */

    function Component(element, options) {

        var defaults = {
            onSomething: null
        };

        this.element = element;

        // extend configurations
        this.options = extend({}, defaults, options);

        // init component
        this.init();
    }

    Component.prototype.init = function () {

        if (this.component) return;

        _build.call(this);
        _bindEvent.call(this);

        // insert component
        document.body.insertBefore(this.component, document.body.firstChild);

        return this;
    };

    Component.prototype.destroy = function () {

        if (this.component === null) return;

        // unbind all events
        _unbindEvent.call(this);

        // remove component from dom
        this.component.parentNode.removeChild(this.component);

        this.component = null;
    };

    Component.prototype.funcionaName = function () {

        // callback function
        if (typeof this.options.onSomething === 'function') {

            this.options.onSomething.call(this);
        }
    };

    Component.prototype.setFunctionName = function () {

        return this;
    };

    Component.prototype.getFuncionName = function () {

        // return something
    };

    /* ----------------------------------------------------------- */
    /* == private methods */
    /* ----------------------------------------------------------- */

    function _build() {

        // build component
    }

    function _bindEvent() {

        this._events = {
            functionHandle: _handleFunction.bind(this),
            methodFunction: this.funcionaName.bind(this)
        };

        this.component.addEventListener('click', this._events.methodFunction);
    }

    function _unbindEvent() {

        this.component.removeEventListener('click', this._events.methodFunction);
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
        component: Component
    };

}));
