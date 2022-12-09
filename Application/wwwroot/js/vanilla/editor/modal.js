
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.modal = factory();
    }

}(this, function () {

    const Info = {
        name: 'modal',
        version: '2.0'
    };

    const Default = {
        link: 'Lorem ipsum dolor sit amet'
    };

    const Key = {
        esc: 27,
        enter: 13
    };

    const Anchor = {
        href: 'anchor_url',
        text: 'anchor_text',
        target: 'anchor_target',
        submit: 'submit_anchor',
        delete: 'delete_anchor'
    };

    const Image = {
        url: 'image_url',
        order: 'image_order',
        subtitle: 'image_subtitle',
        submit: 'submit_image',
        delete: 'delete_image'
    };

    const Style = {
        clean: 'clean_style',
        submit: 'submit_style',
        marginTop: 'style_margin_top',
        marginBottom: 'style_margin_bottom'
    };

    /* ----------------------------------------------------------- */
    /* == component */
    /* ----------------------------------------------------------- */

    let isBusy = false;

    function EditorModal(context) {

        this.level = -1;
        this.backward = 1;
        this.hierarchy = [];

        this.element = null;
        this.context = context;

        _build.call(this);

        _bindEvent.call(this);

        return this;
    }

    EditorModal.prototype._busy = function (state) {

        isBusy = state;
    };

    EditorModal.prototype._isBusy = function () {

        return isBusy;
    };

    EditorModal.prototype.isOpen = function () {

        return !!this.modal.classList.contains('active');
    };

    EditorModal.prototype.open = function (index) {

        if (this._isBusy()) return;

        this._busy(true);

        document.body.classList.add('o-hidden');

        this.modal.classList.add('active');

        this._busy(false);

        return this;
    };

    EditorModal.prototype.close = function () {

        if (this._isBusy()) return;

        this._busy(true);

        document.body.classList.remove('o-hidden');

        this.modal.classList.remove('active');

        this._busy(false);
    };

    EditorModal.prototype.createURL = function (element) {

        let elementBlock = element.closestClass('component');

        let elementBlockComponent = elementBlock.querySelectorAll('[data-component]')[0];

        let elementComponent = document.createNode('a', window.getSelection().toString() || Default.link, { 'href': '#' });

        if (elementBlockComponent.dataset.component !== 'table') {

            elementBlock.focus();

            elementComponent = elementComponent.outerHTML.insertInto();

        } else {

            let elementCursor = _getCursorElement();

            elementCursor.focus();

            elementCursor.outerHTML = elementComponent.outerHTML;
        }

        this.element = elementComponent;
    };

    /* ----------------------------------------------------------- */
    /* == private methods */
    /* ----------------------------------------------------------- */

    function _build() {

        this.modal = document.createElement('div');
        this.modal.className = 'modal';
        this.modal.setAttribute('data-component', Info.name);
        this.modal.setAttribute('data-version', Info.version);

        this.popup = document.createElement('div');
        this.popup.className = 'modal-popup';

        this.closeButton = document.createElement('button');
        this.closeButton.setAttribute('type', 'button');
        this.closeButton.className = 'modal-close';

        this.content = document.createElement('content');
        this.content.className = 'modal-content';

        this.popup.appendChild(this.content);

        this.modal.appendChild(this.closeButton);
        this.modal.appendChild(this.popup);

        this.context.editor.appendChild(this.modal);

        return this;
    }

    function _bindEvent() {

        this._events = {
            modalClick: _handleModalClick.bind(this),
            closeClick: _handleClickClose.bind(this),
            anchorClick: _handleAnchorClick.bind(this),
            actionClick: _handleActionClick.bind(this),
            overlayClick: _handleClickOutside.bind(this),
            keyboardPress: _handleKeyboardNav.bind(this)
        };

        this.modal.addEventListener('mousedown', this._events.overlayClick);

        document.addEventListener('keydown', this._events.keyboardPress);

        this.closeButton.addEventListener('click', this._events.closeClick);

        this.modal.addEventListener('click', this._events.modalClick);
        this.context.sectionBuild.addEventListener('click', this._events.anchorClick);
        this.context.sectionBuild.addEventListener('click', this._events.actionClick);
    }

    function _unbindEvent() {
    }

    function _handleKeyboardNav(e) {

        if (e.which === Key.esc && this.isOpen()) {

            this.close();
        }
    }

    function _handleClickOutside(e) {

        if (e.target.closestClass('modal-popup') === null && e.clientX < this.modal.clientWidth) {

            this.close();
        }
    }

    function _handleClickClose(e) {

        this.close();
    }

    function _handleActionClick(e) {

        let element = e.target.closestNode('button');

        if (element !== null) {

            if (/anchor_.*?/g.test(element.name)) {

                this.createURL(element);

            } else if (/style_.*?/g.test(element.name)) {

                let elementBlockComponent = element.closestClass('component').querySelectorAll('[data-component]')[0];

                this.element = elementBlockComponent;

                let computedStyle = elementBlockComponent.currentStyle || window.getComputedStyle(elementBlockComponent);

                this.content.innerHTML = _getStyleContent(computedStyle.marginTop.match(/\d+/) || '', computedStyle.marginBottom.match(/\d+/) || '');

                this.open(null);
            }
        }
    }

    function _handleAnchorClick(e) {

        let element = e.target.closestNode('img') || e.target.closestNode('a');

        if (element !== null) {

            e.preventDefault();

            if (element.nodeName === 'A') {

                this.content.innerHTML = _getAnchorContent();

                this.element = element;

                this.content.querySelectorAll(`input[name=${Anchor.href}]`)[0].value = element.attributes[0].textContent;

                this.content.querySelectorAll(`select[name=${Anchor.target}]`)[0].value = element.target;

                this.content.querySelectorAll(`input[name=${Anchor.text}]`)[0].value = element.textContent;

                this.open(0);

            } else if (element.nodeName === 'IMG') {

                this.element = element;

                this.content.innerHTML = _getImageContent(element.parentNode.href);

                this.open(null);
            }
        }
    }

    function _handleModalClick(e) {

        let element = e.target.closestNode('button');

        if (element !== null) {
            
            switch (element.name) {
                case Anchor.submit:

                    if (element.form[1].value.isURL()) {

                        this.element.textContent = element.form[0].value;

                        this.element.setAttribute('target', element.form[2].value);

                        this.element.setAttribute('href', element.form[1].value);

                        this.element = null;

                        this.close();

                    } else {

                        console.log('is not a valid url');
                    }

                    break;
                case Anchor.delete:

                    this.element.parentNode.removeChild(this.element);

                    this.close();

                    break;
                case Image.submit:

                    let url = element.form[0].value;

                    if (url !== '' && url.isURL()) {

                        if (this.element.nodeName === 'A') {

                            this.element.setAttribute('href', url);

                        } else {

                            this.element.outerHTML = document.createNode('a', this.element.outerHTML, { 'href': url }).outerHTML;
                        }

                    } else if (url === '') {

                        if (elementNode.nodeName === 'A') {

                            this.element.outerHTML = this.element.querySelectorAll('img')[0].outerHTML;
                        }
                    }

                    this.close();

                    break;
                case Image.delete:

                    let elementBlock = this.element.closestClass('component');

                    if (elementBlock.children.length - 1 === 1) {

                        elementBlock.classList.add('image');
                        elementBlock.classList.remove('gallery');
                    }

                    let elementComponent = this.element.closestClass('item');

                    if (elementComponent !== null) {

                        elementComponent.parentNode.removeChild(elementComponent);
                    }

                    this.close();

                    break;
                case Style.submit:

                    this.element.style.marginTop = `${element.form[0].value}px`;
                    this.element.style.marginBottom = `${element.form[1].value}px`;

                    this.close();

                    break;
                case Style.clean:

                    this.element.removeAttribute('style');

                    this.close();

                    break;
            }
        }
    }

    function _getCursorElement() {

        let selection;

        if (window.getSelection) {

            selection = window.getSelection();

        } else if (document.selection && document.selection.type !== 'Control') {

            selection = document.selection;
        }

        return selection.anchorNode;
    }

    function _getAnchorContent(create) {

        return '<form>' +
            '<div class="container-fluid">' +
            '<div class="row">' +
            '<div class="col col-12 col-sm-4">' +
            '<div class="form-group">' +
            '<label>Text</label>' +
            `<input type="text" name="${Anchor.text}" />` +
            '</div>' +
            '</div>' +
            '<div class="col col-12 col-sm-4">' +
            '<div class="form-group">' +
            '<label>URL</label>' +
            `<input type="text" name="${Anchor.href}" />` +
            '</div>' +
            '</div>' +
            '<div class="col col-12 col-sm-4">' +
            '<div class="form-group">' +
            '<label>Open link in</label>' +
            `<select name="${Anchor.target}">` +
            '<option value="_self">current window</option>' +
            '<option value="_blank">new window</option>' +
            '</select>' +
            '</div>' +
            '</div>' +
            '<div class="col col-12">' +
            '<div class="form-group">' +
            `<button type="button" name="${Anchor.submit}" class="button ${create ? 'button-add' : 'button-save'}">Update</button>` +
            `<button type="button" class="button button-delete" name="${Anchor.delete}">Remove</button>` +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</form>';
    }

    function _getImageContent(url) {

        return '<form>' +
            '<div class="container-fluid">' +
            '<div class="row">' +
            '<div class="col col-12">' +
            '<div class="form-group">' +
            '<label>URL</label>' +
            `<input type="url" name="${Image.image_url}" value="${url || ''}" />` +
            '</div>' +
            '</div>' +
            '<div class="col col-12">' +
            '<div class="form-group">' +
            `<button type="button" class="button button-save" name="${Image.submit}">Save</button>` +
            `<button type="button" class="button button-delete" name="${Image.delete}">Remove</button>` +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</form>';
    }

    function _getStyleContent(marginTop, marginBottom) {

        return '<form>' +
            '<div class="container-fluid">' +
            '<div class="row">' +
            '<div class="col col-6">' +
            '<div class="form-group">' +
            '<label>Margin Top</label>' +
            `<input type="text" name="${Style.marginTop}" value="${marginTop || ''}" />` +
            '</div>' +
            '</div>' +
            '<div class="col col-6">' +
            '<div class="form-group">' +
            '<label>Margin Bottom</label>' +
            `<input type="text" name="${Style.marginBottom}" value="${marginBottom || ''}" />` +
            '</div>' +
            '</div>' +
            '<div class="col col-12">' +
            '<div class="form-group">' +
            `<button type="button" class="button button-save" name="${Style.submit}">Save</button>` +
            `<button type="button" class="button button-clean" name="${Style.clean}">Clean</button>` +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</form>';
    }

    /* ----------------------------------------------------------- */
    /* == helpers */
    /* ----------------------------------------------------------- */

    String.prototype.isURL = function () {

        return /^(https?|s?ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i.test(this);
    };

    /* ----------------------------------------------------------- */
    /* == return */
    /* ----------------------------------------------------------- */

    return {
        init: EditorModal
    };

}));
