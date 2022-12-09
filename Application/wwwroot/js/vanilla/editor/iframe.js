
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.iframe = factory();
    }

}(this, function () {

    const Info = {
        name: 'iframe',
        version: '2.0'
    };

    const Ratio = {
        widescreen: 0,
        square: 1,
        standard: 2,
        ultrawide: 3
    };

    const Form = {
        url: 'iframe_url',
        edit: 'iframe_edit',
        frame: 'iframe_frame',
        ratio: 'iframe_ratio',
        submit: 'submit_iframe'
    };

    /* ----------------------------------------------------------- */
    /* == component */
    /* ----------------------------------------------------------- */

    function EditorIframe(context) {

        this.context = context;

        _bindEvent.call(this);

        return this;
    }

    EditorIframe.prototype.getControl = function () {

        let configurationBlock = document.createElement('div');
        configurationBlock.className = 'configuration-block d-none';

        configurationBlock.appendNode('button', '<i class="mdi mdi-pencil"></i> Editar', { 'type': 'button', 'name': Form.edit, 'class': 'action-item' });
        configurationBlock.appendNode('button', '<i class="mdi mdi-settings"></i>', { 'type': 'button', 'name': 'style_set', 'class': 'action-item' });

        return configurationBlock.outerHTML;
    };

    EditorIframe.prototype.getContent = function () {

        return this.getForm(null, null, null, true);
    };

    EditorIframe.prototype.getForm = function (url, ratio, frame, create) {

        return '<form>' +
            '<div class="container-fluid">' +
            '<div class="row">' +
            '<div class="col col-12 col-sm-6">' +
            '<div class="form-group">' +
            '<label>URL</label>' +
            `<input type="text" name="${Form.url}" value="${url === null ? '' : url}" autofocus />` +
            '</div>' +
            '</div>' +
            '<div class="col col-12 col-sm-3">' +
            '<div class="form-group">' +
            '<label>Proporção</label>' +
            `<select name="${Form.ratio}">` +
            `<option value="${Ratio.widescreen}"${ratio === Ratio.widescreen ? ' selected' : ''}>Widescreen (16:9)</option>` +
            `<option value="${Ratio.square}"${ratio === Ratio.square ? ' selected' : ''}>Quadrado (1:1)</option>` +
            `<option value="${Ratio.standard}"${ratio === Ratio.standard ? ' selected' : ''}>Retrato (4:3)</option>` +
            `<option value="${Ratio.ultrawide}"${ratio === Ratio.ultrawide ? ' selected' : ''}>Ultrawide (21:9)</option>` +
            '</select>' +
            '</div>' +
            '</div>' +
            '<div class="col col-12 col-sm-3">' +
            '<div class="form-group">' +
            '<label>Moldura</label>' +
            `<select name="${Form.frame}">` +
            `<option value="0"${frame === 0 ? ' selected' : ''}>Não</option>` +
            `<option value="1"${frame === 1 ? ' selected' : ''}>Sim</option>` +
            '</select>' +
            '</div>' +
            '</div>' +
            '<div class="col col-12">' +
            '<div class="form-group">' +
            `<button type="button" name="${Form.submit}" class="button ${create ? 'button-add' : 'button-save'}">Video</button>` +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</form>';
    };

    /* ----------------------------------------------------------- */
    /* == private methods */
    /* ----------------------------------------------------------- */

    function _build() {

    }

    function _bindEvent() {

        this._events = {
            actionClick: _handleActionClick.bind(this),
            submitClick: _handleSubmitClick.bind(this)
        };

        this.context.sectionBuild.addEventListener('click', this._events.actionClick);
        this.context.sectionBuild.addEventListener('click', this._events.submitClick);
    }

    function _unbindEvent() {

    }

    function _handleActionClick(e) {

        let element = e.target.closestNode('button');

        if (element !== null && element.name === Form.edit) {
            
            element.parentNode.classList.add('d-none');

            let elementBlockComponent = element.closestClass('component').querySelectorAll('[data-component]')[0];

            elementBlockComponent.outerHTML = this.getForm(elementBlockComponent.dataset.url, parseInt(elementBlockComponent.dataset.ratio, 10), parseInt(elementBlockComponent.dataset.frame, 10), false);
        }
    }

    function _handleSubmitClick(e) {

        let element = e.target.closestNode('button');

        if (element !== null && element.name === Form.submit) {

            let url = element.form[0].value;

            let ratio = parseInt(element.form[1].selectedIndex, 10);

            let frame = parseInt(element.form[2].selectedIndex, 10);

            if (url) {

                let elementComponent = document.createElement('div');
                elementComponent.className = `embed-responsive ${_getRatioCls(ratio)}`;
                elementComponent.setAttribute('data-url', url);
                elementComponent.setAttribute('data-ratio', ratio);
                elementComponent.setAttribute('data-frame', frame);
                elementComponent.setAttribute('data-component', Info.name);
                elementComponent.setAttribute('data-version', Info.version);

                if (frame === 1) {

                    elementComponent.classList.add('frame');

                } else {

                    elementComponent.classList.remove('frame');
                }

                let elementEmbed = document.createElement('iframe');
                elementEmbed.src = _getURL(url);
                elementEmbed.frameBorder = 0;
                elementEmbed.allowFullscreen = true;

                elementComponent.appendChild(elementEmbed);

                let elementBlock = e.target.closestClass('component');

                elementBlock.children[0].querySelectorAll('button')[0].parentNode.classList.remove('d-none');

                elementBlock.children[1].innerHTML = elementComponent.outerHTML;

            } else {

                console.log('url is required');
            }
        }
    }

    function _getRatioCls(ratio) {

        return ratio === Ratio.widescreen ? 'widescreen' : ratio === Ratio.square ? 'square' : ratio === Ratio.standard ? 'standard' : ratio === Ratio.ultrawide ? 'ultrawide' : '';
    }

    function _getURL(origin) {

        var vimeo = /(?:http?s?:\/\/)?(?:www\.)?(?:vimeo\.com)\/?(.+)/g;
        var youtube = /(?:http?s?:\/\/)?(?:www\.)?(?:youtube\.com|youtu\.be)\/(?:watch\?v=)?(.+)/g;

        if (vimeo.test(origin)) {

            return origin.replace(vimeo, '//player.vimeo.com/video/$1');
        }

        if (youtube.test(origin)) {

            return origin.replace(youtube, 'https://www.youtube.com/embed/$1?rel=0&amp;controls=1&amp;showinfo=0;');
        }
    }

    /* ----------------------------------------------------------- */
    /* == return */
    /* ----------------------------------------------------------- */

    return {
        init: EditorIframe
    };

}));
