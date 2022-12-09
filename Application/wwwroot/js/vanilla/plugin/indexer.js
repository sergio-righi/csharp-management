
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.indexer = factory();
    }

}(this, function () {

    const Animation =
    {
        DURATION: 300
    };

    /* ----------------------------------------------------------- */
    /* == component */
    /* ----------------------------------------------------------- */

    function Indexer(element, options) {

        this.items = [];
        this.busy = false;
        this.characters = [];
        this.selected = null;
        this.touchStarted = false;

        var defaults = {
            selector: '',
            onCreate: null,
            shortcut: true,
            highlight: true,
            navigation: true,
            highlightClass: 'active'
        };

        this.element = element;

        // extend configurations
        this.options = extend({}, defaults, options);

        // init component
        this.init();
    }

    Indexer.prototype.init = function () {

        if (this.element === undefined) return;

        _build.call(this);

        if (this.options.navigation) {

            _buildNav.call(this);

            _bindEvent.call(this);
        }

        if (typeof this.options.onCreate === 'function') {

            this.options.onCreate.call(this);
        }

        return this;
    };

    Indexer.prototype.destroy = function () {

        if (this.component === null) return;

        _unbindEvent.call(this);

        this.component.parentNode.removeChild(this.component);

        this.component = null;
    };

    Indexer.prototype.setSituation = function (id, active, cls) {

        let header = document.querySelectorAll(`[data-header="${id}"]`)[0];

        if (header !== undefined) {

            if (active) {

                header.classList.add(cls);

            } else {

                header.classList.remove(cls);
            }
        }

        if (this.options.navigation) {

            let navigation = document.querySelectorAll(`[data-nav="${id}"]`)[0];

            if (navigation !== undefined) {

                if (active) {

                    navigation.classList.add(cls);

                } else {

                    navigation.classList.remove(cls);
                }
            }
        }

        return this;
    };

    Indexer.prototype.setHighlight = function (id) {

        if (this.options.highlight) {

            let currentHeader = document.querySelectorAll(`[data-header].${this.options.highlightClass}`)[0];

            if (currentHeader !== undefined) {

                currentHeader.classList.remove(this.options.highlightClass);
            }

            let header = this.element.querySelectorAll(`[data-header="${id}"]`)[0];

            if (header !== undefined) {

                header.classList.add(this.options.highlightClass);
            }

            if (this.options.navigation) {

                let currentNav = document.querySelectorAll(`[data-nav].${this.options.highlightClass}`)[0];

                if (currentNav !== undefined) {

                    currentNav.classList.remove(this.options.highlightClass);
                }

                let navigation = document.querySelectorAll(`[data-nav="${id}"]`)[0];

                if (navigation !== undefined) {

                    navigation.classList.add(this.options.highlightClass);
                }
            }
        }
    };

    Indexer.prototype.moveTo = function (id, duration) {

        if (!this.busy) {

            let header = this.element.querySelectorAll(`[data-header="${id}"]`)[0];

            if (header !== undefined) {

                let self = this;

                self.busy = true;

                scrollTo(document.documentElement, header.offsetTop, duration, function () {

                    self.setHighlight(id);

                    setTimeout(function () {

                        self.busy = false;

                    }, 100);

                });
            }
        }
    };

    /* ----------------------------------------------------------- */
    /* == private methods */
    /* ----------------------------------------------------------- */

    function _build() {

        let self = this;
        let contentHTML = [], letter = '';

        this.items = Array.prototype.slice.call(this.element.querySelectorAll(this.options.selector), 0);

        Array.prototype.sort.call(this.items, function (itemA, itemB) {

            let aValue = itemA.dataset.title.trim();

            let bValue = itemB.dataset.title.trim();

            return aValue.toString().toUpperCase().localeCompare(bValue.toString().toUpperCase());

        });

        Array.prototype.forEach.call(this.items, function (item, i) {

            let firstLetter = item.dataset.title.substring(0, 1).toUpperCase().toISO();

            if (letter !== firstLetter) {

                letter = firstLetter;

                self.characters.push(letter);

                contentHTML.push(`<div class="title-item" data-header="${letter.toUpperCase()}">${letter.toUpperCase()}</div>`);
            }

            contentHTML.push(item.outerHTML);

        });

        this.element.innerHTML = contentHTML.join('');
    }

    function _buildNav() {

        this.navigation = document.createElement('div');
        this.navigation.className = 'indexer';

        let contentHTML = [];

        for (let i = 0; i < this.characters.length; i++) {

            contentHTML.push(`<div class="navigation-item" data-nav="${this.characters[i]}">${this.characters[i]}</div>`);
        }

        this.navigation.innerHTML = contentHTML.join('');

        this.element.parentNode.insertBefore(this.navigation, this.element.parentNode.firstElementChild);
    }

    function _bindEvent() {

        this._events = {
            windowScroll: _handleWindowScroll.bind(this),
            windowKeyDown: _handleWindowKeyDown.bind(this),
            navigationMouseUp: _handleNavigationMouseUp.bind(this),
            navigationTouchEnd: _handleNavigationTouchEnd.bind(this),
            navigationTouchMove: _handleNavigationTouchMove.bind(this),
            navigationTouchStart: _handleNavigationTouchStart.bind(this),
        };

        if (this.options.highlight) {

            window.addEventListener('scroll', this._events.windowScroll);
        }

        if (this.options.shortcut) {

            window.addEventListener('keydown', this._events.windowKeyDown);
        }

        if (this.options.navigation) {
            
            this.navigation.addEventListener('mouseup', this._events.navigationMouseUp);
            this.navigation.addEventListener('touchend', this._events.navigationTouchEnd);
            this.navigation.addEventListener('touchmove', this._events.navigationTouchMove);
            this.navigation.addEventListener('touchstart', this._events.navigationTouchStart);
        }
    }

    function _unbindEvent() {

        if (this.options.highlight) {

            window.removeEventListener('scroll', this._events.windowScroll);
        }

        if (this.options.shortcut) {

            window.removeEventListener('keydown', this._events.windowKeyDown);
        }

        if (this.options.navigation) {
            
            this.navigation.removeEventListener('mouseup', this._events.navigationMouseUp);
            this.navigation.removeEventListener('touchend', this._events.navigationTouchEnd);
            this.navigation.removeEventListener('touchmove', this._events.navigationTouchMove);
            this.navigation.removeEventListener('touchstart', this._events.navigationTouchStart);
        }
    }

    function _handleWindowScroll(event) {

        if (!this.busy) {

            for (let i = this.characters.length - 1; i >= 0; i--) {

                let element = document.querySelectorAll(`[data-header="${this.characters[i]}"]`)[0];

                if (element !== undefined) {

                    const offsetTop = (element.getBoundingClientRect().top + document.documentElement.scrollTop);

                    if (window.pageYOffset >= (offsetTop - element.outerHeight())) {

                        this.setHighlight(this.characters[i]);
                        break;
                    }
                }
            }
        }
    }

    function _handleWindowKeyDown(event) {

        if (event.altKey) {

            let characterCode = event.which;

            let character = String.fromCharCode(characterCode);

            if (this.characters.indexOf(character) !== -1) {

                this.moveTo(character, Animation.DURATION);
            }
        }
    }

    function _handleNavigationMouseUp(event) {

        if (!this.touchStarted && event.target && event.target.className === 'navigation-item') {

            this.moveTo(event.target.dataset.nav, Animation.DURATION);
        }
    }

    function _handleNavigationTouchStart(event) {
        
        this.selected = null;
        this.touchStarted = true;
    }

    function _handleNavigationTouchEnd(event) {
        
        this.touchStarted = false;

        if (this.selected !== null) {

            this.moveTo(this.selected.dataset.nav, 10);

            this.selected = null;
        }
    }

    function _handleNavigationTouchMove(event) {

        if (this.touchStarted) {

            event.preventDefault();

            let element = document.elementFromPoint(event.touches[0].clientX, event.touches[0].clientY);

            if (typeof element !== undefined) {

                this.selected = element;

                this.moveTo(element.dataset.nav, 10);
            }
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

    function scrollTo(from, to, duration, callback) {

        if (duration <= 0) return;

        var difference = to - from.scrollTop;

        var perTick = difference / duration * 10;

        setTimeout(function () {

            from.scrollTop = from.scrollTop + perTick;

            if (from.scrollTop === to) {

                callback();
                return;
            }

            scrollTo(from, to, duration - 10, callback);

        }, 10);
    }

    String.prototype.toISO = function () {

        return this.replace(new RegExp('[ÁÀÂÃÄ]', 'gi'), 'A').replace(new RegExp('[ÉÈÊË]', 'gi'), 'E').replace(new RegExp('[ÍÌÎÏ]', 'gi'), 'I').replace(new RegExp('[ÓÒÔÕÖ]', 'gi'), 'O').replace(new RegExp('[ÚÙÛÜ]', 'gi'), 'U').replace(new RegExp('[Ç]', 'gi'), 'C');
    };

    Element.prototype.outerHeight = function () {

        let estilo = window.getComputedStyle(this);

        let margem = parseFloat(estilo['marginTop'], 10) + parseFloat(estilo['marginBottom'], 10);

        return Math.ceil(this.offsetHeight + margem);
    };

    /* ----------------------------------------------------------- */
    /* == return */
    /* ----------------------------------------------------------- */

    return {
        init: Indexer
    };

}));
