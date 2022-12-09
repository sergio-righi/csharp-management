
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.card = factory();
    }

}(this, function () {

    const Info = {
        name: 'card',
        version: '2.0'
    };

    const Default = {
        button: 'Lorem ipsum dolor sit amet',
        header: 'Lorem ipsum dolor sit amet',
        subheader: 'Lorem ipsum dolor sit amet',
        paragraph: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla risus nulla, consequat sed tincidunt et, rutrum at tellus. Nullam ut quam nec orci tincidunt venenatis. Etiam eu metus enim. Duis eget pretium lectus, mollis ultrices nisi. Morbi gravida libero magna, vitae tempor turpis gravida ut. Integer viverra elementum enim at accumsan. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Pellentesque vitae malesuada quam.'
    };

    /* ----------------------------------------------------------- */
    /* == component */
    /* ----------------------------------------------------------- */

    function EditorCard(context) {

        this.header = true;
        this.button = true;
        this.subheader = true;
        this.paragraph = true;

        this.context = context;

        _bindEvent.call(this);

        return this;
    }

    EditorCard.prototype.getControl = function () {

        let configurationBlock = document.createElement('div');
        configurationBlock.className = 'configuration-block';

        configurationBlock.appendNode('button', 'Header', { 'type': 'button', 'name': 'card_header', 'class': 'action-item' });

        let configurationBlock1 = document.createElement('div');
        configurationBlock1.className = 'configuration-block';

        configurationBlock1.appendNode('button', 'Subheader', { 'type': 'button', 'name': 'card_subheader', 'class': 'action-item' });

        let configurationBlock2 = document.createElement('div');
        configurationBlock2.className = 'configuration-block';

        configurationBlock2.appendNode('button', 'Paragraph', { 'type': 'button', 'name': 'card_paragraph', 'class': 'action-item' });

        let configurationBlock3 = document.createElement('div');
        configurationBlock3.className = 'configuration-block';

        configurationBlock3.appendNode('button', 'Button', { 'type': 'button', 'name': 'card_button', 'class': 'action-item' });

        let configurationBlock4 = document.createElement('div');
        configurationBlock4.className = 'configuration-block';

        let elementDropdown = document.createElement('div');
        elementDropdown.className = 'dropdown';

        let dropdownContent = document.createElement('ul');
        dropdownContent.className = 'dropdown-content left';

        let elements = [
            { text: 'Sem ícone', value: '' },
            { text: '<i class="mdi mdi-exclamation"></i> Atenção', value: 'exclamation' },
            { text: '<i class="mdi mdi-magnify"></i> Consultar', value: 'magnify' },
            { text: '<i class="mdi mdi-calendar-blank"></i> Calendário', value: 'calendar-blank' },
            { text: '<i class="mdi mdi-share-variant"></i> Compartilhar', value: 'share-variant' },
            { text: '<i class="mdi mdi-phone"></i> Contato', value: 'phone' },
            { text: '<i class="mdi mdi-star"></i> Destaque', value: 'star' },
            { text: '<i class="mdi mdi-bullhorn"></i> Divulgação', value: 'bullhorn' },
            { text: '<i class="mdi mdi-download"></i> Download', value: 'download' },
            { text: '<i class="mdi mdi-help"></i> Dúvida', value: 'magnify' },
            { text: '<i class="mdi mdi-information-variant"></i> Informação', value: 'information-variant' },
            { text: '<i class="mdi mdi-account-edit"></i> Inscrição', value: 'account-edit' },
            { text: '<i class="mdi mdi-printer"></i> Imprimir', value: 'printer' },
            { text: '<i class="mdi mdi-map-marker"></i> Localização', value: 'map-marker' },
            { text: '<i class="mdi mdi-barcode"></i> Pagamento', value: 'barcode' },
            { text: '<i class="mdi mdi-taxi"></i> Transporte', value: 'taxi' }
        ];

        dropdownContent.appendToList(elements);

        elementDropdown.appendNode('button', 'Icon', { 'type': 'button', 'name': 'card_icon', 'class': 'action-item dropdown-trigger' });

        elementDropdown.appendChild(dropdownContent);

        configurationBlock4.appendChild(elementDropdown);

        return configurationBlock.outerHTML + configurationBlock1.outerHTML + configurationBlock2.outerHTML + configurationBlock3.outerHTML + configurationBlock4.outerHTML;
    };

    EditorCard.prototype.getContent = function () {

        return `<div class="card" data-component="${Info.name}" data-version="${Info.version}">` +
            '<div class="card-info">' +
            `<div class="card-header" contenteditable="true">${Default.header}</div>` +
            `<div class="card-header small" contenteditable="true">${Default.subheader}</div>` +
            '<div class="card-icon"></div></div>' +
            '<div class="card-content">' +
            `<p contenteditable="true">${Default.paragraph}</p>` +
            '</div>' +
            '<div class="card-control">' +
            `<a href="#" class="card-button">${Default.button}</a>` +
            '</div>' +
            '</div>';
    };

    /* ----------------------------------------------------------- */
    /* == private methods */
    /* ----------------------------------------------------------- */

    function _bindEvent() {

        this._events = {
            actionClick: _handleActionClick.bind(this)
        };

        this.context.sectionBuild.addEventListener('click', this._events.actionClick);
    }

    function _unbindEvent() {

    }

    function _handleActionClick(e) {

        let element = e.target.closestNode('button') || e.target.closestClass('action-icon');

        if (element !== null) {

            if (/card_.*?/g.test(element.name)) {

                e.preventDefault();
                e.stopPropagation();

                switch (element.name) {
                    case 'card_header':
                        _toggleHeader(element);
                        break;
                    case 'card_subheader':
                        _toggleSubheader(element);
                        break;
                    case 'card_paragraph':
                        _toggleParagraph(element);
                        break;
                    case 'card_button':
                        _toggleButton(element);
                        break;
                }

            } else if (/action-icon/g.test(element.className)) {

                _toggleIcon(element);
            }
        }
    }

    function _toggleHeader(element) {

        this.header = /disabled/g.test(element.className);

        let elementBlock = element.closestClass('component');

        if (this.header) {

            element.classList.remove('disabled');

            let header = document.createElement('div');
            header.className = 'card-header';
            header.setAttribute('contenteditable', true);
            header.textContent = Default.header;

            let elementBlockComponent = elementBlock.querySelectorAll('.card-info')[0];

            if (elementBlockComponent.children.length > 0) {

                elementBlockComponent.insertBefore(header, elementBlockComponent.firstChild);

            } else {

                elementBlockComponent.appendChild(header);
            }

        } else {

            element.classList.add('disabled');

            let elementHeader = elementBlock.querySelectorAll('.card-header:not(.small)')[0];

            if (elementHeader !== undefined) {

                elementHeader.parentNode.removeChild(elementHeader);
            }
        }

        _manageComponent(elementBlock);
    }

    function _toggleSubheader(element) {

        this.subheader = /disabled/g.test(element.className);

        let elementBlock = element.closestClass('component');

        if (this.subheader) {

            element.classList.remove('disabled');

            let subheader = document.createElement('div');
            subheader.className = 'card-header small';
            subheader.setAttribute('contenteditable', true);
            subheader.textContent = Default.subheader;

            let elementBlockComponent = elementBlock.querySelectorAll('.card-info')[0];

            if (elementBlockComponent !== undefined) {

                elementBlockComponent.appendChild(subheader);
            }

        } else {

            element.classList.add('disabled');

            let elementSubheader = elementBlock.querySelectorAll('.card-header.small')[0];

            if (elementSubheader !== undefined) {

                elementSubheader.parentNode.removeChild(elementSubheader);
            }
        }

        _manageComponent(elementBlock);

    }

    function _toggleParagraph(element) {

        this.paragraph = /disabled/g.test(element.className);

        let elementBlock = element.closestClass('component');

        if (this.paragraph) {

            element.classList.remove('disabled');

            let paragraph = document.createElement('p');
            paragraph.setAttribute('contenteditable', true);
            paragraph.textContent = Default.paragraph;

            let elementBlockComponent = elementBlock.querySelectorAll('.card-content')[0];

            if (elementBlockComponent !== undefined) {

                elementBlockComponent.appendChild(paragraph);
            }

        } else {

            element.classList.add('disabled');

            let elementParagraph = elementBlock.querySelectorAll('p')[0];

            if (elementParagraph !== undefined) {

                elementParagraph.parentNode.removeChild(elementParagraph);
            }
        }

        _manageComponent(elementBlock);
    }

    function _toggleButton(element) {

        this.button = /disabled/g.test(element.className);

        let elementBlock = element.closestClass('component');

        let elements = elementBlock.querySelectorAll('.card-button');

        if (!this.button && elements.length === 0) {

            element.classList.add('disabled');

            let elementButton = elements[0];

            if (elementButton !== undefined) {

                elementButton.parentNode.removeChild(elementButton);
            }

        } else {

            this.button = true;

            element.classList.remove('disabled');

            let elementBlockComponent = elementBlock.querySelectorAll('.card-control')[0];

            let elementButton = document.createElement('a');
            elementButton.className = 'card-button';
            elementButton.href = '#';
            elementButton.textContent = Default.button;

            elementBlockComponent.appendChild(elementButton);
        }

        _manageComponent(elementBlock);
    }

    function _toggleIcon(element) {

        let elementBlock = element.closestClass('component');

        let elementBlockComponent = elementBlock.querySelectorAll('.card-icon')[0];

        let elementIcon = elementBlockComponent.querySelectorAll('i')[0];

        if (elementIcon === undefined) {

            elementIcon = document.createElement('i');
            elementIcon.className = `mdi mdi-${element.dataset.value}`;

            elementBlockComponent.appendChild(elementIcon);

        } else if (element.value === '') {

            elementBlockComponent.removeChild(elementIcon);

        } else {

            elementIcon.className = `mdi mdi-${element.dataset.value}`;
        }
    }

    function _manageComponent(element) {

        if (!this.header && !this.subheader) {

            element.querySelectorAll('.card-info')[0].classList.add('d-none');

        } else {

            element.querySelectorAll('.card-info')[0].classList.remove('d-none');
        }

        if (!this.paragraph) {

            element.querySelectorAll('.card-content')[0].classList.add('d-none');

        } else {

            element.querySelectorAll('.card-content')[0].classList.remove('d-none');
        }

        if (!this.button) {

            element.querySelectorAll('.card-control')[0].classList.add('d-none');

        } else {

            element.querySelectorAll('.card-control')[0].classList.remove('d-none');
        }
    }

    /* ----------------------------------------------------------- */
    /* == helpers */
    /* ----------------------------------------------------------- */

    Element.prototype.appendToList = function (elements) {

        let element = this;

        [].forEach.call(elements, function (item, i) {

            element.appendNode('li', item.text, { 'class': 'action-icon', 'data-value': item.value });

        });
    };

    /* ----------------------------------------------------------- */
    /* == return */
    /* ----------------------------------------------------------- */

    return {
        init: EditorCard
    };

}));
