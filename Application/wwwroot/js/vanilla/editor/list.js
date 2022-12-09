
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.list = factory();
    }

}(this, function () {

    const Info = {
        name: 'list',
        version: '2.0'
    };

    const Key = {
        tab: 9,
        enter: 13,
        backspace: 8
    };

    /* ----------------------------------------------------------- */
    /* == component */
    /* ----------------------------------------------------------- */

    function EditorList(context) {

        this.context = context;

        _bindEvent.call(this);

        return this;
    }

    EditorList.prototype.getControl = function () {

        let configurationBlock = document.createElement('div');
        configurationBlock.className = 'configuration-block';

        configurationBlock.appendNode('button', '<i class="mdi mdi-format-list-bulleted"></i>', { 'type': 'button', 'name': 'list_unordered', 'class': 'action-item', 'value': 'ul' });
        configurationBlock.appendNode('button', '<i class="mdi mdi-format-list-numbered"></i>', { 'type': 'button', 'name': 'list_ordered', 'class': 'action-item', 'value': 'ol' });

        let configurationBlock1 = document.createElement('div');
        configurationBlock1.className = 'configuration-block';

        configurationBlock.appendNode('button', '<i class="mdi mdi-format-indent-increase"></i>', { 'type': 'button', 'name': 'list_indent', 'class': 'action-item' });
        configurationBlock.appendNode('button', '<i class="mdi mdi-format-indent-decrease"></i>', { 'type': 'button', 'name': 'list_outdent', 'class': 'action-item' });

        let configurationBlock2 = document.createElement('div');
        configurationBlock2.className = 'configuration-block';

        configurationBlock2.appendNode('button', '<i class="mdi mdi-link-variant"></i>', { 'type': 'button', 'name': 'add_link', 'class': 'action-item' });
        configurationBlock2.appendNode('button', '<i class="mdi mdi-settings"></i>', { 'type': 'button', 'name': 'style_set', 'class': 'action-item' });

        return configurationBlock.outerHTML + configurationBlock1.outerHTML + configurationBlock2.outerHTML;
    };

    EditorList.prototype.getContent = function () {

        return `<ul contenteditable="true" data-component="${Info.name}" data-version="${Info.version}"><li>&nbsp;</li></ul>`;
    };

    /* ----------------------------------------------------------- */
    /* == private methods */
    /* ----------------------------------------------------------- */

    function _bindEvent() {

        this._events = {
            actionClick: _handleActionClick.bind(this),
            listKeydown: _handleListKeydown.bind(this)
        };

        this.context.sectionBuild.addEventListener('click', this._events.actionClick);
        this.context.sectionBuild.addEventListener('keydown', this._events.listKeydown);
    }

    function _unbindEvent() {

    }

    function _handleActionClick(e) {

        let element = e.target.closestNode('button');

        if (element !== null && /list_.*?/g.test(element.name)) {

            e.preventDefault();
            e.stopPropagation();

            let elementBlockComponent = element.closestClass('component').querySelectorAll('[data-component]')[0];

            switch (element.name) {
                case 'list_ordered':
                case 'list_unordered':

                    if (element.value === 'ul') {

                        [].forEach.call(elementBlockComponent.children, function (item) {

                            item.removeAttribute('data-level');

                        });

                    } else {

                        _buildOrderedList(elementBlockComponent, null);
                    }

                    elementBlockComponent.outerHTML = elementBlockComponent.outerHTML.replace(/ul|ol/g, element.value);

                    break;
                case 'list_indent':
                    document.execCommand('indent', false, null);
                    break;
                case 'list_outdent':
                    document.execCommand('outdent', false, null);
                    break;
            }
        }
    }

    function _handleListKeydown(e) {

        let element = e.target.closestNode('ul') || e.target.closestNode('ol');

        if (element !== null) {

            e.stopPropagation();

            const keyCode = e.keyCode || e.charCode;

            if (keyCode === Key.tab && e.shiftKey) {

                e.preventDefault();

                document.execCommand('outdent', false, null);

            } else if (keyCode === Key.tab) {

                e.preventDefault();

                document.execCommand('indent', false, null);

            } else if (keyCode === Key.backspace) {

                if (element.children.length === 1) {

                    e.preventDefault();
                }
            }

            if (element.nodeName === 'OL') {

                _buildOrderedList(element, null);
            }
        }
    }

    function _buildOrderedList(element, level) {

        let counter = 0;

        [].forEach.call(element.children, function (item, i) {

            if (item.nodeName === 'LI') counter++;

            if (item.children.length > 0) {

                let level = item.previousElementSibling.dataset.level || null;

                _buildOrderedList(item, level);

            } else {

                if (level === null) {

                    item.setAttribute('data-level', counter);

                } else {

                    item.setAttribute('data-level', `${level}.${counter}`);
                }
            }

        });
    }

    /* ----------------------------------------------------------- */
    /* == return */
    /* ----------------------------------------------------------- */

    return {
        init: EditorList
    };

}));
