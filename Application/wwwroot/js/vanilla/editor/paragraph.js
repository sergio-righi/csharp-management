
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.paragraph = factory();
    }

}(this, function () {

    const Info = {
        name: 'paragraph',
        version: '2.0'
    };

    const Default = {
        paragraph: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla risus nulla, consequat sed tincidunt et, rutrum at tellus. Nullam ut quam nec orci tincidunt venenatis. Etiam eu metus enim. Duis eget pretium lectus, mollis ultrices nisi. Morbi gravida libero magna, vitae tempor turpis gravida ut. Integer viverra elementum enim at accumsan. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Pellentesque vitae malesuada quam.'
    };

    /* ----------------------------------------------------------- */
    /* == component */
    /* ----------------------------------------------------------- */

    function EditorParagraph(context) {

        this.context = context;

        return this;
    }

    EditorParagraph.prototype.getControl = function () {

        let configurationBlock = document.createElement('div');
        configurationBlock.className = 'configuration-block';

        configurationBlock.appendNode('button', '<i class="mdi mdi-format-bold"></i>', { 'type': 'button', 'name': 'paragraph_bold', 'class': 'action-item', 'onclick': 'document.execCommand("bold", false)' });
        configurationBlock.appendNode('button', '<i class="mdi mdi-format-italic"></i>', { 'type': 'button', 'name': 'paragraph_italic', 'class': 'action-item', 'onclick': 'document.execCommand("italic", false)' });
        configurationBlock.appendNode('button', '<i class="mdi mdi-format-underline"></i>', { 'type': 'button', 'name': 'paragraph_underline', 'class': 'action-item', 'onclick': 'document.execCommand("underline", false)' });

        let configurationBlock1 = document.createElement('div');
        configurationBlock1.className = 'configuration-block';

        configurationBlock1.appendNode('button', '<i class="mdi mdi-link-variant"></i>', { 'type': 'button', 'name': 'anchor_add', 'class': 'action-item' });
        configurationBlock1.appendNode('button', '<i class="mdi mdi-settings"></i>', { 'type': 'button', 'name': 'style_set', 'class': 'action-item' });

        return configurationBlock.outerHTML + configurationBlock1.outerHTML;
    };

    EditorParagraph.prototype.getContent = function () {

        return `<p contenteditable="true" data-component="${Info.name}" data-version="${Info.version}">${Default.paragraph}</p>`;
    };

    /* ----------------------------------------------------------- */
    /* == private methods */
    /* ----------------------------------------------------------- */

    function _build() {

    }

    function _bindEvent() {

        this._events = {
        };
    }

    function _unbindEvent() {

    }

    /* ----------------------------------------------------------- */
    /* == return */
    /* ----------------------------------------------------------- */

    return {
        init: EditorParagraph
    };

}));
