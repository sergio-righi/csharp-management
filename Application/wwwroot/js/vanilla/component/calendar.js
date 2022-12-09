
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.calendar = factory();
    }

}(this, function () {

    function Get(element) {

        return dataStorage.get(element, 'calendar');
    }

    /* ----------------------------------------------------------- */
    /* == component */
    /* ----------------------------------------------------------- */

    function Calendar(element, options) {

        this.dates = [];
        this.exception = [];

        this.lastWeekday = 6;
        this.firstWeekday = 0;

        this.today = new Date();
        this.selectedElement = null;

        this.month = this.today.getMonth();
        this.year = this.today.getFullYear();

        var defaults = {
            min: null,
            max: null,
            today: true,
            categories: [
                { category: -1, cls: 'black' }
            ],
            weekend: true,
            selected: null,
            overflow: true,
            weekday: 'short',
            description: true,
            locale: window.navigator.language
        };

        this.calendar = element;

        this.options = extend({}, defaults, options);

        this.init();
    }

    Calendar.prototype.init = function () {

        if (this.calendar === undefined) return;

        _config.call(this);

        _build.call(this);

        _bindEvent.call(this);

        _buildMonth.call(this);

        dataStorage.put(this.calendar, 'calendar', this);

        //if (typeof this.options.onSomething === 'function') {

        //    this.options.onSomething.call(this);
        //}

        return this;
    };

    Calendar.prototype.destroy = function () {

        if (this.calendar === null) return;

        _unbindEvent.call(this);

        this.calendar.parentNode.removeChild(this.calendar);

        this.calendar = null;
    };

    Calendar.prototype.add = function (event) {

        if (typeof event.date !== 'undefined' && !this.exception.isException(event.date)) {

            const element = this.dates.getEvent(event.id);

            if (element.index === -1 || element.event !== null && (element.event.category === -1 || element.event.category !== event.category)) {

                let date = new Date(event.date);

                let calendarEvent = new CalendarEvent(event);

                calendarEvent.startDate = event.date;

                calendarEvent.cls = this.options.categories.getCategoryClass(calendarEvent.category);

                do {

                    if (date.inRange(this.options.min, this.options.max)) {

                        calendarEvent = new CalendarEvent(calendarEvent);

                        calendarEvent.date = new Date(date);

                        this.dates.push(calendarEvent);
                    }

                    date.setDate(date.getDate() + 1);

                } while (date <= event.endDate);
            }
        }

        return this;
    };

    Calendar.prototype.addException = function (date) {

        if (typeof date !== 'undefined' && !this.exception.isException(date)) {

            if (date.inRange(this.options.min, this.options.max)) {

                this.exception.push(date);

                this.update();
            }
        }

        return this;
    };

    Calendar.prototype.remove = function (id) {

        if (typeof id !== 'undefined') {

            let index = this.dates.getEvent(id).index;

            if (index !== -1) {

                this.dates.splice(index, 1);

                this.update();
            }
        }

        return this;
    };

    Calendar.prototype.update = function () {

        _buildMonth.call(this);

        return this;
    };

    Calendar.prototype.moveTo = function (last, first, direction, day) {

        this.month !== last ? this.month += direction : (this.month = first, this.year += direction);

        const date = new Date(this.year, this.month, day);

        //this.$elemento.trigger('calendar.change', [this]);

        this.update();

        return this;
    };

    Calendar.prototype.open = function (date) {

        this.options.selected = date;

        if (this.options.description) {

            let events = this.dates.getEventByDate(date);

            if (events.length > 0) {

                let interval;

                let contentHTML = ['<ul class="list list-disorderly">'];

                for (let i = 0; i < events.length; i++) {

                    if (events[i].description !== null) {

                        if (events[i].endDate === null || events[i].startDate.compareTo(events[i].endDate)) {

                            contentHTML.push(`<li class="${events[i].cls}">${events[i].description}</li>`);

                        } else {

                            interval = `(${eventos[i].startDate.toLocaleDateString(this.options.locale, { day: 'numeric', month: 'numeric', year: 'numeric' })} - ${events[i].endDate.toLocaleDateString(this.options.locale, { day: 'numeric', month: 'numeric', year: 'numeric' })})`;

                            contentHTML.push(`<li class="${events[i].cls}">${events[i].description}<br/><small>${interval}</small></li>`);
                        }
                    }
                }

                contentHTML.push('</ul>');

                this.calendarFooterHeader.innerHTML = date.toLocaleDateString(this.options.locale, { day: 'numeric', month: 'long', year: 'numeric' });

                this.calendarFooterContent.innerHTML = contentHTML.join('');

                this.calendarFooter.classList.add('active');

            } else {

                events = null;
            }

            //this.$elemento.trigger('calendar.click', [date, events]);
        }

        this.update();

        return this;
    };

    /* ----------------------------------------------------------- */
    /* == private methods */
    /* ----------------------------------------------------------- */

    function _build() {

        let calendarHeader = document.createElement('div');
        calendarHeader.className = 'calendar-header';

        this.arrowLeft = document.createElement('span');
        this.arrowLeft.className = 'header-left';

        let calendarLabel = document.createElement('div');
        calendarLabel.className = 'header-label';

        this.calendarMonth = document.createElement('span');
        this.calendarMonth.className = 'header-month';

        this.calendarYear = document.createElement('span');
        this.calendarYear.className = 'header-year';

        this.arrowRight = document.createElement('span');
        this.arrowRight.className = 'header-right';

        calendarLabel.appendChild(this.calendarMonth);
        calendarLabel.appendChild(this.calendarYear);

        calendarHeader.appendChild(this.arrowLeft);
        calendarHeader.appendChild(calendarLabel);
        calendarHeader.appendChild(this.arrowRight);

        let calendarContent = document.createElement('div');
        calendarContent.className = 'calendar-content';

        calendarContent.innerHTML = _buildWeek.call(this);

        this.calendarDate = document.createElement('div');
        this.calendarDate.className = 'calendar-month';

        calendarContent.appendChild(this.calendarDate);

        this.calendar.appendChild(calendarHeader);
        this.calendar.appendChild(calendarContent);

        if (this.options.description) {

            this.calendarFooter = document.createElement('div');
            this.calendarFooter.className = 'calendar-footer';

            this.calendarFooterClose = document.createElement('div');
            this.calendarFooterClose.className = 'calendar-footer-icon';

            this.calendarFooterHeader = document.createElement('div');
            this.calendarFooterHeader.className = 'calendar-footer-header';

            this.calendarFooterContent = document.createElement('div');
            this.calendarFooterContent.className = 'calendar-footer-content';

            this.calendarFooter.appendChild(this.calendarFooterClose);
            this.calendarFooter.appendChild(this.calendarFooterHeader);
            this.calendarFooter.appendChild(this.calendarFooterContent);

            this.calendar.appendChild(this.calendarFooter);
        }
    }

    function _buildWeek() {

        let date = _getFirstDay.call(this);

        let contentHTML = ['<div class="calendar-weekday">'];

        for (let i = this.firstWeekday; i <= this.lastWeekday; i++) {

            contentHTML.push(`<div class="item">${date.toLocaleString(this.options.locale, { weekday: this.options.weekday }).toUpperCase()}</div>`);

            date.setDate(date.getDate() + 1);
        }

        contentHTML.push('</div>');

        return contentHTML.join('');
    }

    function _buildEventDate(date, events) {

        let element = document.createElement('div');

        element.classList = 'item';
        element.textContent = date.getUTCDate();

        const disabled = date.getMonth() !== this.month || this.exception.isException(date) ? true : !date.inRange(this.options.min, this.options.max);

        if (disabled) {

            element.classList.add('disabled');

        } else {

            if (this.options.today && date.compareTo(this.today)) {

                element.classList.add('today');

                this.selectedElement = element;
            }

            if (this.options.selected !== null && date.compareTo(this.options.selected)) {

                element.classList.add('selected');

                this.selectedElement = element;
            }

            let event = events.getEventByDate(date);

            if (event.length > 0) {

                element.setAttribute('data-quantity', event.length);

                element.classList.add('highlight');
            }

            const categories = event.getCategory();

            if (categories.length > 0) {

                let contentHTML = ['<div class="group">'];

                [].forEach.call(categories, function (category, i) {

                    contentHTML.push(`<div class="category bg-${category}"></div>`);

                });

                contentHTML.push('</div>');

                element.innerHTML += contentHTML.join('');
            }
        }

        return element.outerHTML;
    }

    function _buildMonth() {

        let contentHTML = [];

        const dates = _getMonthDays.call(this);

        const events = this.dates.getEventByPeriod(this.month, this.year);

        const outOfIntervalMin = this.options.min !== null && dates[0] <= this.options.min ? true : false;

        const outOfIntervalMax = this.options.max !== null && dates[dates.length - 1] >= this.options.max ? true : false;

        for (let i = 0; i < dates.length; i++) {

            const date = dates[i];

            const weekday = dates[i].getDay();

            if (!this.options.overflow && i === 0 && weekday !== this.firstWeekday) {

                contentHTML.push('<div class="calendar-week">');

                for (let j = this.firstWeekday; j < weekday; j++) {

                    contentHTML.push('<div class="item disabled"></div>');
                }
            }

            if (i === 0 && outOfIntervalMin) {

                this.arrowLeft.classList.add('disabled');

            } else if (!outOfIntervalMin) {

                this.arrowLeft.classList.remove('disabled');
            }

            if (weekday === this.firstWeekday) contentHTML.push('<div class="calendar-week">');

            contentHTML.push(_buildEventDate.call(this, date, events));

            if (weekday === this.lastWeekday) contentHTML.push('</div>');
        }

        if (!this.options.overflow && date.getDay() !== this.lastWeekday) {

            for (let k = date.getDay(); k < this.lastWeekday; k++) {

                contentHTML.push('<div class="item disabled"></div>');
            }

            contentHTML.push('</div>');
        }


        if (outOfIntervalMax) {

            this.arrowRight.classList.add('disabled');

        } else {

            this.arrowRight.classList.remove('disabled');
        }


        this.calendarMonth.innerHTML = new Date(this.year, this.month, 1).toLocaleString(this.options.locale, { month: 'long' });

        this.calendarYear.innerHTML = this.year;

        this.calendarDate.innerHTML = contentHTML.join('');

        return this;
    }

    function _bindEvent() {

        this._events = {
            dateClick: _handleDateClick.bind(this),
            arrowClick: _handleArrowClick.bind(this),
            closeClick: _handleCloseClick.bind(this)
        };

        this.arrowLeft.addEventListener('click', this._events.arrowClick.bind(this, 'left'));
        this.arrowRight.addEventListener('click', this._events.arrowClick.bind(this, 'right'));

        this.calendar.addEventListener('click', this._events.dateClick);

        if (this.options.description) {

            this.calendarFooterClose.addEventListener('click', this._events.closeClick);
        }
    }

    function _unbindEvent() {

        this.arrowLeft.removeEventListener('click', this._events.arrowClick.bind(this, 'left'));
        this.arrowRight.removeEventListener('click', this._events.arrowClick.bind(this, 'right'));

        this.calendar.removeEventListener('click', this._events.dateClick);

        if (this.options.description) {

            this.calendarFooterClose.removeEventListener('click', this._events.closeClick);
        }
    }

    function _handleDateClick(event) {

        if (event.target && event.target.classList.contains('item')) {

            if (!event.target.classList.contains('out') && !event.target.classList.contains('disabled')) {

                const day = parseInt(event.target.textContent);

                const date = new Date(this.year, this.month, day);

                this.selectedElement.classList.remove('selected');

                event.target.classList.add('selected');

                this.open(date);

                this.selectedElement = event.target;
            }
        }
    }

    function _handleArrowClick(direction, event) {

        if (event.target && !/disabled/.test(event.target.className)) {

            if (direction === 'left') {

                this.moveTo(0, 11, -1, -1);

            } else if (direction === 'right') {

                this.moveTo(11, 0, 1, -1);
            }
        }
    }

    function _handleCloseClick(event) {

        this.calendarFooter.classList.remove('active');
    }

    function _config() {

        this.options.min !== null ? !this.options.min.compareTo(new Date()) ? this.options.min.setMonth(this.options.min.getMonth() - 1) : null : null;

        this.options.max !== null ? !this.options.max.compareTo(new Date()) ? this.options.max.setMonth(this.options.max.getMonth() - 1) : null : null;

        this.options.max !== null && this.options.max.getMonth() < this.month && this.options.max.getFullYear() === this.year ? this.month = this.options.max.getMonth() : null;

        if (!this.options.weekend) {

            this.lastWeekday = 5;

            this.firstWeekday = 1;
        }
    }

    function _getMonthDays() {

        let days = [];

        let first = new Date(this.year, this.month, 1);

        let last = new Date(this.year, this.month + 1, 0);

        if (this.options.overflow) {

            while (last.getDay() !== this.lastWeekday) last.setDate(last.getDate() + 1);

            while (first.getDay() !== this.firstWeekday) first.setDate(first.getDate() - 1);
        }

        while (first < last || first.compareTo(last)) {

            let weekday = first.getDay();

            if (weekday >= this.firstWeekday && weekday <= this.lastWeekday) {

                days.push(new Date(first));
            }

            first.setDate(first.getDate() + 1);
        }

        return days;
    }

    function _getFirstDay() {

        let date = new Date();

        if (date.getDay() !== this.firstWeekday) {

            while (date.getDay() !== this.firstWeekday) {

                date.setDate(date.getDate() - 1);
            }
        }

        return date;
    }

    /* ----------------------------------------------------------- */
    /* == objects */
    /* ----------------------------------------------------------- */

    let CalendarEvent = function (event) {

        this.id = event.id;

        this.startDate = null;

        this.date = event.date;

        this.cls = typeof event.cls === 'undefined' ? null : event.cls;

        this.endDate = typeof event.endDate === 'undefined' ? null : event.endDate;

        this.category = typeof event.category === 'undefined' ? -1 : event.category;

        this.description = typeof event.description === 'undefined' ? null : event.description;
    };

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

    Date.prototype.compareTo = function (date) {

        if (date === null) return false;

        return this.getDate() === date.getDate() && this.getMonth() === date.getMonth() && this.getFullYear() === date.getFullYear();
    };

    Date.prototype.inRange = function (start, end) {

        return (start === null || this >= start) && (end === null || this <= end);
    };

    Array.prototype.isException = function (date) {

        let exception = false;

        [].forEach.call(this, function (item, i) {

            if (item.compareTo(date)) {

                exception = true;

                return false;
            }

        });

        return exception;
    };

    Array.prototype.getEvent = function (id) {

        let event = { index: -1, item: null };

        [].forEach.call(this, function (item, i) {

            if (item.id === id) {

                event.index = i;
                event.item = item;

                return false;
            }

        });

        return event;
    };

    Array.prototype.getEventByPeriod = function (month, year) {

        return Array.prototype.filter.call(this, function (item, i) {

            if (item.date.getMonth() === month && item.date.getFullYear() === year) {

                return item;
            }

        });
    };

    Array.prototype.getEventByDate = function (date) {

        return Array.prototype.filter.call(this, function (item, i) {

            if (date.compareTo(item.date)) {

                return item;
            }

        });
    };

    Array.prototype.getCategoryClass = function (category) {

        let cls = '';

        if (this.length > 0) {

            [].forEach.call(this, function (item, i) {

                if (item.category === category) {

                    cls = item.cls;

                    return false;
                }

            });
        }

        return cls;
    };

    Array.prototype.getCategory = function () {

        let categoryClass = [];

        [].forEach.call(this, function (item, i) {

            if (categoryClass.indexOf(item.cls) === -1) {

                categoryClass.push(item.cls);
            }

        });

        return categoryClass;
    };

    /* ----------------------------------------------------------- */
    /* == return */
    /* ----------------------------------------------------------- */

    return {
        init: Calendar,
        get: Get
    };

}));
