
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.datetimepicker = factory();
    }

}(this, function () {

    const Component = {
        DATE_TIME: 'datetime',
        DATE: 'date',
        TIME: 'time'
    };

    const Key = {
        enter: 13,
        backspace: 8,
        refresh: 116
    };

    function Get(element) {

        return dataStorage.get(element, 'datetimepicker');
    }

    /* ----------------------------------------------------------- */
    /* == component */
    /* ----------------------------------------------------------- */

    function Datetimepicker(element, options) {

        this.hour = null;
        this.minute = null;

        this.lastWeekday = 6;
        this.firstWeekday = 0;

        this.counter = 0;
        this.timer = null;
        this.mobile = false;

        this.today = new Date();
        this.selected = this.today;

        this.day = this.today.getDate();
        this.month = this.today.getMonth();
        this.year = this.today.getFullYear();

        var defaults = {
            minHour: 0,
            clock: null,
            maxHour: 23,
            minMinute: 0,
            maxMinute: 59,
            minDate: null,
            maxDate: null,
            weekend: true,
            calendar: null,
            overflow: true,
            weekday: 'short',
            locale: window.navigator.language
        };

        this.element = element;

        this.options = extend({}, defaults, options);

        this.init();
    }

    Datetimepicker.prototype.init = function () {

        if (this.element === null) return;

        _config.call(this);

        _bindEvent.call(this);

        dataStorage.put(this.element, 'datetimepicker', this);

        //if (typeof this.options.onSomething === 'function') {

        //    this.options.onSomething.call(this);
        //}

        return this;
    };

    Datetimepicker.prototype.destroy = function () {

        if (this.datetime === undefined && this.overlay === undefined) return;

        _unbindElementEvent.call(this);

        if (this.options.popup || this.mobile) {

            if (this.overlay !== undefined) {

                this.overlay.parentNode.removeChild(this.overlay);
                this.overlay = null;
            }

        } else {

            if (this.datetime !== null) {

                this.datetime.parentNode.removeChild(this.datetime);
                this.datetime = null;
            }
        }
    };

    Datetimepicker.prototype.setDate = function (date) {

        if (date !== null && date !== undefined) {

            this.selected = date;

            this.day = date.getDate();

            this.month = date.getMonth();

            this.year = date.getFullYear();

            _buildMonth.call(this);
        }
    };

    Datetimepicker.prototype.setMinDate = function (date) {
        
        if (date !== null && date !== undefined) {

            this.options.minDate = date;
        }
    };

    Datetimepicker.prototype.setMaxDate = function (date) {

        if (date !== null && date !== undefined) {

            this.options.maxDate = date;
        }
    };

    Datetimepicker.prototype.setTime = function (hour, minute) {

        this.setHour(hour);
        this.setMinute(minute);
    };

    Datetimepicker.prototype.setHour = function (hour) {

        if (hour !== null && hour !== undefined) {

            this.hour = hour === null ? this.options.minHour.format() : ('0' + hour).slice(-2);

            this.clockHeaderHour.textContent = this.hour;

            this.clockHour.value = this.hour;
        }
    };

    Datetimepicker.prototype.setMinute = function (minute) {

        if (minute !== null && minute !== undefined) {

            this.minute = minute === null ? this.options.minMinute.format() : ('0' + minute).slice(-2);

            this.clockHeaderMinute.textContent = this.minute;

            this.clockMinute.value = this.minute;
        }
    };

    Datetimepicker.prototype.open = function (value) {

        if (this.component === Component.DATE_TIME) {

            if (value.length > 0) {

                let indexOf = value.indexOf(' ');

                if (indexOf !== -1) {

                    let date = value.substring(0, indexOf).toDate();

                    _setSelectedDate.call(this, date);

                    let time = value.substring(indexOf, value.length).toTime();

                    this.setTime(time[0], time[1]);
                }

            } else {

                _isValidDate.call(this, this.today);

                this.setTime(0, 0);
            }

        } else if (this.component === Component.DATE) {

            if (value.length > 0) {

                let indexOf = value.indexOf(' ');

                let date = indexOf === -1 ? value.toDate() : value.split(' ')[0].toDate();

                _setSelectedDate.call(this, date);

            } else {

                _isValidDate.call(this, this.today);
            }

        } else if (this.component === Component.TIME) {

            if (value.length > 0) {

                let indexOf = value.indexOf(' ');

                let time = indexOf === -1 ? value.toTime() : value.split(' ')[1].toTime();

                this.setTime(time[0], time[1]);

            } else {

                this.setTime(0, 0);
            }
        }

        if (this.component === Component.DATE_TIME || this.component === Component.DATE) {

            this.calendarHeader.classList.add('active');
            this.calendarControl.classList.add('active');
            this.calendarContent.classList.add('active');
        }

        if (this.component === Component.TIME) {

            this.clockHeader.classList.add('active');
            this.clockControl.classList.add('active');
            this.clockContent.classList.add('active');
        }

        if (this.options.popup || this.mobile) {

            this.overlay.classList.add('active');
        }

        return this;
    };

    Datetimepicker.prototype.close = function () {

        let time = this.hour + ':' + this.minute;

        let date = this.selected.getDate().format() + '/' + (this.selected.getMonth() + 1).format() + '/' + this.selected.getFullYear();

        if (this.component === Component.DATE_TIME) {

            this.element.value = date + ' ' + time;

        } else if (this.component === Component.DATE) {

            this.element.value = date;

        } else if (this.component === Component.TIME) {

            this.element.value = time;
        }

        this.element.dispatchEvent(new CustomEvent('datetimepicker.close', { detail: this }));

        this.destroy();

        return this;
    };

    Datetimepicker.prototype.switch = function () {

        this.clockControl.classList.toggle('active');
        this.calendarControl.classList.toggle('active');

        this.calendarHeader.classList.toggle('active');
        this.calendarContent.classList.toggle('active');

        this.clockHeader.classList.toggle('active');
        this.clockContent.classList.toggle('active');
    };

    /* ----------------------------------------------------------- */
    /* == private methods */
    /* ----------------------------------------------------------- */

    function _build() {

        this.datetime = document.createElement('div');
        this.datetime.className = 'datetime';

        let datetimeHeader = document.createElement('div');
        datetimeHeader.className = 'datetime-header';

        let datetimeContent = document.createElement('div');
        datetimeContent.className = 'datetime-content';

        let datetimeFooter = document.createElement('div');
        datetimeFooter.className = 'datetime-footer';

        if (this.component === Component.DATE_TIME || this.component === Component.DATE) {

            this.calendarHeader = document.createElement('div');
            this.calendarHeader.className = 'calendar-header';

            let calendarHeaderDayContainer = document.createElement('div');
            calendarHeaderDayContainer.className = 'calendar-day';

            this.calendarHeaderDay = document.createElement('div');
            this.calendarHeaderDay.className = 'day';

            this.calendarHeaderWeekday = document.createElement('div');
            this.calendarHeaderWeekday.className = 'weekday';

            calendarHeaderDayContainer.appendChild(this.calendarHeaderDay);
            calendarHeaderDayContainer.appendChild(this.calendarHeaderWeekday);

            let calendarHeaderMonthContainer = document.createElement('div');
            calendarHeaderMonthContainer.className = 'calendar-month';

            this.calendarHeaderMonth = document.createElement('div');
            this.calendarHeaderMonth.className = 'month';

            this.calendarMonthArrowUp = document.createElement('div');
            this.calendarMonthArrowUp.className = 'arrow up';

            this.calendarMonthArrowDown = document.createElement('div');
            this.calendarMonthArrowDown.className = 'arrow down';

            calendarHeaderMonthContainer.appendChild(this.calendarMonthArrowUp);
            calendarHeaderMonthContainer.appendChild(this.calendarHeaderMonth);
            calendarHeaderMonthContainer.appendChild(this.calendarMonthArrowDown);

            let calendarHeaderYearContainer = document.createElement('div');
            calendarHeaderYearContainer.className = 'calendar-year';

            this.calendarHeaderYear = document.createElement('div');
            this.calendarHeaderYear.className = 'year';

            this.calendarYearArrowUp = document.createElement('div');
            this.calendarYearArrowUp.className = 'arrow up';

            this.calendarYearArrowDown = document.createElement('div');
            this.calendarYearArrowDown.className = 'arrow down';

            calendarHeaderYearContainer.appendChild(this.calendarYearArrowUp);
            calendarHeaderYearContainer.appendChild(this.calendarHeaderYear);
            calendarHeaderYearContainer.appendChild(this.calendarYearArrowDown);

            this.calendarHeader.appendChild(calendarHeaderDayContainer);
            this.calendarHeader.appendChild(calendarHeaderMonthContainer);
            this.calendarHeader.appendChild(calendarHeaderYearContainer);

            datetimeHeader.appendChild(this.calendarHeader);

            this.calendarContent = document.createElement('div');
            this.calendarContent.className = 'calendar-content';
            this.calendarContent.innerHTML = _buildCalendarWeek.call(this);

            this.calendarDate = document.createElement('div');
            this.calendarDate.className = 'calendar-month';

            this.calendarContent.appendChild(this.calendarDate);

            datetimeContent.appendChild(this.calendarContent);

            this.calendarControl = document.createElement('div');
            this.calendarControl.className = 'control-calendar';

            this.calendarTodayButton = document.createElement('div');
            this.calendarTodayButton.className = 'control-item today';

            this.calendarConfirmButton = document.createElement('div');
            this.calendarConfirmButton.className = 'control-item ok';

            this.calendarControl.appendChild(this.calendarTodayButton);
            this.calendarControl.appendChild(this.calendarConfirmButton);
        }

        if (this.component === Component.DATE_TIME || this.component === Component.TIME) {

            this.clockHeader = document.createElement('div');
            this.clockHeader.className = 'clock-header';

            this.clockHeaderHour = document.createElement('div');
            this.clockHeaderHour.className = 'clock-hour';
            this.clockHeaderHour.textContent = '00';

            this.clockHeaderMinute = document.createElement('div');
            this.clockHeaderMinute.className = 'clock-minute';
            this.clockHeaderMinute.textContent = '00';

            this.clockHeader.appendChild(this.clockHeaderHour);
            this.clockHeader.appendChild(this.clockHeaderMinute);

            datetimeHeader.appendChild(this.clockHeader);

            this.clockContent = document.createElement('div');
            this.clockContent.className = 'clock-content';

            let clockContentHourContainer = document.createElement('div');
            clockContentHourContainer.className = 'clock-hour';

            this.clockHour = document.createElement('input');
            this.clockHour.setAttribute('type', 'text');
            this.clockHour.setAttribute('min', this.options.minHour);
            this.clockHour.setAttribute('max', this.options.maxHour);
            this.clockHour.setAttribute('step', 1);
            this.clockHour.className = 'hour';
            this.clockHour.value = 0;

            this.clockHourArrowUp = document.createElement('div');
            this.clockHourArrowUp.className = 'arrow up';

            this.clockHourArrowDown = document.createElement('div');
            this.clockHourArrowDown.className = 'arrow down';

            clockContentHourContainer.appendChild(this.clockHourArrowUp);
            clockContentHourContainer.appendChild(this.clockHour);
            clockContentHourContainer.appendChild(this.clockHourArrowDown);

            let clockContentMinuteContainer = document.createElement('div');
            clockContentMinuteContainer.className = 'clock-minute';

            this.clockMinute = document.createElement('input');
            this.clockMinute.setAttribute('type', 'text');
            this.clockMinute.setAttribute('min', this.options.minMinute);
            this.clockMinute.setAttribute('max', this.options.maxMinute);
            this.clockMinute.setAttribute('step', 1);
            this.clockMinute.className = 'minute';
            this.clockMinute.value = 0;

            this.clockMinuteArrowUp = document.createElement('div');
            this.clockMinuteArrowUp.className = 'arrow up';

            this.clockMinuteArrowDown = document.createElement('div');
            this.clockMinuteArrowDown.className = 'arrow down';

            clockContentMinuteContainer.appendChild(this.clockMinuteArrowUp);
            clockContentMinuteContainer.appendChild(this.clockMinute);
            clockContentMinuteContainer.appendChild(this.clockMinuteArrowDown);

            this.clockContent.appendChild(clockContentHourContainer);
            this.clockContent.appendChild(clockContentMinuteContainer);

            datetimeContent.appendChild(this.clockContent);

            this.clockControl = document.createElement('div');
            this.clockControl.className = 'control-clock';

            this.clockConfirmButton = document.createElement('div');
            this.clockConfirmButton.className = 'control-item ok';

            this.clockControl.appendChild(this.clockConfirmButton);
        }

        if (this.component === Component.DATE_TIME) {

            this.clockSwitchButton = document.createElement('div');
            this.clockSwitchButton.className = 'control-item clock';

            this.componentCleanButton = document.createElement('div');
            this.componentCleanButton.className = 'control-item clean';

            this.calendarSwitchButton = document.createElement('div');
            this.calendarSwitchButton.className = 'control-item calendar';

            this.clockControl.insertBefore(this.clockSwitchButton, this.clockControl.firstElementChild);

            this.calendarControl.insertBefore(this.componentCleanButton, this.calendarControl.firstElementChild);
            this.calendarControl.insertBefore(this.calendarSwitchButton, this.calendarControl.firstElementChild);
        }

        if (this.calendarControl !== undefined) {

            datetimeFooter.appendChild(this.calendarControl);
        }

        if (this.clockControl !== undefined) {

            datetimeFooter.appendChild(this.clockControl);
        }

        this.datetime.appendChild(datetimeHeader);
        this.datetime.appendChild(datetimeContent);
        this.datetime.appendChild(datetimeFooter);

        if (this.options.popup || this.mobile) {

            this.overlay = document.createElement('div');
            this.overlay.className = 'datetime-overlay';

            this.overlay.appendChild(this.datetime);

            document.body.appendChild(this.overlay);

        } else {

            this.element.parentNode.insertBefore(this.datetime, this.element.nextSibling);
        }

        _bindElementEvent.call(this);
    }

    function _buildCalendarWeek() {

        let date = _getFirstDay.call(this);

        let contentHTML = ['<div class="calendar-day">'];

        for (let i = this.firstWeekday; i <= this.lastWeekday; i++) {

            contentHTML.push(`<div class="item">${date.toLocaleString(this.options.locale, { weekday: this.options.weekday }).toUpperCase()}</div>`);

            date.setDate(date.getDate() + 1);
        }

        contentHTML.push('</div>');

        return contentHTML.join('');
    }

    function _buildDate(date) {

        let element = document.createElement('div');

        element.classList = 'item';
        element.textContent = date.getUTCDate();

        const disabled = date.getMonth() !== this.month ? true : !date.inRange(this.options.minDate, this.options.maxDate);

        if (disabled) {

            element.classList.add('disabled');

        } else {

            if (this.options.today && date.compareTo(this.today)) {

                element.classList.add('today');
            }

            if (this.selected !== null && date.compareTo(this.selected)) {

                element.classList.add('selected');
            }
        }

        return element.outerHTML;
    }

    function _buildMonth() {

        let contentHTML = [];

        const dates = _getMonthDays.call(this);

        for (let i = 0; i < dates.length; i++) {

            const date = dates[i];

            const weekday = dates[i].getDay();

            if (!this.options.overflow && i === 0 && weekday !== this.firstWeekday) {

                contentHTML.push('<div class="calendar-week">');

                for (let j = this.firstWeekday; j < weekday; j++) {

                    contentHTML.push('<div class="item disabled"></div>');
                }
            }

            if (weekday === this.firstWeekday) contentHTML.push('<div class="calendar-week">');

            contentHTML.push(_buildDate.call(this, date));

            if (weekday === this.lastWeekday) contentHTML.push('</div>');
        }

        if (!this.options.overflow && dates[i].getDay() !== this.lastWeekday) {

            for (let k = dates[i].getDay(); k < this.lastWeekday; k++) {

                contentHTML.push('<div class="item disabled"></div>');
            }

            contentHTML.push('</div>');
        }

        this.calendarDate.innerHTML = contentHTML.join('');
    }

    function _bindEvent() {

        this.element.addEventListener('keydown', this._events.elementKeyDown);
        this.element.addEventListener('mouseup', this._events.elementMouseUp);

        document.addEventListener('mousedown', this._events.documentMouseDown);
    }

    function _handleElementKeyDown(event) {

        if ([Key.backspace, Key.enter, Key.refresh].indexOf(event.keyCode) === -1) {

            if (/[a-zA-Z]/.test(event.key)) event.preventDefault();
        }
    }

    function _handleElementMouseUp(event) {

        this.destroy();

        _build.call(this);

        this.open(event.target.value);
    }

    function _handleClickOutside(event) {

        if (!_findAncestor(event.target, 'datetime') && event.target !== this.element) {

            this.destroy();
        }
    }

    function _bindElementEvent() {

        if (this.component === Component.DATE_TIME || this.component === Component.DATE) {

            this.calendarYearArrowUp.addEventListener('click', this._events.calendarArrow);
            this.calendarYearArrowDown.addEventListener('click', this._events.calendarArrow);

            this.calendarMonthArrowUp.addEventListener('click', this._events.calendarArrow);
            this.calendarMonthArrowDown.addEventListener('click', this._events.calendarArrow);

            this.calendarDate.addEventListener('click', this._events.calendarDateClick);

            this.calendarTodayButton.addEventListener('mouseup', this._events.calendarTodayButton);
            this.calendarConfirmButton.addEventListener('mouseup', this._events.confirmComponentButton);
        }

        if (this.component === Component.DATE_TIME || this.component === Component.TIME) {

            this.clockHourArrowUp.addEventListener('mousedown', this._events.clockArrowMouseDown);
            this.clockHourArrowUp.addEventListener('mouseup', this._events.clockArrowMouseUpLeave);
            this.clockHourArrowUp.addEventListener('mouseleave', this._events.clockArrowMouseUpLeave);

            this.clockHourArrowDown.addEventListener('mousedown', this._events.clockArrowMouseDown);
            this.clockHourArrowDown.addEventListener('mouseup', this._events.clockArrowMouseUpLeave);
            this.clockHourArrowDown.addEventListener('mouseleave', this._events.clockArrowMouseUpLeave);

            this.clockMinuteArrowUp.addEventListener('mousedown', this._events.clockArrowMouseDown);
            this.clockMinuteArrowUp.addEventListener('mouseup', this._events.clockArrowMouseUpLeave);
            this.clockMinuteArrowUp.addEventListener('mouseleave', this._events.clockArrowMouseUpLeave);

            this.clockMinuteArrowDown.addEventListener('mousedown', this._events.clockArrowMouseDown);
            this.clockMinuteArrowDown.addEventListener('mouseup', this._events.clockArrowMouseUpLeave);
            this.clockMinuteArrowDown.addEventListener('mouseleave', this._events.clockArrowMouseUpLeave);

            this.clockConfirmButton.addEventListener('mouseup', this._events.confirmComponentButton);
        }

        if (this.component === Component.DATE_TIME) {

            this.clockSwitchButton.addEventListener('click', this._events.switchComponentButton);
            this.componentCleanButton.addEventListener('click', this._events.cleanComponentButton);
            this.calendarSwitchButton.addEventListener('click', this._events.switchComponentButton);
        }
    }

    function _handleCalendarArrow(event) {

        if (event.target) {

            const month = /month/.test(event.target.parentNode.className);

            if (event.target.classList.contains('up')) {

                if (month) {

                    _moveMonth.call(this, 1);

                } else {

                    _moveYear.call(this, 1);
                }

            } else if (event.target.classList.contains('down')) {

                if (month) {

                    _moveMonth.call(this, -1);

                } else {

                    _moveYear.call(this, -1);
                }
            }
        }
    }

    function _handleCalendarDateClick(event) {

        if (event.target && event.target.classList.contains('item') && !event.target.classList.contains('disabled')) {

            let day = parseInt(event.target.textContent, 10);

            let date = new Date(this.year, this.month, day);

            _setSelectedDate.call(this, date);
        }
    }

    function _handleCalendarTodayButton(event) {

        _setSelectedDate.call(this, this.today);
    }

    function _handleConfirmComponentButton(event) {

        this.close();
    }

    function _handleSwitchComponentButton(event) {

        if (event.target && !/active/.test(event.target.className)) {

            this.switch();
        }
    }

    function _handleCleanComponentButton(event) {

        this.close();
        this.element.blur();
        this.element.value = '';
    }

    function _handleClockArrowMouseDown(event) {

        if (event.target) {

            this.counter = 0;

            const hour = /hour/.test(event.target.parentNode.className);

            if (event.target.classList.contains('up')) {

                if (hour) {

                    _moveHour.call(this, 1);
                    _hourTimer.call(this, true, 1);

                } else {

                    _moveMinute.call(this, 1);
                    _minuteTimer.call(this, true, 1);
                }

            } else if (event.target.classList.contains('down')) {

                if (hour) {

                    _moveHour.call(this, -1);
                    _hourTimer.call(this, true, -1);

                } else {

                    _moveMinute.call(this, -1);
                    _minuteTimer.call(this, true, -1);
                }
            }
        }
    }

    function _handleClockArrowMouseUpLeave(event) {

        if (event.target) {

            const hour = /hour/.test(event.target.parentNode.className);

            if (hour) {

                _hourTimer.call(this, false);

            } else {

                _minuteTimer.call(this, false);
            }
        }
    }

    function _unbindEvent() {

        this.element.removeEventListener('keydown', this._events.elementKeyDown);
        this.element.removeEventListener('mouseup', this._events.elementMouseUp);

        document.removeEventListener('mousedown', this._events.documentMouseDown);
    }

    function _unbindElementEvent() {

        if (this.component === Component.DATE_TIME || this.component === Component.DATE) {

            this.calendarYearArrowUp.removeEventListener('click', this._events.calendarArrow);
            this.calendarYearArrowDown.removeEventListener('click', this._events.calendarArrow);

            this.calendarMonthArrowUp.removeEventListener('click', this._events.calendarArrow);
            this.calendarMonthArrowDown.removeEventListener('click', this._events.calendarArrow);

            this.calendarDate.removeEventListener('click', this._events.calendarDateClick);

            this.calendarTodayButton.removeEventListener('mouseup', this._events.calendarTodayButton);
            this.calendarConfirmButton.removeEventListener('mouseup', this._events.confirmComponentButton);
        }

        if (this.component === Component.DATE_TIME || this.component === Component.TIME) {

            this.clockHourArrowUp.removeEventListener('mousedown', this._events.clockArrowMouseDown);
            this.clockHourArrowUp.removeEventListener('mouseup', this._events.clockArrowMouseUpLeave);
            this.clockHourArrowUp.removeEventListener('mouseleave', this._events.clockArrowMouseUpLeave);

            this.clockHourArrowDown.removeEventListener('mousedown', this._events.clockArrowMouseDown);
            this.clockHourArrowDown.removeEventListener('mouseup', this._events.clockArrowMouseUpLeave);
            this.clockHourArrowDown.removeEventListener('mouseleave', this._events.clockArrowMouseUpLeave);

            this.clockMinuteArrowUp.removeEventListener('mousedown', this._events.clockArrowMouseDown);
            this.clockMinuteArrowUp.removeEventListener('mouseup', this._events.clockArrowMouseUpLeave);
            this.clockMinuteArrowUp.removeEventListener('mouseleave', this._events.clockArrowMouseUpLeave);

            this.clockMinuteArrowDown.removeEventListener('mousedown', this._events.clockArrowMouseDown);
            this.clockMinuteArrowDown.removeEventListener('mouseup', this._events.clockArrowMouseUpLeave);
            this.clockMinuteArrowDown.removeEventListener('mouseleave', this._events.clockArrowMouseUpLeave);

            this.clockConfirmButton.removeEventListener('mouseup', this._events.confirmComponentButton);
        }

        if (this.component === Component.DATE_TIME) {

            this.clockSwitchButton.removeEventListener('click', this._events.switchComponentButton);
            this.componentCleanButton.removeEventListener('click', this._events.cleanComponentButton);
            this.calendarSwitchButton.removeEventListener('click', this._events.switchComponentButton);
        }
    }

    function _config() {

        this.popup = typeof this.element.dataset.popup !== 'undefined' ? true : false;

        if (typeof this.element.dataset.component !== 'undefined') {

            if (this.element.dataset.component === 'time') {

                this.component = Component.TIME;

            } else if (this.element.dataset.component === 'date') {

                this.component = Component.DATE;

            } else {

                this.component = Component.DATE_TIME;
            }
        } else {

            this.component = Component.DATE_TIME;
        }

        this.element.setAttribute('autocomplete', 'off');

        if (!this.options.weekend) {

            this.lastWeekday = 5;
            this.firstWeekday = 1;
        }

        this.mobile = isMobile();

        this._events = {
            calendarArrow: _handleCalendarArrow.bind(this),
            elementKeyDown: _handleElementKeyDown.bind(this),
            elementMouseUp: _handleElementMouseUp.bind(this),
            documentMouseDown: _handleClickOutside.bind(this),
            calendarDateClick: _handleCalendarDateClick.bind(this),
            clockArrowMouseDown: _handleClockArrowMouseDown.bind(this),
            calendarTodayButton: _handleCalendarTodayButton.bind(this),
            cleanComponentButton: _handleCleanComponentButton.bind(this),
            switchComponentButton: _handleSwitchComponentButton.bind(this),
            confirmComponentButton: _handleConfirmComponentButton.bind(this),
            clockArrowMouseUpLeave: _handleClockArrowMouseUpLeave.bind(this)
        };
    }

    function _moveMonth(direction) {

        let year = this.year;

        let month = this.month;

        this.year = _getYear.call(this, direction);

        this.month = _getMonth.call(this, direction);

        let day = _getDay.call(this, direction);

        let date = new Date(this.year, this.month, day);

        if (this.options.minDate === null && this.options.maxDate === null || this.options.minDate !== null && date >= this.options.minDate || this.options.maxDate !== null && date <= this.options.maxDate || date >= this.options.minDate && date <= this.options.maxDate) {

            this.day = day;

            _setSelectedDate.call(this, date);

            _buildMonth.call(this);

        } else {

            this.year = year;

            this.month = month;
        }
    }

    function _moveYear(direction) {

        let day = this.day;

        let year = this.year;

        this.year += direction;

        this.day = _getDay.call(this, direction);

        let date = new Date(this.year, this.month, this.day);

        if (this.options.minDate === null && this.options.maxDate === null || this.options.minDate !== null && date >= this.options.maxDate || this.options.maxDate !== null && date <= this.options.maxDate || date >= this.options.minDate && date <= this.options.maxDate) {

            _setSelectedDate.call(this, date);

            _buildMonth.call(this);

        } else {

            this.day = day;

            this.year = year;
        }
    }

    function _getDay(direction) {

        let days = _getMonthDays.call(this, false);

        if (this.selected !== null && this.month === this.selected.getMonth() && this.year === this.selected.getFullYear()) {

            return this.selected.getDate();
        }

        let day = (direction > 0 ? days[0] : days[days.length - 1]).getDate();

        if (this.options.minDate !== null && this.month === this.options.minDate.getMonth() && this.year === this.options.minDate.getFullYear() && direction < 0) {

            return this.options.minDate.getDate();
        }

        if (this.options.maxDate !== null && this.month === this.options.maxDate.getMonth() && this.year === this.options.maxDate.getFullYear() && direction > 0) {

            return this.options.maxDate.getDate();
        }

        return day;
    }

    function _getMonth(direction) {

        let month = this.month + direction;

        return month <= 11 ? month === -1 ? 11 : month : 0;
    }

    function _getYear(direction) {

        let month = this.month + direction;

        return month <= 11 ? month === -1 ? this.year - 1 : this.year : this.year + 1;
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

    function _setSelectedDate(date) {

        if (this.component === Component.DATE_TIME || this.component === Component.DATE) {

            this.setDate(date);

            this.calendarHeaderMonth.innerHTML = date.toLocaleString(this.options.locale, { month: 'short' });

            this.calendarHeaderYear.innerHTML = this.year;

            this.calendarHeaderDay.innerHTML = date.toLocaleString(this.options.locale, { day: 'numeric' });

            this.calendarHeaderWeekday.innerHTML = date.toLocaleString(this.options.locale, { weekday: 'long' });

            _buildMonth.call(this);
        }
    }

    function _isValidDate(date) {

        if (this.options.minDate === null && this.options.maxDate === null) {

            _setSelectedDate.call(this, date);

        } else if (this.options.minDate !== null && date >= this.options.minDate) {

            _setSelectedDate.call(this, date);

        } else if (this.options.maxDate !== null && date <= this.options.maxDate) {

            _setSelectedDate.call(this, date);

        } else {

            if (this.options.minDate !== null) {

                _setSelectedDate.call(this, this.options.minDate);

            } else if (this.options.maxDate !== null) {

                _setSelectedDate.call(this, this.options.maxDate);
            }
        }
    }

    function _moveHour(direction) {

        let hour = parseInt(this.clockHour.value, 10) + direction;

        hour = hour > this.clockHour.max ? this.clockHour.min : hour < this.clockHour.min ? this.clockHour.max : hour;

        if (this.options.clock === null) {

            this.setHour(hour);

        } else {

            //this.verificarRegraHora(hour);
        }

        return hour;
    }

    function _hourTimer(start, direction) {

        if (start) {

            let self = this;

            this.timer = setInterval(function () {

                if (self.counter > .5) {

                    _moveHour.call(self, direction * 2);

                } else {

                    _moveHour.call(self, direction);
                }

                self.counter += .1;

            }, 100);

        } else {

            clearInterval(this.timer);
        }
    }

    function _moveMinute(direction) {

        let minute = parseInt(this.clockMinute.value, 10) + direction;

        minute = minute > this.clockMinute.max ? this.clockMinute.min : minute < this.clockMinute.min ? this.clockMinute.max : minute;

        if (this.options.clock === null) {

            this.setMinute(minute);

        } else {

            //this.verificarRegraMinuto(minute);
        }

        return minute;
    }

    function _minuteTimer(start, direction) {

        if (start) {

            let self = this;

            this.timer = setInterval(function () {

                if (self.counter > 1) {

                    _moveMinute.call(self, direction * 3);

                } else {

                    _moveMinute.call(self, direction);
                }

                self.counter += .1;

            }, 100);

        } else {

            clearInterval(this.timer);
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

    function isMobile() {

        let mobile = false;

        (function (a) {

            if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino|android|ipad|playbook|silk/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) {

                check = true;
            }

        })(navigator.userAgent || navigator.vendor || window.opera);

        return mobile;
    }

    String.prototype.toDate = function () {

        if (this !== '' && this !== null) {

            let date = this.split('/');

            if (date.length === 3) {

                return new Date(date[2], date[1] - 1, date[0]);
            }

            return new Date();
        }

        return null;
    };

    String.prototype.toTime = function () {

        let hour = '00', minute = '00';

        let indexOf = this.indexOf(':');

        if (indexOf !== -1) {

            hour = this.substring(0, indexOf).trim();

            minute = this.substring(indexOf + 1, this.length).trim();
        }

        return [hour, minute];
    };

    Number.prototype.format = function () {

        return this.toString().length > 1 ? this : '0' + this;
    };

    Date.prototype.compareTo = function (date) {

        if (date === undefined) return false;

        return this.getDate() === date.getDate() && this.getMonth() === date.getMonth() && this.getFullYear() === date.getFullYear();
    };

    Date.prototype.inRange = function (start, end) {

        if (start === undefined || end === undefined) return true;

        return (start === null || this >= start) && (end === null || this <= end);
    };

    /* ----------------------------------------------------------- */
    /* == return */
    /* ----------------------------------------------------------- */

    return {
        init: Datetimepicker,
        get: Get
    };

}));
