
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.carousel = factory();
    }

}(this, function () {

    /* ----------------------------------------------------------- */
    /* == component */
    /* ----------------------------------------------------------- */

    let forced = false;
    let moveToIndex = 0;

    function Carousel(element, options) {

        this.index = 0;
        this.width = 0;

        this.locked = false;
        this.stopped = false;
        this.slideshow = 5000;

        this.distance = 200;
        this.positionXEnd = 0;
        this.positionXStart = 0;

        this.mobile = isMobile();

        var defaults = {
            hide: true,
            loop: true,
            arrow: true,
            rewind: false,
            draggable: true,
            slideshow: false,
            resizeable: true,
            control: 'number'
        };

        this.carousel = element;

        this.options = extend({}, defaults, options);

        this.init();
    }

    Carousel.prototype.init = function () {

        if (this.carousel !== undefined && this.carousel.children.length > 1) {

            _build.call(this);

            _config.call(this);

            _bindEvent.call(this);
        }

        return this;
    };

    Carousel.prototype.destroy = function () {

        if (this.carousel === null) return;

        _unbindEvent.call(this);

        this.carousel.parentNode.removeChild(this.carousel);

        this.carousel = null;
    };

    Carousel.prototype.moveTo = function (index) {

        if (!this.locked) {

            forced = true;

            this.locked = true;

            this.carouselItems.classList.add('animation-transition');

            if (this.options.loop) {

                moveToIndex = index;

            } else {

                if (index >= this.carouselItems.children.length) {

                    this.index = this.options.rewind ? 0 : this.carouselItems.children.length - 1;

                } else if (index < 0) {

                    this.index = this.options.rewind ? this.carouselItems.children.length - 1 : 0;

                } else {

                    this.index = index;
                }

                moveToIndex = this.index;
            }

            this.carouselItems.style.transform = `translateX(-${this.width * moveToIndex}px)`;

            this.carouselItems.addEventListener('transitionend', this._events.transitionEnd);

            //if (typeof this.options.onSomething === 'function') {

            //    this.options.onSomething.call(this);
            //}
        }
    };

    Carousel.prototype.next = function () {

        this.moveTo(this.index + 1);

        return this;
    };

    Carousel.prototype.prev = function () {

        this.moveTo(this.index - 1);

        return this;
    };

    /* ----------------------------------------------------------- */
    /* == private methods */
    /* ----------------------------------------------------------- */

    function _build() {

        this.items = this.carousel.querySelectorAll('.item');

        let carouselContent = document.createElement('div');
        carouselContent.className = 'carousel-content';

        this.carouselItems = document.createElement('div');
        this.carouselItems.className = 'carousel-items';

        this.controlContent = document.createElement('div');
        this.controlContent.className = 'carousel-control';

        let self = this;

        [].forEach.call(this.items, function (item, i) {

            self.carouselItems.appendChild(item);

        });

        carouselContent.appendChild(this.carouselItems);

        this.carousel.innerHTML = '';
        this.carousel.appendChild(carouselContent);

        carouselContent.appendChild(this.controlContent);

        if (this.options.loop) {

            let cloneElementLast = this.items[this.carouselItems.children.length - 1].cloneNode(true);
            cloneElementLast.className = 'item-clone';

            let cloneElementFirst = this.items[0].cloneNode(true);
            cloneElementFirst.className = 'item-clone';

            this.carouselItems.appendChild(cloneElementFirst);

            this.carouselItems.insertBefore(cloneElementLast, this.carouselItems.firstElementChild);
        }

        if (this.options.control !== null) {

            this.control = document.createElement('div');
            this.control.className = 'control';

            this.controlContent.appendChild(this.control);
        }

        if (this.options.arrow) {

            this.arrowRight = document.createElement('div');
            this.arrowRight.className = 'control-right';

            this.arrowLeft = document.createElement('div');
            this.arrowLeft.className = 'control-left';

            this.controlContent.appendChild(this.arrowLeft);
            this.controlContent.appendChild(this.arrowRight);
        }

        if (!this.options.hide) {

            this.controlContent.classList.add('active');
        }
    }

    function _bindEvent() {

        this._events = {
            arrowClick: _handleArrowClick.bind(this),
            windowResize: _handleWindowResize.bind(this),
            controlClick: _handleControlClick.bind(this),
            carouselDrag: _handleCarouselDrag.bind(this),
            carouselClick: _handleCarouselClick.bind(this),
            transitionEnd: _handleTransitionEnd.bind(this),
            carouselMouseUp: _handleCarouselMouseUp.bind(this),
            carouselMouseMove: _handleCarouselMouseMove.bind(this),
            carouselMouseOver: _handleCarouselMouseOver.bind(this),
            carouselMouseLeave: _handleCarouselMouseLeave.bind(this)
        };

        if (this.options.resizeable) {

            window.addEventListener('resize', this._events.windowResize);
        }

        if (this.options.arrow) {

            this.arrowLeft.addEventListener('click', this._events.arrowClick.bind(this, 'left'));
            this.arrowRight.addEventListener('click', this._events.arrowClick.bind(this, 'right'));
        }

        if (this.options.control !== null) {

            this.control.addEventListener('click', this._events.controlClick);
        }

        if (!this.mobile && (this.options.hide || this.options.slideshow)) {

            this.carousel.addEventListener('mouseover', this._events.carouselMouseOver);
            this.carousel.addEventListener('mouseleave', this._events.carouselMouseLeave);
        }

        if (this.options.draggable) {

            if (!this.mobile) {

                this.carouselItems.addEventListener('dragstart', this._events.carouselDrag);
                this.carouselItems.addEventListener('mouseup', this._events.carouselMouseUp);
                this.carouselItems.addEventListener('mousemove', this._events.carouselMouseMove);

            } else {

                this.carouselItems.addEventListener('touchstart', this._events.carouselDrag);
                this.carouselItems.addEventListener('touchend', this._events.carouselMouseUp);
                this.carouselItems.addEventListener('touchmove', this._events.carouselMouseMove);
            }
        }

        this.carousel.addEventListener('click', this._events.carouselClick);
    }

    function _unbindEvent() {

        if (this.options.resizeable) {

            window.removeEventListener('resize', this._events.windowResize);
        }

        if (this.options.arrow) {

            this.arrowLeft.removeEventListener('click', this._events.arrowClick.bind(this, 'left'));
            this.arrowRight.removeEventListener('click', this._events.arrowClick.bind(this, 'right'));
        }

        if (this.options.control !== null) {

            this.control.removeEventListener('click', this._events.controlClick);
        }

        if (!this.mobile && (this.options.hide || this.options.slideshow)) {

            this.carousel.removeEventListener('mouseover', this._events.carouselMouseOver);
            this.carousel.removeEventListener('mouseleave', this._events.carouselMouseLeave);
        }

        if (this.options.draggable) {

            if (!this.mobile) {

                this.carouselItems.removeEventListener('dragstart', this._events.carouselDrag);
                this.carouselItems.removeEventListener('mouseup', this._events.carouselMouseUp);
                this.carouselItems.removeEventListener('mousemove', this._events.carouselMouseMove);

            } else {

                this.carouselItems.removeEventListener('touchstart', this._events.carouselDrag);
                this.carouselItems.removeEventListener('touchend', this._events.carouselMouseUp);
                this.carouselItems.removeEventListener('touchmove', this._events.carouselMouseMove);
            }
        }

        this.carousel.removeEventListener('click', this._events.carouselClick);
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

    function _handleControlClick(event) {

        if (event.target && event.target.classList.contains('control-item')) {

            let index = Array.prototype.slice.call(this.control.children).indexOf(event.target);

            index = this.options.loop ? index + 1 : index;

            this.moveTo(index);
        }
    }

    function _handleCarouselClick(event) {

        if (this.locked && event.target.parentNode.nodeName === 'A') {

            event.preventDefault();
        }
    }

    function _handleCarouselDrag(event) {

        this.stopped = true;

        if (this.mobile) {

            this.positionXStart = event.touches[0].clientX;

        } else {

            this.positionXStart = event.pageX;
        }

        event.preventDefault();
    }

    function _handleCarouselMouseMove(event) {

        if (this.mobile) {

            this.positionXEnd = event.touches[0].clientX;

        } else {

            this.positionXEnd = event.pageX;
        }

        _move.call(this);
    }

    function _handleCarouselMouseUp(event) {

        this.stopped = false;

        _change.call(this);
    }

    function _handleCarouselMouseOver(event) {

        if (this.options.hide) {

            this.controlContent.classList.add('active');
        }

        this.stopped = true;
    }

    function _handleCarouselMouseLeave(event) {

        if (this.positionXStart > 0) {

            this.positionXStart = 0;
            this.moveTo(this.index);
        }

        if (this.options.hide) {

            this.controlContent.classList.remove('active');
        }

        this.stopped = false;
    }

    function _handleTransitionEnd(event) {

        this.carouselItems.removeEventListener('animationend', this._events.transitionEnd);

        this.carouselItems.classList.remove('animation-transition');

        if (forced) {

            if (this.options.loop) {

                this.index = moveToIndex;

                if (moveToIndex === this.carouselItems.children.length - 1) {

                    this.index = 1;

                } else if (moveToIndex === 0) {

                    this.index = this.carouselItems.children.length - 2;
                }

                if (moveToIndex !== this.index) {

                    this.carouselItems.style.transform = `translateX(-${this.width * this.index}px)`;
                }

            } else {

                _arrowManagement.call(this);
            }

            _controlManagement.call(this);
        }

        this.locked = false;
    }

    function _config() {

        this.width = this.carousel.clientWidth;

        let controlHTML = [];

        for (let i = 0; i < this.items.length; i++) {

            this.items[i].style.width = this.width + 'px';

            controlHTML.push(`<span class="control-item ${this.options.control}${(i === 0 ? ' active' : '')}">${(/number/.test(this.options.control) ? i + 1 : '')}</span>`);
        }

        if (this.options.control !== null) {

            this.control.innerHTML = controlHTML.join('');
        }

        if (this.options.slideshow) {

            this.slideshow = this.carousel.dataset.slideshow !== undefined ? this.carousel.dataset.slideshow : this.slideshow;

            _slideshowManagement.call(this);
        }

        if (this.options.loop) {

            let items = this.carouselItems.querySelectorAll('.item-clone');

            for (let i = 0; i < items.length; i++) {

                items[i].style.width = this.width + 'px';
            }

            this.carouselItems.style.transform = `translateX(-${this.width}px)`;

            this.index = 1;

        } else {

            if (this.options.arrow) {

                _arrowManagement.call(this);
            }
        }

        this.carouselItems.style.width = this.width * this.carouselItems.children.length + 'px';
    }

    function _controlManagement() {

        if (this.options.control !== null) {

            let elements = this.control.children;

            let index = this.options.loop ? this.index - 1 : this.index;

            let item = this.control.querySelectorAll('.active')[0];

            if (item !== undefined) {

                item.classList.remove('active');
            }

            elements[index].classList.add('active');
        }
    }

    function _arrowManagement() {

        if (this.options.arrow) {

            if (this.carouselItems.children.length < 1) {

                this.arrowLeft.classList.remove('d-none');
                this.arrowRight.classList.remove('d-none');

            } else {

                if (this.index === 0) {

                    this.arrowLeft.classList.add('d-none');

                } else if (this.index === this.carouselItems.children.length - 1) {

                    this.arrowRight.classList.add('d-none');

                } else {

                    this.arrowLeft.classList.remove('d-none');
                    this.arrowRight.classList.remove('d-none');
                }
            }
        }
    }

    function _slideshowManagement() {

        let self = this;

        setInterval(function () {

            if (!self.stopped) {

                self.next();
            }

        }, this.slideshow);
    }

    function _change() {

        let distance = (this.positionXEnd - this.positionXStart) * 2;

        if (this.positionXStart > 0) {

            forced = false;

            if (distance > this.distance) {

                this.prev();

            } else if (distance < -this.distance) {

                this.next();

            } else {

                moveToIndex = this.index;

                this.carouselItems.classList.add('animation-transition');
                this.carouselItems.style.transform = `translateX(-${this.width * this.index}px)`;

                this.carouselItems.addEventListener('transitionend', this._events.transitionEnd);
            }

            this.positionXStart = 0;
        }
    }

    function _move() {

        if (this.positionXStart > 0) {

            let quantity = this.carouselItems.children.length;

            if (this.options.loop || (this.index >= 0 && this.index < quantity)) {

                let transition = this.index * this.width;

                let distance = this.positionXEnd - this.positionXStart;

                if (transition - distance <= this.width * (quantity - 1)) {

                    this.carouselItems.style.transform = `translateX(-${transition - distance}px)`;
                }
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

    function isMobile() {

        let mobile = false;

        (function (a) {

            if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino|android|ipad|playbook|silk/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) {

                check = true;
            }

        })(navigator.userAgent || navigator.vendor || window.opera);

        return mobile;
    }

    /* ----------------------------------------------------------- */
    /* == return */
    /* ----------------------------------------------------------- */

    return {
        init: Carousel
    };

}));
