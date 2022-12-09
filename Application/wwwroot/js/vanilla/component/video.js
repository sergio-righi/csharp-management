
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.video = factory();
    }

}(this, function () {

    /* ----------------------------------------------------------- */
    /* == component */
    /* ----------------------------------------------------------- */

    function Video(element, options) {

        this.timer = 0;

        var defaults = {
            closed: false,
            loaded: false,
            running: false,
            theater: false,
            undocked: false,
            fullscreen: false,
            quality: ['Automático'],
            speed: [0.25, 0.5, 0.75, 1, 1.25, 1.5, 2],
            connection: navigator.connection || navigator.mozConnection || navigator.webkitConnection
        };

        this.element = element;

        this.options = extend({}, defaults, options);

        this.init();
    }

    Video.prototype.init = function () {

        if (this.element === undefined) return;

        _config.call(this);

        _setSourceConnection.call(this);

        _build.call(this);

        _setReference.call(this);

        _setPoster.call(this);

        _bindEvent.call(this);

        return this;
    };

    Video.prototype.destroy = function () {

        if (this.component === null) return;

        _unbindEvent.call(this);

        this.component.parentNode.removeChild(this.component);

        this.component = null;
    };

    Video.prototype.setSource = function (quality) {

        this.loaded = false;
        this.running = false;

        let newSource = `${this.element.dataset.path}/${quality}/${this.element.dataset.src}`;

        let currentTime = this.element.currentTime;
        this.element.src = newSource;
        this.configurations.time = currentTime;

        this.configurations.quality = quality;
    };

    /* ----------------------------------------------------------- */
    /* == private methods */
    /* ----------------------------------------------------------- */

    function _build() {

        const contentHTML =
            '<div class="video-container">' +
            '<div class="video-button"></div>' +
            '<div class="video-poster"></div>' +
            '<div class="process">' +
            '<div class="process-position">' +
            '<svg class="process-circle" viewBox="25 25 50 50">' +
            '<circle class="process-animation" cx="50" cy="50" r="20" fill="none" stroke-width="2" stroke-miterlimit="10"></circle>' +
            '</svg>' +
            '</div>' +
            '</div>' +
            '<div class="toolbar">' +
            '<div class="video-progress">' +
            '<div class="bar-buffer"></div>' +
            '<div class="bar-progress">' +
            '<div class="pointer-progress"></div>' +
            '</div>' +
            '</div>' +
            '<button class="button-control play"></button>' +
            '<div class="video-volume">' +
            '<button class="button-control volume-high"></button>' +
            '<div class="volume-progress">' +
            '<div class="bar-volume">' +
            '<div class="pointer-volume"></div>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '<div class="video-time">' +
            '<span class="current">00:00</span>' +
            '<span class="separator"></span>' +
            '<span class="duration">00:00</span>' +
            '</div>' +
            '<button class="button-control configuration"></button>' +
            '<button class="button-control fullscreen"></button>' +
            '</div>' +
            '<div class="menu"></div>' +
            '</div>';

        this.component = document.createElement('div');

        this.component.className = 'video';
        this.component.innerHTML = contentHTML;

        this.element.parentNode.insertBefore(this.component, this.element.nextSibling);

        let element = this.component.querySelectorAll('.video-container')[0];

        element.children[0].parentNode.insertBefore(this.element, element.children[0]);
    }

    function _buildConfigurationMenu() {

        let contentHTML =
            '<div class="configuration-item" role="menuitemcheckbox" data-property="loop">' +
            '<div class="configuration-property">Loop</div>' +
            '<div class="configuration-control">' +
            '<div class="switch">' +
            '<input type="checkbox" id="chk_loop"' + (this.configurations.loop ? ' checked' : '') + '/>' +
            '<label for="chk_loop"></label>' +
            '</div>' +
            '</div>' +
            '</div>' +

            '<div class="configuration-item" role="menuitemcheckbox" data-property="undocked">' +
            '<div class="configuration-property">PIP</div>' +
            '<div class="configuration-control">' +
            '<div class="switch">' +
            '<input type="checkbox" id="chk_undocked"' + (!this.options.closed ? ' checked' : '') + '/>' +
            '<label for="chk_undocked"></label>' +
            '</div>' +
            '</div>' +
            '</div>' +

            '<div class="configuration-item" role="menuitem" data-property="speed">' +
            '<div class="configuration-property">Speed</div>' +
            '<div class="configuration-control">' +
            '<span>' + this.element.playbackRate + '</span>' +
            '<i class="mdi mdi-chevron-right"></i>' +
            '</div>' +
            '</div>' +

            '<div class="configuration-item" role="menuitem" data-property="quality">' +
            '<div class="configuration-property">Quality</div>' +
            '<div class="configuration-control">' +
            '<span>' + this.configurations.quality + '</span>' +
            '<i class="mdi mdi-chevron-right"></i>' +
            '</div>' +
            '</div>';

        return contentHTML;
    }

    function _bindEvent() {

        this._events = {
            onplay: _handleOnPlay.bind(this),
            onpause: _handleOnPause.bind(this),
            onended: _handleOnEnded.bind(this),
            onseeked: _handleOnSeeked.bind(this),
            onwaiting: _handleOnWaiting.bind(this),
            onplaying: _handleOnPlaying.bind(this),
            oncanplay: _handleOnCanPlay.bind(this),
            onprogress: _handleOnProgress.bind(this),
            loadeddata: _handleLoadedData.bind(this),
            ontimeupdate: _handleOnTimeUpdate.bind(this),

            elementClick: _handleClick.bind(this),

            volumeMouseEnter: _handleVolumeMouseEnter.bind(this),
            volumeMouseLeave: _handleVolumeMouseLeave.bind(this),

            contentMouseMove: _handleContentMouseMove.bind(this),
            contentMouseEnter: _handleContentMouseEnter.bind(this),
            contentMouseLeave: _handleContentMouseLeave.bind(this),

            progressMouseEnd: _handleProgressEnd.bind(this),
            progressMouseMove: _handleProgressMove.bind(this),
            videoProgressMouseStart: _handleVideoProgressStart.bind(this),
            volumeProgressMouseStart: _handleVolumeProgressStart.bind(this),

            closeClick: _handleCloseClick.bind(this),
            volumeClick: _handleVolumeClick.bind(this),
            controlClick: _handleControlClick.bind(this),
            fullscreenClick: _handleFullscreenClick.bind(this),
            configurationClick: _handleConfigurationClick.bind(this),

            menuClick: _handleMenuClick.bind(this),

            contextMenu: _handleContextMenu.bind(this),
            windowScroll: _handleWindowScroll.bind(this),
            exitFullscreen: _handleExitFullscreen.bind(this)
        };

        this.element.addEventListener('play', this._events.onplay);
        this.element.addEventListener('pause', this._events.onpause);
        this.element.addEventListener('ended', this._events.onended);
        this.element.addEventListener('seeked', this._events.onseeked);
        this.element.addEventListener('waiting', this._events.onwaiting);
        this.element.addEventListener('playing', this._events.onplaying);
        this.element.addEventListener('canplay', this._events.oncanplay);
        this.element.addEventListener('progress', this._events.onprogress);
        this.element.addEventListener('loadeddata', this._events.loadeddata);
        this.element.addEventListener('loadedmetadata', this._events.onloadeddata);
        this.element.addEventListener('timeupdate', this._events.ontimeupdate);
        this.element.addEventListener('dblclick', this._events.fullscreenClick);

        this.element.addEventListener('click', this._events.elementClick);
        this.element.addEventListener('contextmenu', this._events.contextMenu);

        this.content.addEventListener('mousemove', this._events.contentMouseMove);
        this.content.addEventListener('mouseenter', this._events.contentMouseEnter);
        this.content.addEventListener('mouseleave', this._events.contentMouseLeave);

        this.poster.addEventListener('click', this._events.elementClick);

        this.control.addEventListener('click', this._events.controlClick);
        this.fullscreen.addEventListener('click', this._events.fullscreenClick);

        this.volume.addEventListener('mouseenter', this._events.volumeMouseEnter);
        this.volume.addEventListener('mouseleave', this._events.volumeMouseLeave);

        this.volume.children[1].addEventListener('mousedown', this._events.volumeProgressMouseStart);

        this.mute.addEventListener('click', this._events.volumeClick);

        if (this.configurations.pip) {

            window.addEventListener('scroll', this._events.windowScroll);

            this.close.addEventListener('click', this._events.closeClick);
        }

        this.menu.addEventListener('click', this._events.menuClick);

        this.configuration.addEventListener('click', this._events.configurationClick);

        this.progress.addEventListener('mousedown', this._events.videoProgressMouseStart);

        window.addEventListener('mouseup', this._events.progressMouseEnd);
        window.addEventListener('mousemove', this._events.progressMouseMove);

        document.addEventListener('fullscreenchange', this._events.exitFullscreen);
        document.addEventListener('webkitfullscreenchange', this._events.exitFullscreen);
        document.addEventListener('mozfullscreenchange', this._events.exitFullscreen);
        document.addEventListener('MSFullscreenChange', this._events.exitFullscreen);
    }

    function _handleContextMenu(event) {

        event.preventDefault();
    }

    function _handleExitFullscreen(event) {

        if (!document.fullscreenElement && !document.webkitIsFullScreen && !document.mozFullScreen && !document.msFullscreenElement) {

            this.options.fullscreen = false;

            this.content.classList.remove('fullscreen');
        }
    }

    function _handleVideoProgressStart(event) {

        let pageX = event.pageX || event.touches[0].pageX;

        let total = pageX - this.progress.children[1].offset().left;

        _calculateProgressDistance.call(this, total);

        this.progress.classList.add('moving');

        this.progress.setAttribute('data-distance', pageX);
    }

    function _handleVolumeProgressStart(event) {

        let pageX = event.pageX || event.touches[0].pageX;

        let total = pageX - this.volume.children[1].children[0].offset().left;

        _calculateVolumeDistance.call(this, total);

        this.volume.setAttribute('data-distance', pageX);

        this.volume.classList.add('moving');
    }

    function _handleProgressMove(event) {

        if (this.progress.classList.contains('moving')) {

            _focusOut.call(this);

            let pageX = event.pageX || event.touches[0].pageX;

            let start = parseInt(this.progress.dataset.distance, 10);

            let total = pageX - start + this.progress.children[1].clientWidth;

            _calculateProgressDistance.call(this, total);

            this.progress.setAttribute('data-distance', pageX);

        } else if (this.volume.classList.contains('moving')) {

            _focusOut.call(this);

            let pageX = event.pageX || event.touches[0].pageX;

            let start = parseInt(this.volume.dataset.distance, 10);

            let total = pageX - start + this.volume.children[1].children[0].clientWidth;

            _calculateVolumeDistance.call(this, total);

            this.volume.setAttribute('data-distance', pageX);
        }
    }

    function _handleProgressEnd(event) {

        if (this.progress.classList.contains('moving')) {

            _focusOut.call(this);

            this.progress.classList.remove('moving');

        } else if (this.volume.classList.contains('moving')) {

            _focusOut.call(this);

            this.volume.classList.remove('moving');
        }
    }

    function _handleMenuClick(event) {

        if (event.target) {

            let configurationItem = event.target.closestClass('configuration-item');

            if (event.target.classList.contains('configuration-header') || event.target.closestClass('configuration-header')) {

                this.menu.innerHTML = _buildConfigurationMenu.call(this);

            } else if (configurationItem !== null && configurationItem.hasAttribute('role')) {

                const role = configurationItem.getAttribute('role');

                if (role === 'menuitem') {

                    if (configurationItem.dataset.value === undefined) {

                        let contentHTML = '';

                        if (configurationItem.dataset.property === 'speed') {

                            contentHTML = this.options.speed.getSubmenu('Speed', this.element.playbackRate, 'speed');

                        } else if (configurationItem.dataset.property === 'quality') {

                            contentHTML = this.options.quality.getSubmenu('Quality', this.configuration.quality, 'quality');
                        }

                        this.menu.innerHTML = contentHTML;

                    } else {

                        if (configurationItem.dataset.property === 'speed') {

                            this.element.playbackRate = parseFloat(configurationItem.dataset.value, 10);

                        } else if (configurationItem.dataset.property === 'quality') {

                            this.setSource(configurationItem.dataset.value);
                        }

                        this.menu.innerHTML = _buildConfigurationMenu.call(this);
                    }

                } else if (role === 'menuitemcheckbox') {

                    let checked = configurationItem.querySelectorAll('input[type=checkbox]')[0].checked;

                    if (configurationItem.dataset.property === 'loop') {

                        this.configurations.loop = checked;

                    } else if (configurationItem.dataset.property === 'undocked') {

                        this.options.closed = !checked;
                    }
                }
            }
        }
    }

    function _handleCloseClick(event) {

        this.element.pause();

        this.options.closed = true;
        this.options.undocked = false;

        this.element.removeAttribute('style');
        this.content.classList.remove('undocked');
    }

    function _handleWindowScroll(event) {

        if (this.options.running && !this.options.closed) {

            if (!this.component.inViewport() && !this.options.undocked) {

                this.options.undocked = true;

                this.component.style.height = this.element.clientHeight + 'px';

                this.content.classList.add('undocked');
                this.menu.classList.remove('active');

            } else if (this.component.inViewport()) {

                this.options.undocked = false;

                this.component.removeAttribute('style');
                this.content.classList.remove('undocked');
            }
        }
    }

    function _handleConfigurationClick(event) {

        this.menu.innerHTML = _buildConfigurationMenu.call(this);

        if (this.menu.classList.contains('active')) {

            this.menu.classList.remove('active');

        } else {

            this.menu.classList.add('active');
        }
    }

    function _handleClick(event) {

        if (this.options.undocked) {

            scrollTo(this.element, 0, 0);

        } else {

            if (!this.menu.classList.contains('active')) {

                this.poster.classList.remove('active');

                _switch.call(this);

            } else {

                this.menu.classList.remove('active');
            }
        }
    }

    function _handleContentMouseMove(event) {

        if (this.options.loaded) {

            if (this.options.fullscreen) {

                let self = this;

                clearTimeout(this.timer);

                this.timer = setTimeout(function () {

                    if (self.options.running) {

                        self.content.classList.add('c-none');
                        self.control.classList.remove('active');
                    }

                }, 2000);

                this.toolbar.classList.add('active');
                this.content.classList.remove('c-none');

            } else {

                clearTimeout(this.timer);

                this.toolbar.classList.add('active');
                this.content.classList.remove('c-none');
            }
        }
    }

    function _handleContentMouseEnter(event) {

        if (this.options.loaded && !this.options.fullscreen && !this.menu.classList.contains('active')) {

            this.toolbar.classList.add('active');
        }
    }

    function _handleContentMouseLeave(event) {

        if (this.options.loaded && !this.options.fullscreen && this.options.running && !this.menu.classList.contains('active')) {

            this.toolbar.classList.remove('active');
        }
    }

    function _handleVolumeMouseEnter(event) {

        this.volume.classList.add('active');
    }

    function _handleVolumeMouseLeave(event) {

        let progress = this.volume;

        if (!progress.classList.contains('moving')) {

            progress.classList.remove('active');
        }
    }

    function _handleControlClick(event) {

        _switch.call(this);
    }

    function _handleVolumeClick(event) {

        let icon = '';

        if (this.element.muted) {

            this.volume.children[1].children[0].style.width = `${this.options.volume * 100}%`;

            this.element.muted = false;

            this.element.volume = this.options.volume;

            icon = 'volume-high';

        } else {

            this.options.volume = this.element.volume;

            this.volume.children[1].children[0].style.width = '0px';

            this.element.muted = true;

            this.element.volume = 0;

            icon = 'volume-off';
        }

        event.target.classList.remove('volume-off');
        event.target.classList.remove('volume-low');
        event.target.classList.remove('volume-medium');
        event.target.classList.remove('volume-high');

        event.target.classList.add(icon);
    }

    function _handleOnPlay(event) {

        this.options.running = true;

        this.poster.classList.remove('active');

        this.control.classList.add('pause');
        this.control.classList.remove('play');
        this.control.classList.remove('replay');
    }

    function _handleOnPause(event) {

        this.options.running = false;

        this.control.classList.add('play');
        this.control.classList.remove('pause');
        this.control.classList.remove('replay');
    }

    function _handleOnEnded(event) {

        if (!this.configurations.loop) {

            this.options.running = false;

            this.control.classList.add('replay');
            this.control.classList.remove('play');
            this.control.classList.remove('pause');

            if (this.configurations.poster) {

                this.poster.classList.add('active');
                this.toolbar.classList.remove('active');

            } else {

                this.toolbar.classList.add('active');
            }

        } else {

            _switch.call(this, true);
        }
    }

    function _handleOnSeeked(event) {

        this.options.loaded = true;
        this.options.running = false;

        _updateProgress.call(this);
    }

    function _handleOnCanPlay(event) {

        if (this.configurations.autoplay && !this.options.running) {

            _switch.call(this, true);
        }
    }

    function _handleOnProgress(event) {

        if (this.options.loaded) {

            _updateProgress.call(this);
        }
    }

    function _handleOnWaiting(event) {

        this.options.running = false;
        this.process.classList.add('active');
    }

    function _handleOnPlaying(event) {

        this.options.running = true;
        this.process.classList.remove('active');
    }

    function _handleLoadedData(event) {

        let time = this.time.children;

        this.element.currentTime = this.configurations.time;

        time[2].textContent = this.element.duration.toTimeString();

        time[0].textContent = this.element.currentTime.toTimeString();
    }

    function _handleOnTimeUpdate(event) {

        if (this.options.loaded) {

            this.progress.children[1].style.width = `${(100 / this.element.duration * this.element.currentTime)}% `;

            this.time.children[0].textContent = this.element.currentTime.toTimeString();
        }
    }

    function _handleFullscreenClick(event) {

        _toggleFullscreen.call(this);
    }

    function _unbindEvent() {

        this.component.removeEventListener('click', this._events.methodFunction);
    }

    function _config() {

        let self = this;

        if (this.element.dataset.quality !== undefined) {

            this.options.quality.push.apply(this.options.quality, this.element.dataset.quality.split(','));
        }

        this.configurations = {

            poster: null,
            pip: self.element.dataset.pip !== undefined,
            loop: self.element.dataset.loop !== undefined,
            autoplay: self.element.dataset.autoplay !== undefined,
            quality: self.options.quality[self.options.quality.length - 1],
            src: self.element.dataset.src !== undefined ? self.element.src : null,
            time: self.element.dataset.time !== undefined ? self.element.dataset.time : 0,
            next: self.element.dataset.next !== undefined ? self.element.dataset.next : null,
            volume: self.element.dataset.volume !== undefined ? self.element.dataset.volume : 1
        };
    }

    function _setSourceConnection() {

        if (this.options.connection.downlink < 1) {

            this.setSource(this.options.quality[1]);

        } else if (this.options.connection.downlink < 2) {

            this.setSource(this.options.quality[2]);

        } else if (this.options.connection.downlink < 5) {

            this.setSource(this.options.quality[3]);

        } else {

            this.setSource(this.options.quality[3]);
        }
    }

    function _setReference() {

        this.content = this.component.children[0];

        let elements = this.content.children;

        //this.video = elements[0];
        this.close = elements[1];
        this.poster = elements[2];
        this.process = elements[3];
        this.toolbar = elements[4];
        this.menu = elements[5];

        elements = this.toolbar.children;

        this.progress = elements[0];
        this.control = elements[1];
        this.volume = elements[2];
        this.time = elements[3];
        this.current = this.time.children[0];
        this.mute = this.volume.children[0];
        this.configuration = elements[4];
        this.fullscreen = elements[5];
    }

    function _setPoster() {

        if (this.configurations.poster === null) {

            this.poster.style.backgroundSize = 'cover';
            this.poster.style.backgroundRepeat = 'no-repeat';

            this.configurations.poster = `${ this.element.dataset.path } cover.jpg`;

            this.poster.style.backgroundImage = `url('${this.configurations.poster}')`;
        }

        if (!this.running) {

            this.poster.classList.add('active');
        }
    }

    function _toggleFullscreen() {

        if (!document.fullscreenElement && !document.mozFullScreenElement && !document.webkitFullscreenElement && !document.msFullscreenElement) {

            if (this.component.requestFullscreen) {

                this.component.requestFullscreen();

            } else if (this.element.msRequestFullscreen) {

                this.component.msRequestFullscreen();

            } else if (this.element.mozRequestFullScreen) {

                this.component.mozRequestFullScreen();

            } else if (this.element.webkitRequestFullscreen) {

                this.component.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
            }

            this.options.fullscreen = true;

            this.content.classList.add('fullscreen');

        } else {

            if (document.exitFullscreen) {

                document.exitFullscreen();

            } else if (document.msExitFullscreen) {

                document.msExitFullscreen();

            } else if (document.mozCancelFullScreen) {

                document.mozCancelFullScreen();

            } else if (document.webkitExitFullscreen) {

                document.webkitExitFullscreen();
            }

            this.options.fullscreen = false;

            this.content.classList.remove('fullscreen');
        }
    }

    function _updateProgress() {

        let duration = this.element.duration;

        let buffered = this.element.buffered;

        if (buffered) {

            let end = buffered.end(0);

            let percentage = end / duration * 100;

            this.progress.children[0].style.width = percentage + '%';
        }
    }

    function _switch(force) {

        if (this.options.loaded) {

            if (this.element.paused && !this.running || force) {

                this.element.play();

            } else {

                this.element.pause();
            }
        }
    }

    function _focusOut() {

        if (document.selection) {

            document.selection.empty();

        } else {

            window.getSelection().removeAllRanges();
        }
    }

    function _calculateProgressDistance(distance) {

        let width = this.progress.clientWidth;

        distance = Math.max(distance >= width ? width : distance, 0);

        let percentage = Math.max(distance / width, 0);

        this.progress.children[1].style.width = `${ percentage * 100 }% `;

        let seconds = percentage * this.element.duration;

        this.element.currentTime = seconds;

        this.current.textContent = seconds.toTimeString();
    }

    function _calculateVolumeDistance(distance) {

        let width = this.volume.children[1].clientWidth;

        distance = Math.max(distance >= width ? width : distance, 0);

        let percentage = Math.max(distance / width, 0);

        this.volume.children[1].children[0].style.width = `${ percentage * 100 }% `;

        this.element.volume = percentage;

        //this.$botaoVolume.removeClass('volume-off volume-low volume-medium volume-high').addClass(porcentagem === 0 ? 'volume-off' : porcentagem < 0.2 ? 'volume-low' : porcentagem < 0.6 ? 'volume-medium' : 'volume-high');
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

    Element.prototype.closestClass = function (cls) {
        let element = this;
        while ((element = element.parentElement) && !element.classList.contains(cls));
        return element;
    };

    Element.prototype.inViewport = function () {

        let dimension = { top: window.pageYOffset, left: window.pageXOffset };

        dimension.right = dimension.left + window.innerWidth;
        dimension.bottom = dimension.top + window.innerHeight;

        let position = this.offset();

        position.right = position.left + this.clientWidth;
        position.bottom = position.top + this.clientHeight;

        return !(dimension.right < position.left || dimension.left > position.right || dimension.bottom < position.top || dimension.top > position.bottom);
    };

    Element.prototype.offset = function () {

        let rect = this.getBoundingClientRect(),
            scrollLeft = window.pageXOffset || document.documentElement.scrollLeft,
            scrollTop = window.pageYOffset || document.documentElement.scrollTop;

        return { top: rect.top + scrollTop, left: rect.left + scrollLeft };
    };

    Number.prototype.toTimeString = function () {

        let date = new Date(null);

        date.setSeconds(this);

        if (this >= 86400) {

            return date.toISOString().substr(11, 8);
        }

        return date.toISOString().substr(14, 5);
    };

    Array.prototype.getSubmenu = function (title, selected, property) {

        let contentHTML = ['<div class="configuration-header"><div class="configuration-label">' + title + '</div></div><div class="configuration-content">'];

        [].forEach.call(this, function (item) {

            contentHTML.push(
                '<div class="configuration-item' + (item === selected ? ' selected' : '') + '" role="menuitem" data-property="' + property + '" data-value="' + item + '"> ' +
                '<div class="configuration-control">' +
                '<span>' + item + '</span>' +
                '</div>' +
                '</div>');

        });

        contentHTML.push('</div></div>');

        return contentHTML.join('');
    };

    /* ----------------------------------------------------------- */
    /* == return */
    /* ----------------------------------------------------------- */

    return {
        init: Video
    };

}));
