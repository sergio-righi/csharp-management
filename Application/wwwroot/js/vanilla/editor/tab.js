
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.tab = factory();
    }

}(this, function () {

    const Info = {
        name: 'tab',
        version: '2.0'
    };

    const Default = {
        header: 'Lorem ipsum dolor sit amet',
        delete_tab: 'Lorem ipsum dolor sit amet, consectetur adipisicing elit',
        paragraph: 'Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in volmover-cimatate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cmover-cimaidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.'
    };

    /* ----------------------------------------------------------- */
    /* == component */
    /* ----------------------------------------------------------- */

    function EditorTab(context) {

        this.tab = null;
        this.context = context;

        _bindEvent.call(this);

        return this;
    }

    EditorTab.prototype.getControl = function () {

        let configurationBlock = document.createElement('div');
        configurationBlock.className = 'configuration-block';

        configurationBlock.appendNode('button', '<i class="mdi mdi-plus"></i>', { 'type': 'button', 'name': 'tab_add_paragraph', 'class': 'action-item' });
        configurationBlock.appendNode('button', '<i class="mdi mdi-minus"></i>', { 'type': 'button', 'name': 'tab_remove', 'class': 'action-item disabled' });

        let configurationBlock1 = document.createElement('div');
        configurationBlock1.className = 'configuration-block';

        configurationBlock1.appendNode('button', '<i class="mdi mdi-chevron-left"></i>', { 'type': 'button', 'name': 'tab_move_left', 'class': 'action-item disabled' });
        configurationBlock1.appendNode('button', '<i class="mdi mdi-chevron-right"></i>', { 'type': 'button', 'name': 'tab_move_right', 'class': 'action-item disabled' });

        return configurationBlock.outerHTML + configurationBlock1.outerHTML;
    };

    EditorTab.prototype.getContent = function () {

        return `<div class="tab" data-component="${Info.name}" data-version="${Info.version}">` +
            '<div class="tab-header">' +
            '</div>' +
            '<div class="tab-content">' +
            '</div>' +
            '</div>';
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

        if (element !== null && /tab_.*?/g.test(element.name)) {

            let elementBlock = element.closestClass('component');

            switch (element.name) {
                case 'tab_add_paragraph':

                    _addTab(elementBlock, null, null, null);

                    break;
                case 'tab_remove':

                    if (this.tab !== null) {

                        let confirmation = confirm(Default.delete_tab);

                        if (confirmation) {

                            element.classList.add('disabled');

                            this.tab.parentNode.removeChild(this.tab);

                            this.tab = null;
                        }
                    }

                    break;
                case 'tab_move_left':
                case 'tab_move_right':

                    if (this.tab !== null) {

                        if (element.name === 'tab_move_left') {

                            this.tab.parentNode.insertBefore(this.tab, this.tab.previousSibling);

                        } else {

                            let elementParent = this.tab.parentNode;

                            let elementNext = this.tab.nextSibling;

                            if (elementNext !== null) {

                                elementParent.insertBefore(elementNext, this.tab);

                            } else {

                                elementParent.insertBefore(this.tab, elementParent.firstChild);
                            }
                        }
                    }

                    break;
            }
        }
    }

    function _handleContentClick(e) {

        let element = e.target.closestClass('tab-item');

        if (element !== null) {

            e.stopPropagation();

            let elementBlock = element.closestClass('component');

            if (this.tab !== null) {

                this.tab.classList.remove('active');

                this.tab.parentNode.nextSibling.children[this.tab.index()].classList.remove('active');
            }

            if (this.tab !== null && this.tab === element) {

                this.tab = null;

                elementBlock.querySelectorAll('button[name=tab_remove]')[0].classList.add('disabled');
                elementBlock.querySelectorAll('button[name=tab_move_left]')[0].classList.add('disabled');
                elementBlock.querySelectorAll('button[name=tab_move_right]')[0].classList.add('disabled');

            } else {

                this.tab = element;

                this.tab.classList.add('active');

                this.tab.parentNode.nextSibling.children[this.tab.index()].classList.add('active');

                elementBlock.querySelectorAll('button[name=tab_remove]')[0].classList.remove('disabled');
                elementBlock.querySelectorAll('button[name=tab_move_left]')[0].classList.remove('disabled');
                elementBlock.querySelectorAll('button[name=tab_move_right]')[0].classList.remove('disabled');
            }
        }
    }

    function _addTab(element, header, type, url) {

        element.querySelectorAll('.tab-header')[0].appendChild(_getHeader(header || Default.header, url));

        element.querySelectorAll('.tab-content')[0].appendChild(_getContent(type));
    }

    function _getHeader(header, url) {

        return document.createElementHTML(`<div class="tab-item" contenteditable="true"${url !== null ? `data-url="${url}"` : ''}>${header}</div>`);
    }

    function _getContent(type) {

        return document.createElementHTML(`<div class="tab-item">${type === null ? `<p contenteditable="true">${Default.paragraph}</p>` : ''}</div>`);
    }

    /* ----------------------------------------------------------- */
    /* == return */
    /* ----------------------------------------------------------- */

    return {
        init: EditorTab
    };

}));
