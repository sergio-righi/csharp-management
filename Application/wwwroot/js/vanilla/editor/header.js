
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.header = factory();
    }

}(this, function () {

    const Info = {
        name: 'header',
        version: '2.0'
    };

    const Default = {
        header: 'Lorem ipsum dolor sit amet'
    };

    /* ----------------------------------------------------------- */
    /* == component */
    /* ----------------------------------------------------------- */

    function EditorHeader(context) {

        this.context = context;

        _bindEvent.call(this);

        return this;
    }

    EditorHeader.prototype.getControl = function () {

        let configurationBlock = document.createElement('div');
        configurationBlock.className = 'configuration-block';

        configurationBlock.appendNode('button', 'Big', { 'type': 'button', 'name': 'header_big', 'class': 'action-item', 'value': 'h3' });
        configurationBlock.appendNode('button', 'Medium', { 'type': 'button', 'name': 'header_medium', 'class': 'action-item', 'value': 'h4' });
        configurationBlock.appendNode('button', 'Small', { 'type': 'button', 'name': 'header_small', 'class': 'action-item', 'value': 'h5' });

        let configurationBlock1 = document.createElement('div');
        configurationBlock1.className = 'configuration-block';

        configurationBlock1.appendNode('button', '<i class="mdi mdi-settings"></i>', { 'type': 'button', 'name': 'style_set', 'class': 'action-item' });

        return configurationBlock.outerHTML + configurationBlock1.outerHTML;
    };

    EditorHeader.prototype.getContent = function () {

        return `<h3 contenteditable="true" data-component="${Info.name}" data-version="${Info.version}">${Default.header}</h3>`;
    };

    /* ----------------------------------------------------------- */
    /* == private methods */
    /* ----------------------------------------------------------- */

    function _build() {

    }

    function _bindEvent() {

        this._events = {
            actionClick: _handleActionClick.bind(this)
        };

        this.context.sectionBuild.addEventListener('click', this._events.actionClick);
    }

    function _unbindEvent() {

    }

    function _handleActionClick(e) {

        let element = e.target.closestNode('button');

        if (element !== null && /header_.*?/g.test(element.name)) {

            e.stopPropagation();

            let elementBlockContent = element.closestClass('component').children[1];

            elementBlockContent.innerHTML = elementBlockContent.innerHTML.replace(/h[3-5]/g, element.value);
        }
    }

    /* ----------------------------------------------------------- */
    /* == return */
    /* ----------------------------------------------------------- */

    return {
        init: EditorHeader
    };

}));
