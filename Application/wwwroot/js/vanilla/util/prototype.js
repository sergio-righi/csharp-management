
/*
 * - is function defined -
 */

Function.prototype.isDefined = function () {

    return typeof this !== 'undefined' && $.isFunction(this);
};

/*
 * - remove accentuation -
 */

String.prototype.removeAccentuation = function () {

    return this.toUpperCase().replace(new RegExp('[ÁÀÂÃÄ]', 'gi'), 'A').replace(new RegExp('[ÉÈÊË]', 'gi'), 'E').replace(new RegExp('[ÍÌÎÏ]', 'gi'), 'I').replace(new RegExp('[ÓÒÔÕÖ]', 'gi'), 'O').replace(new RegExp('[ÚÙÛÜ]', 'gi'), 'U').replace(new RegExp('[Ç]', 'gi'), 'C').toLowerCase();
};

/*
 * - is JSON -
 */

String.prototype.isJSON = function () {

    try {

        JSON.parse(this);

    } catch (e) {

        return false;
    }

    return true;
};

/*
 * - Date -
 */

String.prototype.isDate = function () {

    return (new Date(this) !== 'Invalid Date') && !isNaN(new Date(this));
};

String.prototype.toDate = function () {
    const [day, month, year] = this.split('/');
    return new Date(year, month - 1, day);
};

/*
 * - number -
 */

Number.prototype.toTwoDigitFormat = function () {
    return this < 0 ? "00" : ('0' + this).slice(-2);
}

/*
 * - string format -
 */

if (!String.prototype.format) {
    String.format = function (format) {
        var args = Array.prototype.slice.call(arguments, 1);
        return format.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] !== 'undefined'
                ? args[number]
                : match
                ;
        });
    };
    //String.prototype.format = function () {
    //    var args = arguments;

    //    if (typeof args[0] !== "object") {
    //        return this.replace(/{\d+}/g, function (m) {
    //            var index = Number(m.replace(/\D/g, ""));
    //            return args[index] ? args[index] : m;
    //        });
    //    }
    //    else {
    //        var obj = args[0],
    //            keys = Object.keys(obj);

    //        return this.replace(/{\w+}/g, function (m) {
    //            var key = m.replace(/{|}/g, "");
    //            return obj.hasOwnProperty(key) ? obj[key] : m;
    //        });
    //    }
    //};
};

/*
 * - find selector -
 */

Element.prototype.matches = Element.prototype.matches || Element.prototype.webkitMatchesSelector || Element.prototype.msMatchesSelector || Element.prototype.mozMatchesSelector;

/*
 * - closest -
 */

Element.prototype.closestClass = function (cls) {
    let element = this;
    if (!element.classList.contains(cls))
        while ((element = element.parentElement) && !element.classList.contains(cls));
    return element;
};

Element.prototype.furthestClass = function (cls) {
    let anchor = null;
    let element = this.parentElement || this.parentNode;

    while (element !== null && element.nodeType === 1) {
        if (element.matches(cls)) anchor = element;
        element = element.parentElement || element.parentNode;
    }
    return anchor;
};

Element.prototype.closestNode = function (nodeName) {
    let element = this;
    if (element.nodeName !== nodeName.toUpperCase())
        while ((element = element.parentElement) && element.nodeName !== nodeName.toUpperCase());
    return element;
};

Element.prototype.closestAttribute = function (attributeName) {
    let element = this;
    if (!element.hasAttribute(attributeName))
        while ((element = element.parentElement) && !element.hasAttribute(attributeName));
    return element;
};

/*
 * - next -
 */

Element.prototype.next = function (selector) {
    let element = this;
    while (element !== document.body) {
        element = element.nextElementSibling;
        if (element.matches(selector)) {
            return element;
        }
    }
};

/*
 * - position -
 */

Element.prototype.isInViewport = function () {
    let element = this;
    let top = element.offsetTop;
    let left = element.offsetLeft;
    let width = element.offsetWidth;
    let height = element.offsetHeight;

    while (element.offsetParent) {
        element = element.offsetParent;
        top += element.offsetTop;
        left += element.offsetLeft;
    }

    return (
        top < (window.pageYOffset + window.innerHeight) &&
        left < (window.pageXOffset + window.innerWidth) &&
        (top + height) > window.pageYOffset &&
        (left + width) > window.pageXOffset
    );
};

/*
 * - serializable (data) -
 */

window.dataStorage = {
    _storage: new WeakMap(),
    put: function (element, key, obj) {
        if (!this._storage.has(element)) {
            this._storage.set(element, new Map());
        }
        this._storage.get(element).set(key, obj);
    },
    get: function (element, key) {
        return this._storage.get(element).get(key);
    },
    has: function (element, key) {
        return this._storage.has(element) && this._storage.get(element).has(key);
    },
    remove: function (element, key) {
        var ret = this._storage.get(element).delete(key);
        if (!this._storage.get(element).size === 0) {
            this._storage.delete(element);
        }
        return ret;
    }
};