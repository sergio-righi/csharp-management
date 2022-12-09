
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.autocomplete = factory();
    }

}(this, function () {

    const Key = {
        tab: 9,
        up: 38,
        esc: 27,
        down: 40,
        enter: 13,
        backspace: 8
    };

    function Get(element) {

        return dataStorage.get(element, 'autocomplete');
    }

    function Autocomplete(element, options) {

        let defaults = {
            min: 0,
            limit: 8,
            max: null,
            onLoad: null,
            onInput: null,
            return: 'TEXT',
            duplicity: false
        };

        this.chips = [];
        this.items = null;
        this.element = element;

        this.configurations = {
            focus: false,
            required: false,
            readOnly: false
        };

        this.options = extend({}, defaults, options);

        this.init();
    }

    Autocomplete.prototype.init = function (element) {

        if (this.autocomplete) {
            return;
        }

        _build.call(this);
        _config.call(this);

        if (this.options.onLoad !== null) {

            this.options.onLoad(this);
        }

        _bindEvent.call(this);

        if (this.element.value.length > 0) {

            _rebuild.call(this);
        }

        dataStorage.put(this.element, 'autocomplete', this);

        return this;
    };

    Autocomplete.prototype.destroy = function () {

        if (this.autocomplete === null) {
            return;
        }

        _unbindEvent.call(this);

        this.autocomplete.parentNode.removeChild(this.autocomplete);

        this.autocomplete = null;
    };

    Autocomplete.prototype.update = function () {

        _config.call(this);

        return this;
    };

    Autocomplete.prototype.getLast = function () {

        return typeof this.dropdown !== 'undefined' ? this.dropdown : this.component;
    };

    Autocomplete.prototype.add = function (id, text) {

        let chip = _create.call(this, id, text);

        if (chip !== null) {

            let element = this.getLast();

            if (element !== null) {

                element.parentNode.insertBefore(chip, element);

                this.chips.push({ id: chip.dataset.id, text: chip.dataset.text, key: chip.dataset.key });

                _verifyQuantity.call(this);
                _verifyRequired.call(this);
            }

            this.component.value = '';
        }

        return chip;
    };

    Autocomplete.prototype.remove = function (element) {

        const index = this.chips.findIndex(x => x.key === element.dataset.key);

        if (index !== -1) {

            this.chips.splice(index, 1);

            element.parentNode.removeChild(element);
        }

        _verifyQuantity.call(this);
        _verifyRequired.call(this);
    };

    Autocomplete.prototype.reset = function () {

        this.chips = [];
        this.component.classList.remove('d-none');
        //this.autocomplete.querySelectorAll('.chip');//.remove();

        return this;
    }

    /* ----------------------------------------------------------- */
    /* == private methods */
    /* ----------------------------------------------------------- */

    function _build() {

        this.element.classList.add('d-none');

        this.autocomplete = document.createElement('div');
        this.autocomplete.classList.add('autocomplete');

        this.component = document.createElement('input');
        this.component.setAttribute('type', 'text');

        if (this.element.autofocus !== undefined) {
            this.component.setAttribute('autofocus', this.element.autofocus);
        }

        if (this.element.placeholder !== undefined) {
            this.component.setAttribute('placeholder', this.element.placeholder);
        }

        this.component.classList.add('autocomplete-component');

        if (this.options.onInput !== null || this.options.onLoad !== null) {

            this.dropdown = document.createElement('div');
            this.dropdown.classList.add('autocomplete-dropdown');

            this.dropdownContent = document.createElement('ul');
            this.dropdownContent.classList.add('dropdown-content');

            this.dropdown.appendChild(this.component);
            this.dropdown.appendChild(this.dropdownContent);

            this.autocomplete.appendChild(this.dropdown);

        } else {

            this.autocomplete.appendChild(this.component);
        }

        this.element.parentNode.insertBefore(this.autocomplete, this.element);
    }

    function _config() {

        this.configurations.focus = this.element.hasAttribute('autofocus');

        if (this.focus) {

            this.component.focus();
        }

        this.configurations.required = this.element.hasAttribute('required');

        if (this.configurations.required) {

            this.component.setAttribute('required', 'required');

            this.element.removeAttribute('required');
        }

        this.configurations.readOnly = this.element.hasAttribute('readonly') || this.element.hasAttribute('disabled');

        if (this.configurations.readOnly) {

            this.component.classList.add('d-none');
        }
    }

    function _rebuild() {

        let elements = [], self = this;

        let json = this.element.value.isJSON();

        if (json) {

            elements = JSON.parse(this.element.value);

        } else {

            elements = this.element.value.split(',');
        }

        Array.prototype.forEach.call(elements, function (item, i) {

            if (json) {

                self.add(item.ID, item.TEXT.trim());

            } else {

                self.add(0, item.trim());
            }
        });
    }

    function _create(id, text) {

        let chip = document.createElement('span');

        chip.title = text;
        chip.className = 'chip';
        chip.setAttribute('data-id', id);
        chip.setAttribute('data-text', text);
        chip.setAttribute('data-key', Math.random().toString(36).substr(2, 16));

        chip.innerHTML = '<span class="chip-title">' + text + '</span>';

        if (!this.configurations.readOnly) {

            chip.innerHTML += '<span class="chip-control"></span>';
        }

        return chip;
    }

    function _bindEvent() {

        this._events = {
            formReset: this.reset.bind(this),
            formSubmit: _handleFormSubmit.bind(this),
            componentBlur: _handleComponentBlur.bind(this),
            componentFocus: _handleComponentFocus.bind(this),
            componentKeyUp: _handleComponentKeyUp.bind(this),
            componentKeyDown: _handleComponentKeyDown.bind(this),
            chipControlRemove: _handleChipControlRemove.bind(this),
            autocompleteClick: _handleAutocompleteClick.bind(this),
            dropdownContentMouseDown: _handleDropdownContentMouseDown.bind(this)
        };

        if (this.element.form !== null) {

            this.element.form.addEventListener('reset', this._events.formReset);
            this.element.form.addEventListener('submit', this._events.formSubmit);
        }

        this.component.addEventListener('blur', this._events.componentBlur);
        this.component.addEventListener('focus', this._events.componentFocus);
        this.component.addEventListener('keydown', this._events.componentKeyDown);

        if (this.options.onInput !== null || this.options.onLoad !== null) {

            this.component.addEventListener('keyup', this._events.componentKeyUp);

            this.dropdownContent.addEventListener('mousedown', this._events.dropdownContentMouseDown);
        }

        this.autocomplete.addEventListener('click', this._events.autocompleteClick);

        document.addEventListener('mouseup', this._events.chipControlRemove);
    }

    function _handleFormSubmit(event) {

        if (this.chips.length >= this.options.min && (this.options.max === null || this.options.max >= this.chips.length)) {

            if (!this.configurations.readOnly) {

                let elements = [];

                const self = this;

                this.chips.forEach(function (item, i) {

                    if (self.options.return === 'ID') {

                        elements.push(item.id);

                    } else {

                        elements.push(item.text);
                    }

                });

                this.element.value = elements.join(', ');
            }

        } else {

            event.preventDefault();
        }
    }

    function _handleComponentKeyUp(event) {

        if (this.component.value.length > 0) {

            if (event.which !== Key.up && event.which !== Key.down) {

                let contentHTML = [];

                this.showing = 0;

                if (this.typingTimer) {

                    clearTimeout(this.typingTimer);
                }

                if (this.options.onInput !== null) this.options.onInput(this);

                if (this.items !== null) {

                    let self = this;

                    this.typingTimer = setTimeout(function (e) {

                        let autocomplete = self.items.searchFor(self.component.value, self.chips, self.options.duplicity, self.options.limit);

                        if (autocomplete.length > 0) {

                            self.showing = autocomplete.length;

                            self.dropdown.classList.add('active');

                            Array.prototype.forEach.call(autocomplete, function (item, i) {

                                let li = document.createElement('li');

                                if (i === 0) li.className = 'selected';

                                if (typeof item === 'string') {

                                    li.innerText = item;
                                    li.setAttribute('data-id', 0);

                                } else {

                                    li.innerText = item.TEXT;
                                    li.setAttribute('data-id', item.ID);
                                }

                                contentHTML.push(li.outerHTML);
                            });

                        } else {

                            self.dropdown.classList.remove('active');
                        }

                        self.dropdownContent.innerHTML = contentHTML.join('');

                    }, 100);

                } else {

                    this.dropdown.classList.remove('active');
                }
            }

        } else {

            this.dropdown.classList.remove('active');

            let selected = this.dropdownContent.querySelectorAll('li.selected')[0];

            if (selected !== undefined) {

                selected.classList.remove('selected');
            }
        }

        return false;
    }

    function _handleComponentKeyDown(event) {

        switch (event.which) {

            case Key.esc:

                this.component.value = '';

                if (this.dropdown !== undefined) {

                    this.dropdown.classList.remove('active');
                }

                break;
            case Key.backspace:

                if (this.chips.length > 0 && this.component.value.length === 0) {

                    let element = this.getLast();

                    if (element !== null) {

                        element = element.previousElementSibling;

                        if (element.classList.contains('chip')) {

                            if (element.classList.contains('active')) {

                                this.remove(element);

                            } else {

                                element.classList.add('active');
                            }
                        }
                    }
                }

                break;
            case Key.tab:
            case Key.enter:

                if (this.component.value.length > 0) {

                    event.preventDefault();
                }

                if (this.options.onInput !== null || this.options.onLoad !== null) {

                    this.dropdown.classList.remove('active');

                    if (this.showing > 0) {

                        let selected = this.dropdownContent.querySelectorAll('li.selected')[0];

                        if (selected !== undefined) {

                            this.add(selected.dataset.id, selected.textContent);

                            selected.classList.remove('selected');
                        }
                    }

                    this.component.value = '';

                } else {

                    if (this.component.value.length > 0) {

                        if (this.options.duplicity || (!this.options.duplicity && this.chips.indexOf(this.component.value.toISO()) === -1)) {

                            this.add(0, this.component.value);
                        }
                    }
                }

                break;
            case Key.up:
            case Key.down:

                if (this.options.onInput !== null || this.options.onLoad !== null) {

                    event.preventDefault();

                    if (this.dropdown.classList.contains('active') && this.showing > 0) {

                        let selected = null;

                        let element = this.dropdownContent.querySelectorAll('li.selected')[0];

                        const index = element.index();

                        if (e.which === Key.CIMA) {

                            if (index !== 0) {

                                selected = element.previousElementSibling;

                                selected.classList.add('selected');

                                element.classList.remove('selected');

                            } else {

                                selected = element.parentNode.lastElementChild;

                                selected.classList.add('selected');

                                element.classList.remove('selected');
                            }

                        } else if (e.which === Key.BAIXO) {

                            if (index !== this.showing - 1) {

                                selected = element.nextElementSibling;

                                selected.classList.add('selected');

                                element.classList.remove('selected');

                            } else {

                                selected = element.parentNode.firstElementChild;

                                selected.classList.add('selected');

                                element.classList.remove('selected');
                            }
                        }

                        //this.$suspensoConteudo.animate({ // mover o scroll para o item selecionado

                        //    scrollTop: $selecionado[0].offsetTop

                        //}, 100);
                    }
                }

                break;
            default:

                if (this.chips.length > 0) {

                    let element = this.getLast();

                    if (element.classList.contains('chip')) {

                        element.classList.remove('active');
                    }
                }
        }
    }

    function _handleComponentFocus(event) {

        this.component.value = '';
        this.autocomplete.classList.add('focus');

        if (this.dropdown !== undefined) {

            this.dropdown.classList.remove('active');
        }
    }

    function _handleComponentBlur(event) {

        this.component.value = '';
        this.autocomplete.classList.remove('focus');

        if (this.dropdown !== undefined) {

            this.dropdown.classList.remove('active');
        }
    }

    function _handleChipControlRemove(event) {

        if (event.target && event.target.className === 'chip-control') {

            this.remove(event.target.parentNode);
        }
    }

    function _handleAutocompleteClick(event) {

        this.component.focus();
    }

    function _handleDropdownContentMouseDown(event) {

        if (event.target && event.target.nodeName === 'LI') {

            let chip = this.add(event.target.dataset.id, event.target.textContent);

            if (chip !== null) {

                this.dropdown.classList.remove('active');

                this.component.value = '';
                this.component.focus();
            }
        }
    }

    function _unbindEvent() {

        if (this.element.form !== null) {

            this.element.form.removeEventListener('reset', this._events.formReset);
            this.element.form.removeEventListener('submit', this._events.formSubmit);
        }

        this.component.removeEventListener('blur', this._events.componentBlur);
        this.component.removeEventListener('focus', this._events.componentFocus);
        this.component.removeEventListener('keydown', this._events.componentKeyDown);

        if (this.options.onInput !== null || this.options.onLoad !== null) {

            this.component.removeEventListener('keyup', this._events.componentKeyUp);

            this.dropdownContent.removeEventListener('mousedown', this._events.dropdownContentMouseDown);
        }

        this.autocomplete.removeEventListener('click', this._events.autocompleteClick);

        document.removeEventListener('mouseup', this._events.chipControlRemove);
    }

    function _verifyRequired() {

        if (this.configurations.required || this.options.min !== null) {

            if (this.chips.length === 0 || (this.options.min !== null && this.options.min > this.chips.length)) {

                this.component.setAttribute('required', 'required');

            } else {

                this.component.removeAttribute('required');
            }
        }
    }

    function _verifyQuantity() {

        if (this.options.max === null || this.chips.length <= this.options.max - 1) {

            this.component.classList.remove('d-none');
            this.component.focus();

        } else {

            this.component.blur();
            this.component.classList.add('d-none');
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

    Element.prototype.index = function () {

        return Array.from(this.parentElement.children).indexOf(this);
    };

    String.prototype.normalize = function (e) {

        return this.toLowerCase().toISO();
    };

    String.prototype.toISO = function () {

        return this.toUpperCase().replace(new RegExp('[ÁÀÂÃÄ]', 'gi'), 'A').replace(new RegExp('[ÉÈÊË]', 'gi'), 'E').replace(new RegExp('[ÍÌÎÏ]', 'gi'), 'I').replace(new RegExp('[ÓÒÔÕÖ]', 'gi'), 'O').replace(new RegExp('[ÚÙÛÜ]', 'gi'), 'U').replace(new RegExp('[Ç]', 'gi'), 'C').toLowerCase();
    };

    String.prototype.isJSON = function () {

        try {

            JSON.parse(this);

        } catch (e) {

            return false;
        }

        return true;
    };

    Array.prototype.searchFor = function (value, list, duplicity, quantity) {

        let items = [];

        this.forEach(function (item, i) {

            if (typeof item === 'string') {

                if (item.normalize().match(value.normalize()) && (duplicity || (!duplicity && list.findIndex(x => x.text.normalize() === item.normalize()) === -1))) items.push(item);

            } else {

                if (item.TEXT.normalize().match(value.normalize()) && (duplicity || (!duplicity && list.findIndex(x => x.text.normalize() === item.TEXT.normalize()) === -1))) items.push(item);
            }

            if (items.length === quantity) return false;

        });

        return items;
    };

    /* ----------------------------------------------------------- */
    /* == return */
    /* ----------------------------------------------------------- */

    return {
        init: Autocomplete,
        get: Get
    };

}));