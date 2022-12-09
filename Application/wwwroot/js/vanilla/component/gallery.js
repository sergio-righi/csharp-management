
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

    let nextIndex = 0;
    let forced = false;

    function Gallery(element, options) {

        this.endX = 0;
        this.index = 0;
        this.items = [];
        this.startX = 0;
        this.distance = 300;
        this.animated = null;
        this.element = element;

        var defaults = {
        };

        this.options = extend({}, defaults, options);

        this.init();
    }

    Gallery.prototype.init = function () {

        _build.call(this);

        _config.call(this);

        _bindEvent.call(this);

        //if (typeof this.options.onSomething === 'function') {

        //    this.options.onSomething.call(this);
        //}

        return this;
    };

    Gallery.prototype.destroy = function () {

        if (this.component === null) return;

        _unbindEvent.call(this);

        this.component.parentNode.removeChild(this.component);

        this.component = null;
    };

    Gallery.prototype.open = function (element) {

        let self = this;

        this.animated = element.cloneNode(false);

        const configuration = dataStorage.get(element, 'configuration');

        this.animated.style.zIndex = '999';
        this.animated.style.position = 'absolute';
        this.animated.style.top = configuration.start.top + 'px';
        this.animated.style.left = configuration.start.left + 'px';
        this.animated.style.width = configuration.start.width + 'px';
        this.animated.style.height = configuration.start.height + 'px';

        document.body.appendChild(this.animated);

        console.log(configuration);

        /* jquery */

        $(this.animated).animate({

            top: configuration.end.top + 'px',
            left: configuration.end.left + 'px',
            width: configuration.end.width + 'px',
            height: configuration.end.height + 'px'

        }, 300, function () {

            this.parentNode.removeChild(this);

            self.gallery.className += ' ativo fixo';

        });

        /* end */
    };

    Gallery.prototype.close = function () {

    };

    Gallery.prototype.add = function (element) {

        let self = this;

        let image = new Image();

        image.src = element.src;

        image.onload = function () {

            dataStorage.put(element, 'configuration', setConfiguration(element));

            _insertElement.call(self, element);
        }

        return this;
    };

    Gallery.prototype.moveTo = function (index) {

        forced = true;

        nextIndex = index;

        this.content.classList.add('animacao-transicao');

        this.content.style.transform = `translateX(-${this.width * nextIndex}px)`;

        this.content.addEventListener('transitionend', this._events.transitionEnd);

        return this;
    };

    Gallery.prototype.next = function () {

        return this.moveTo(this.index + 1);
    };

    Gallery.prototype.prev = function () {

        return this.moveTo(this.index - 1);
    };

    /* ----------------------------------------------------------- */
    /* == private methods */
    /* ----------------------------------------------------------- */

    function _build() {

        this.gallery = document.createElement('div');
        this.gallery.className = 'galeria-componente';

        let background = document.createElement('div');
        background.className = 'galeria-fundo';

        this.wrapper = document.createElement('div');
        this.wrapper.className = 'galeria-conteudo';

        this.content = document.createElement('div');
        this.content.className = 'galeria-itens';

        let control = document.createElement('div');
        control.className = 'galeria-controle';

        this.closeButton = document.createElement('div');
        this.closeButton.className = 'galeria-fechar';

        this.nextButton = document.createElement('div');
        this.nextButton.className = 'galeria-proximo';

        this.prevButton = document.createElement('div');
        this.prevButton.className = 'galeria-anterior';

        control.appendChild(this.nextButton);
        control.appendChild(this.prevButton);
        control.appendChild(this.closeButton);

        this.wrapper.appendChild(this.content);

        this.gallery.appendChild(background);
        this.gallery.appendChild(this.wrapper);
        this.gallery.appendChild(control);

        document.body.appendChild(this.gallery);
    }

    function _bindEvent() {

        this._events = {
            arrowClick: _handleArrowClick.bind(this),
            imageClick: _handleImageClick.bind(this),
            closeClick: _handleCloseClick.bind(this),
            galleryDrag: _handleGalleryDrag.bind(this),
            windowResize: _handleWindowResize.bind(this),
            transitionEnd: _handleTransitionEnd.bind(this),
            galleryMouseUp: _handleGalleryMouseUp.bind(this),
            galleryMouseMove: _handleGalleryMouseMove.bind(this)
        };

        for (var key in this.items) {

            this.items[key].addEventListener('click', this._events.imageClick);
        }

        window.addEventListener('resize', this._events.windowResize);

        this.closeButton.addEventListener('click', this._events.closeClick);
        this.prevButton.addEventListener('click', this._events.arrowClick.bind(this, 'left'));
        this.nextButton.addEventListener('click', this._events.arrowClick.bind(this, 'right'));


        this.content.addEventListener('dragstart', this._events.galleryDrag);
        this.content.addEventListener('mouseup', this._events.galleryMouseUp);
        this.content.addEventListener('mousemove', this._events.galleryMouseMove);

        //if (!this.mobile) {

        //} else {

        //    this.content.addEventListener('touchstart', this._events.galleryDrag);
        //    this.content.addEventListener('touchend', this._events.galleryMouseUp);
        //    this.content.addEventListener('touchmove', this._events.galleryMouseMove);
        //}
    }

    function _unbindEvent() {

        for (var key in this.items) {

            this.items[key].removeEventListener('click', this._events.imageClick);
        }

        window.removeEventListener('resize', this._events.windowResize);

        this.closeButton.removeEventListener('click', this._events.closeClick);
        this.prevButton.removeEventListener('click', this._events.arrowClick.bind(this, 'left'));
        this.nextButton.removeEventListener('click', this._events.arrowClick.bind(this, 'right'));

        this.content.removeEventListener('dragstart', this._events.galleryDrag);
        this.content.removeEventListener('mouseup', this._events.galleryMouseUp);
        this.content.removeEventListener('mousemove', this._events.galleryMouseMove);
    }

    function _handleImageClick(event) {

        this.open(event.target);
    }

    function _handleCloseClick(event) {

        this.close();
    }

    function _handleArrowClick(direction, event) {

        if (direction === 'right') {

            this.next();

        } else if (direction === 'left') {

            this.prev();
        }
    }

    function _handleWindowResize(event) {

        _config.call(this);
    }

    function _handleGalleryDrag(event) {

        //if (this.mobile) {

        //    this.startX = event.touches[0].clientX;

        //} else {

        //}

        this.startX = event.pageX;

        event.preventDefault();
    }

    function _handleGalleryMouseMove(event) {

        //if (this.mobile) {

        //    this.endX = event.touches[0].clientX;

        //} else {

        //}

        this.endX = event.pageX;

        _move.call(this);
    }

    function _handleGalleryMouseUp(event) {

        _change.call(this);
    }

    function _handleTransitionEnd(event) {

        this.content.removeEventListener('animationend', this._events.transitionEnd);

        this.content.classList.remove('animacao-transicao');

        if (forced) {

            this.index = nextIndex;

            if (nextIndex === this.content.children.length - 1) {

                this.index = 1;

            } else if (nextIndex === 0) {

                this.index = this.content.children.length - 2;
            }

            if (nextIndex !== this.index) {

                this.content.style.transform = `translateX(-${this.width * this.index}px)`;
            }
        }
    }

    function _config() {

        this.content.innerHTML = '';

        if (this.element.nodeName === 'IMG') {

            this.items = this.add(this.element);

        } else {

            let self = this;

            this.items = [].filter.call(this.element.querySelectorAll('img'), function (item) {

                if (item.parentNode.nodeName !== 'A') {

                    self.add(item);

                    return item;
                }

            });
        }
    }

    function _insertElement(element) {

        let clone = element.cloneNode(false);

        let container = document.createElement('div');
        container.className = 'galeria-item';

        const configuration = dataStorage.get(element, 'configuration');

        clone.style.width = configuration.end.width + 'px';
        clone.style.height = configuration.end.height + 'px';

        this.content.appendChild(container);

        configuration.index = Array.prototype.slice.call(this.content.children).indexOf(container);
    }

    function _change() {

        let distance = (this.endX - this.startX) * 2;

        if (this.startX > 0) {

            forced = false;

            if (distance > this.distance) {

                this.prev();

            } else if (distance < -this.distance) {

                this.next();

            } else {

                nextIndex = this.index;

                this.content.classList.add('animacao-transicao');
                this.content.style.transform = `translateX(-${this.width * this.index}px)`;

                this.content.addEventListener('transitionend', this._events.transitionEnd);
            }

            this.startX = 0;
        }
    }

    function _move() {

        if (this.startX > 0) {

            let transition = this.index * this.width;

            let distance = this.endX - this.startX;

            if (transition - distance <= this.width * (this.content.children.length - 1)) {

                this.content.style.transform = `translateX(-${transition - distance}px)`;
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

    function setConfiguration(element) {

        const offset = element.offset();

        return {
            index: -1,
            width: element.naturalWidth,
            height: element.naturalHeight,
            start: {
                top: offset.top,
                left: offset.left,
                width: element.offsetWidth,
                height: element.offsetHeight
            },
            end: getPosition(element.naturalWidth, element.naturalHeight)
        };
    }

    function getPosition(width, height) {

        let proportion = Math.min(window.innerWidth / width, window.innerHeight / height);

        let dimension = { width: width * proportion, height: height * proportion };

        return {
            width: dimension.width,
            height: dimension.height,
            left: (window.innerWidth - dimension.width) / 2 + window.scrollX,
            top: (window.innerHeight - dimension.height) / 2 + window.scrollY
        };
    }

    Element.prototype.offset = function () {

        let rect = this.getBoundingClientRect(),
            scrollLeft = window.pageXOffset || document.documentElement.scrollLeft,
            scrollTop = window.pageYOffset || document.documentElement.scrollTop;

        return { top: rect.top + scrollTop, left: rect.left + scrollLeft }
    }

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
    }

    /* ----------------------------------------------------------- */
    /* == return */
    /* ----------------------------------------------------------- */

    return {
        gallery: Gallery
    };

}));

//element.animate({
//    offset: [configuration.end.left, configuration.end.top],
//    easing: ['ease-in', 'ease-out']
//}, 1000);

//let fps = 60;
//let duration = 2;
//let start = configuration.left;
//let finish = position.left;
//let distance = finish - start;
//let increment = distance / (duration * fps);

//let step = start;

//function move() {
//    step += increment;
//    if (step >= finish) {
//        clearInterval(handler);
//        element.style.left = finish + 'px';
//        return;
//    }
//    element.style.left = step + 'px';
//}

//let handler = setInterval(move, 1000 / fps);