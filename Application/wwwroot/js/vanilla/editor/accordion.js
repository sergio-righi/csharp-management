
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.accordion = factory();
    }

}(this, function () {

    const Info = {
        name: 'accordion',
        version: '2.0'
    };

    const Default = {
        header: 'Lorem ipsum dolor sit amet',
        subheader: 'Lorem ipsum dolor sit amet',
        delete_menu: 'Lorem ipsum dolor sit amet, consectetur adipisicing elit',
        delete_submenu: 'Lorem ipsum dolor sit amet, consectetur adipisicing elit',
        paragraph: 'Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in volmover-cimatate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cmover-cimaidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.'
    };

    /* ----------------------------------------------------------- */
    /* == component */
    /* ----------------------------------------------------------- */

    function EditorAccordion(context) {

        this.menu = null;
        this.submenu = null;
        this.context = context;

        _bindEvent.call(this);

        return this;
    }

    EditorAccordion.prototype.getControl = function () {

        let configurationBlock = document.createElement('div');
        configurationBlock.className = 'configuration-block';

        configurationBlock.appendNode('button', '<i class="mdi mdi-plus"></i>', { 'type': 'button', 'name': 'accordion_add_menu', 'class': 'action-item' });
        configurationBlock.appendNode('button', '<i class="mdi mdi-minus"></i>', { 'type': 'button', 'name': 'accordion_delete_menu', 'class': 'action-item' });

        let configurationBlock1 = document.createElement('div');
        configurationBlock1.className = 'configuration-block';

        configurationBlock1.appendNode('button', '<i class="mdi mdi-plus"></i>', { 'type': 'button', 'name': 'accordion_add_paragraph', 'class': 'action-item disabled' });
        configurationBlock1.appendNode('button', '<i class="mdi mdi-minus"></i>', { 'type': 'button', 'name': 'accordion_delete_submenu', 'class': 'action-item disabled' });

        let configurationBlock2 = document.createElement('div');
        configurationBlock2.className = 'configuration-block';

        configurationBlock2.appendNode('button', '<i class="mdi mdi-chevron-up"></i>', { 'type': 'button', 'name': 'accordion_move_up', 'class': 'action-item' });
        configurationBlock2.appendNode('button', '<i class="mdi mdi-chevron-down"></i>', { 'type': 'button', 'name': 'accordion_move_down', 'class': 'action-item' });

        let configurationBlock3 = document.createElement('div');
        configurationBlock3.className = 'configuration-block';

        configurationBlock3.appendNode('button', '<i class="mdi mdi-settings"></i>', { 'type': 'button', 'name': 'style_set', 'class': 'action-item' });

        return configurationBlock.outerHTML + configurationBlock1.outerHTML + configurationBlock2.outerHTML + configurationBlock3.outerHTML;
    };

    EditorAccordion.prototype.getContent = function () {

        return `<div class="accordion" data-component="${Info.name}" data-version="${Info.name}" data-exclusive></div>`;
    };

    /* ----------------------------------------------------------- */
    /* == private methods */
    /* ----------------------------------------------------------- */

    function _build() {

    }

    function _bindEvent() {

        this._events = {
            actionClick: _handleActionClick.bind(this),
            contentClick: _handleContentClick.bind(this)
        };

        this.context.sectionBuild.addEventListener('click', this._events.actionClick);
        this.context.sectionBuild.addEventListener('click', this._events.contentClick);
    }

    function _unbindEvent() {

    }

    function _handleActionClick(e) {

        let element = e.target.closestNode('button');

        if (element !== null && /accordion_.*?/g.test(element.name)) {

            let elementBlock = element.closestClass('component');

            let elementBlockComponent = elementBlock.querySelectorAll('[data-component]')[0];

            switch (element.name) {
                case 'accordion_add_menu':

                    _addItem(elementBlockComponent, null, null);

                    break;
                case 'accordion_add_paragraph':

                    if (this.menu !== null) {

                        _addParagraph(this.menu.querySelectorAll('.accordion-content')[0]);
                    }

                    break;
                case 'accordion_delete_menu':

                    if (this.menu !== null) {

                        let confirmation = confirm(Default.delete_menu);

                        if (confirmation) {

                            element.classList.add('disabled');

                            this.menu.parentNode.removeChild(this.menu);

                            this.menu = null;
                        }
                    }

                    break;
                case 'accordion_delete_submenu':

                    if (this.submenu !== null) {

                        let confirmation = confirm(Default.delete_submenu);

                        if (confirmation) {

                            element.classList.add('disabled');

                            this.submenu.parentNode.removeChild(this.submenu);

                            this.submenu = null;
                        }
                    }

                    break;
                case 'accordion_move_up':
                case 'accordion_move_down':

                    let elementNode = null;

                    if (this.menu !== null && this.submenu === null) {

                        elementNode = this.menu;

                    } else if (this.submenu !== null) {

                        elementNode = this.submenu;
                    }

                    if (elementNode !== null) {

                        if (element.name === 'accordion_move_up') {

                            elementNode.parentNode.insertBefore(elementNode, elementNode.previousSibling);

                        } else {

                            let elementParent = elementNode.parentNode;

                            let elementNext = elementNode.nextSibling;

                            if (elementNext !== null) {

                                elementParent.insertBefore(elementNext, elementNode);

                            } else {

                                elementParent.insertBefore(elementNode, elementParent.firstChild);
                            }
                        }
                    }

                    break;
            }
        }
    }

    function _handleContentClick(e) {

        if (e.target.closestClass('accordion-item') !== null) {

            let element = e.target.closestNode('p') || e.target.closestClass('accordion-item');

            if (element !== null) {

                e.stopPropagation();

                let elementBlock = element.closestClass('component');

                if (element.nodeName === 'P') {

                    if (this.submenu !== null) {

                        this.submenu.classList.remove('selected');
                    }

                    if (this.submenu !== null && this.submenu === element) {

                        this.submenu = null;

                        elementBlock.querySelectorAll('button[name=accordion_delete_submenu]')[0].classList.add('disabled');

                    } else {

                        this.submenu = element;

                        this.submenu.classList.add('selected');

                        elementBlock.querySelectorAll('button[name=accordion_delete_submenu]')[0].classList.remove('disabled');
                    }

                } else if (element.className === 'accordion-item') {

                    if (this.menu !== null) {

                        this.menu.classList.remove('selected');
                    }

                    if (this.menu !== null && this.menu === element) {

                        this.menu = null;

                        elementBlock.querySelectorAll('button[name=accordion_delete_menu]')[0].classList.add('disabled');

                        elementBlock.querySelectorAll('button[name=accordion_add_paragraph]')[0].classList.add('disabled');

                    } else {

                        this.menu = element;

                        this.menu.classList.add('selected');

                        elementBlock.querySelectorAll('button[name=accordion_delete_menu]')[0].classList.remove('disabled');

                        elementBlock.querySelectorAll('button[name=accordion_add_paragraph]')[0].classList.remove('disabled');
                    }
                }
            }
        }
    }

    function _addItem(element, title, url) {

        let contentHTML = `<div class="accordion-item"${url !== null ? `data-url="${url}"` : ''}>` +
            '<div class="accordion-header arrow">' +
            `<div class="accordion-title" contenteditable="true">${title || Default.header}<small contenteditable="true">${Default.subheader}</small>` +
            '</div>' +
            '</div>' +
            '<div class="accordion-body">' +
            '<div class="accordion-content"></div>' +
            '</div>' +
            '</div>';

        element.appendHTML(contentHTML);
    };

    function _addParagraph(element) {

        let contentHTML = `<p contenteditable="true">${Default.paragraph}</p>`;

        element.appendHTML(contentHTML);
    };

    /* ----------------------------------------------------------- */
    /* == return */
    /* ----------------------------------------------------------- */

    return {
        init: EditorAccordion
    };

}));
