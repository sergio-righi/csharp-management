
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.image = factory();
    }

}(this, function () {

    const Info = {
        name: 'image',
        version: '2.0'
    };

    const Default = {
        url: null
    };

    const Form = {
        url: 'image_url',
        src: 'image_src',
        edit: 'image_edit',
        submit: 'submit_image'
    };

    /* ----------------------------------------------------------- */
    /* == component */
    /* ----------------------------------------------------------- */

    function EditorImage(context) {

        this.previous = null;
        this.context = context;

        _bindEvent.call(this);

        return this;
    }

    EditorImage.prototype.getControl = function () {

        let configurationBlock = document.createElement('div');
        configurationBlock.className = 'configuration-block';

        configurationBlock.appendNode('button', '<i class="mdi mdi-pencil"></i>', { 'name': Form.edit, 'class': 'action-item d-none' });

        return configurationBlock.outerHTML;
    };

    EditorImage.prototype.getContent = function () {

        return this.getForm(true);
    };

    EditorImage.prototype.getForm = function (create) {

        return '<form enctype="multipart/form-data">' +
            '<div class="container-fluid">' +
            '<div class="row">' +
            '<div class="col col-12">' +
            '<div class="form-group">' +
            '<label>Image</label>' +
            `<input type="file" name="${Form.src}[]" multiple="multiple" accept="image/*" />` +
            //'<div class="file">' +
            //`<input type="file" id="${Form.src}" name="${Form.src}[]" multiple="multiple" accept="image/*" data-text="{counter} item(s) selected(s)" />` +
            //`<label for="${Form.src}">Image</label>` +
            //'</div>' +
            '</div>' +
            '</div>' +
            '<div class="col col-12">' +
            '<div class="form-group">' +
            `<button type="button" name="${Form.submit}" class="button ${create ? 'button-add' : 'button-save'}">Image</button>` +
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

            element.classList.add('d-none');

            let elementBlockComponent = element.closestClass('component').querySelectorAll('[data-component]')[0];

            this.previous = elementBlockComponent.outerHTML;

            elementBlockComponent.outerHTML = this.getForm(false);
        }
    }

    function _handleSubmitClick(e) {

        let element = e.target.closestNode('button');

        if (element !== null && element.name === Form.submit) {

            let files = element.form[0].files;

            let elementBlock = element.closestClass('component');

            let elementBlockContent = elementBlock.children[1];

            if (this.previous !== null) {

                elementBlockContent.innerHTML = this.previous;

                this.previous = null;
            }

            if (files.length > 0) {

                const quantity = elementBlockContent.querySelectorAll('.item').length;

                let elementComponent = null;

                if (files.length + quantity > 1) {

                    let elementGallery = elementBlockContent.querySelectorAll('.gallery')[0];

                    if (elementGallery === undefined && quantity > 0) {

                        elementComponent = elementBlockContent.children[0];

                        elementComponent.classList.add('gallery');
                        elementComponent.classList.remove('image');

                    } else if (elementGallery !== undefined) {

                        elementComponent = elementGallery;

                    } else {

                        elementComponent = document.createElement('div');
                        elementComponent.className = 'gallery';
                        elementComponent.setAttribute('data-component', Info.name);
                        elementComponent.setAttribute('data-version', Info.version);
                    }

                } else {

                    elementComponent = document.createElement('div');
                    elementComponent.className = 'image';
                    elementComponent.setAttribute('data-component', Info.name);
                    elementComponent.setAttribute('data-version', Info.version);
                }

                [].forEach.call(files, function (item) {

                    _showPreview(elementComponent, item);

                });

                elementBlockContent.innerHTML = '';
                elementBlockContent.appendChild(elementComponent);

            } else {

                if (this.previous !== null) {

                    console.log('source is required');
                }
            }

            elementBlock.children[0].querySelectorAll('button')[0].classList.remove('d-none');
        }
    }

    function _showPreview(element, image) {

        let fileReader = new FileReader();

        fileReader.addEventListener('load', function () {

            let image = new Image();

            image.src = fileReader.result;

            image.onload = function () {

                let elementComponent = document.createElement('div');
                elementComponent.className = 'item';
                elementComponent.innerHTML = `<img src='${fileReader.result}' />`;

                element.appendChild(elementComponent);

                //_sendImage(elementComponent, image);
            };

        }, false);

        if (image) {

            fileReader.readAsDataURL(image);
        }
    }

    function _sendImage(element, image) {

        if (Default.url !== null) {

            let formData = new FormData();

            formData.append('file', image);

            $.ajax({

                type: 'POST',

                cache: false,

                data: formData,

                url: Default.url,

                processData: false,

                contentType: false,

                success: function (src) {

                    let elementImage = element.querySelectorAll('img')[0];

                    if (elementImage !== undefined) {

                        elementImage.setAttribute('src', src);
                    }
                },

                error: function () {

                    element.parentNode.removeChild(element);
                }

            });
        }
    }

    /* ----------------------------------------------------------- */
    /* == helpers */
    /* ----------------------------------------------------------- */

    String.prototype.isImageURL = function () {

        return (/\.(jpeg|jpg|gif|png)$/.test(this.toLowerCase()));
    };

    /* ----------------------------------------------------------- */
    /* == return */
    /* ----------------------------------------------------------- */

    return {
        init: EditorImage
    };

}));
