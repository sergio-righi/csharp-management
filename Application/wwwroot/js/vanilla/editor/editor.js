
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.editor = factory();
    }

}(this, function () {

    const Sidebar = {

        structure: [

            ['Bloco Principal', initStructure, 'application']
        ],
        component: [

            ['Título', initHeader, 'format-title', 'header'],
            ['Parágrafo', initParagraph, 'parking', 'paragraph'],
            ['Imagem', initImage, 'image', 'image'],
            ['Vídeo', initIframe, 'play-circle', 'iframe'],
            ['Tabela', initTable, 'table', 'table'],
            ['Lista', initList, 'format-list-bulleted', 'list'],
            ['Linha Horizontal', initHorizontalRule, 'minus', 'horizontal_rule'],
            ['Aba', initTab, 'tab', 'aba'],
            ['Sanfona', initAccordion, 'view-stream', 'accordion'],
            ['Cartão', initCard, 'crop-landscape', 'card']
        ]
    };

    /* ----------------------------------------------------------- */
    /* == component */
    /* ----------------------------------------------------------- */

    function Editor(element) {

        this.reference = [];

        this.sidebar = null;
        this.toggleButton = null;
        this.sectionBuild = null;
        this.sectionPreview = null;

        this.selectedBlock = -1;
        this.selectedColumn = -1;
        this.transferComponent = null;

        this.element = element;

        this.init();
    }

    Editor.prototype.init = function () {

        _build.call(this);

        _buildComponent.call(this);

        _buildSidebar.call(this);

        _bindEvent.call(this);

        new modal.init(this);

        return this;
    };

    Editor.prototype.destroy = function () {

        if (this.element === null) return;

        _unbindEvent.call(this);

        this.element.parentNode.removeChild(this.element);

        this.element = null;
    };

    Editor.prototype.setActiveBlock = function () {

        let structures = this.sectionBuild.querySelectorAll('.structure');

        let selectedStructure = this.sectionBuild.querySelectorAll('.structure.active')[0];

        if (selectedStructure !== undefined) {
            selectedStructure.classList.remove('active');
        }

        let structure = structures[this.selectedBlock];

        if (structure !== undefined) {
            structure.classList.add('active');
        }
    };

    Editor.prototype.getCursorElement = function () {

        let element;

        if (window.getSelection) {

            element = window.getSelection();

        } else if (document.selection && document.selection.type !== 'Control') {

            element = document.selection;
        }

        return element.anchorNode;
    };

    /* ----------------------------------------------------------- */
    /* == private methods */
    /* ----------------------------------------------------------- */

    function _build() {

        this.element.classList.add('d-none');

        this.editor = document.createElement('section');
        this.editor.className = 'editor';

        this.element.parentNode.insertBefore(this.editor, this.element.nextSibling);
    }

    function _buildSidebar() {

        this.sidebar = document.createElement('section');
        this.sidebar.className = 'sidebar';

        this.toggleButton = document.createElement('button');
        this.toggleButton.setAttribute('type', 'button');
        this.toggleButton.className = 'toggle-button';

        let contentHTML = [];

        let structures = Sidebar.structure;

        for (let i = 0; i < structures.length; i++) {

            this.reference.push(structures[i][1](this));

            contentHTML.push(
                '<div class="structure-container active">' +
                '<div class="structure-icon">' +
                `<i class="mdi mdi-${structures[i][2]}"></i>` +
                '</div>' +
                '<div class="structure-content">' +
                '<div class="col col-12 active"></div>' +
                '</div>' +
                `<div class="structure-control">${this.reference[i].getControl()}</div>` +
                '</div>');
        }

        let components = Sidebar.component;

        for (let j = 0; j < components.length; j++) {

            this.reference.push(components[j][1](this));

            contentHTML.push(
                `<div class="item-container" data-component="${components[j][3]}">` +
                '<div class="item-icon">' +
                `<i class="mdi mdi-${components[j][2]}"></i>` +
                '</div>' +
                '<div class="item-content">' +
                (typeof this.reference[j + structures.length] !== 'string' ? this.reference[j + structures.length].getContent() : this.reference[j + structures.length]) +
                '</div>' +
                '<div class="item-control">' +
                (typeof this.reference[j + structures.length] !== 'string' ? this.reference[j + structures.length].getControl() : '') +
                '</div>' +
                '</div>');
        }

        let sidebarContent = document.createElement('div');
        sidebarContent.className = 'sidebar-items';
        sidebarContent.innerHTML = contentHTML.join('');

        sidebarContent.insertBefore(this.toggleButton, sidebarContent.childNodes[0] || null);

        this.sidebar.append(sidebarContent);

        this.editor.insertBefore(this.sidebar, this.editor.childNodes[0] || null);
    }

    function _buildComponent() {

        let mainContainer = document.createElement('section');
        mainContainer.className = 'main';

        this.sectionBuild = document.createElement('section');
        this.sectionBuild.className = 'section-build active';

        this.sectionPreview = document.createElement('section');
        this.sectionPreview.className = 'section-preview';

        mainContainer.appendChild(this.sectionBuild);
        mainContainer.appendChild(this.sectionPreview);

        this.editor.appendChild(mainContainer);
    }

    function _rebuild() {

        let self = this;

        let contentHTML = [];

        [].forEach.call(this.editor.children, function (structure, i) {

            let columnHTML = [];

            [].forEach.call(structure.querySelectorAll('.col'), function (column, i) {

                columnHTML.push(`<div class="${column.className}">`);

                [].forEach.call(structure.querySelectorAll('[data-component]'), function (component, k) {

                    let reference = component.dataset.component !== undefined ? self.reference.getReference(component.dataset.component) : null;

                    let control = reference !== null && typeof reference !== 'string' ? reference.getControl() : reference;

                    let componentHTML = component.outerHTML.replace(/data-editable=\\"true\\"/g, 'contenteditable="true"');

                    if (/d-none/g.test(control)) {

                        control = control.replace(/d-none/g, '');
                    }

                    if (/d-none/g.test(componentHTML)) {

                        columnHTML.push(self.reference.getReference('structure').getComponent('component', component.dataset.component, componentHTML.replace(/d-none/g, '').outerHTML, control, true));

                    } else {

                        columnHTML.push(self.reference.getReference('structure').getComponent('component', component.dataset.component, componentHTML.replace(/d-none/g, '').outerHTML, control, false));
                    }

                });

                columnHTML.push('</div>');

            });

            contentHTML.push(self.reference.getReference('structure').getComponent('component', null, columnHTML.join(''), self.reference.getReference('structure').getControl(), false));

        });

        this.sectionBuild.innerHTML = contentHTML.join('');
    }

    function _compile() {

        let contentHTML = [];

        let structures = this.sectionBuild.querySelectorAll('.structure');

        [].forEach.call(structures, function (structure, i) {

            contentHTML.push('<div data-structure="true"><div class="row">');

            [].forEach.call(structure.querySelectorAll('.col'), function (column, j) {

                let structureHTML = [`<div class="${column.className}">`];

                [].forEach.call(column.querySelectorAll('[data-component]'), function (component, k) {

                    let elementBlock = component.closestClass('component');

                    let elementNode = document.createNode(component.nodeName, component.innerHTML, component.getProperty());

                    if (typeof elementBlock.dataset.hidden !== 'undefined') {

                        elementNode.classList.add('d-none');
                    }

                    let componentHTML = elementNode.outerHTML.replace(/contenteditable=\\"true\\"/g, 'data-editable="true"');

                    structureHTML.push(componentHTML.replace(/selected/g, ''));

                });

                structureHTML.push('</div>');

                contentHTML.push(structureHTML.join(''));

            });

            contentHTML.push('</div></div>');

        });

        return contentHTML.join('');
    }

    function _bindEvent() {

        this._events = {
            toggleClick: _handleToggleClick.bind(this),
            sidebarClick: _handleSidebarClick.bind(this),
            controlClick: _handleControlClick.bind(this)
        };

        this.sidebar.addEventListener('mouseup', this._events.sidebarClick);
        this.toggleButton.addEventListener('click', this._events.toggleClick);
        this.sectionBuild.addEventListener('click', this._events.controlClick);
    }

    function _unbindEvent() {

    }

    function _handleToggleClick(e) {

        if (this.sectionBuild.querySelectorAll('form').length === 0) {

            e.target.classList.toggle('active');

            if (this.sectionBuild.classList.contains('active')) {

                this.sectionBuild.classList.remove('active');

                this.sectionPreview.classList.add('active');

                this.sectionPreview.innerHTML = _compile.call(this);

            } else if (this.sectionPreview.classList.contains('active')) {

                this.sectionPreview.classList.remove('active');

                this.sectionBuild.classList.add('active');
            }

            [].forEach.call(this.sidebar.querySelectorAll('.item-container, .structure-container'), function (item) {

                item.classList.toggle('active');

            });

        } else {

            console.log('There are uncreated elements');
        }
    }

    function _handleSidebarClick(e) {

        if (this.sectionBuild.classList.contains('active')) {

            if (e.target.closestClass('structure-container') !== null) {

                let element = e.target.closestClass('structure-container');

                let contentHTML = this.reference.getReference('structure').getComponent('structure', null, element.querySelectorAll('.structure-control')[0].innerHTML, element.querySelectorAll('.structure-content')[0].innerHTML, false);

                if (this.sectionBuild.querySelectorAll('.structure').length === 0) {

                    this.sectionBuild.innerHTML = contentHTML;

                    this.selectedBlock = 0;

                    this.selectedColumn = 0;

                    this.setActiveBlock();

                } else {

                    this.transferComponent = contentHTML;

                    let selectedItem = this.sidebar.querySelectorAll('selected')[0];

                    if (selectedItem !== undefined) {
                        selectedItem.classList.remove('selected');
                    }

                    element.classList.add('selected');
                }

                let items = this.sidebar.querySelectorAll('.item-container');

                [].forEach.call(items, function (el) {

                    el.classList.add('active');

                });

            } else if (e.target.closestClass('item-container') !== null) {

                let element = e.target.closestClass('item-container');

                if (!element.classList.contains('selected')) {

                    let contentHTML = this.reference.getReference('structure').getComponent('component', element.dataset.component, element.querySelectorAll('.item-control')[0].innerHTML, element.querySelectorAll('.item-content')[0].innerHTML, false);

                    let structure = this.sectionBuild.querySelectorAll('.structure')[this.selectedBlock];

                    if (structure !== undefined) {

                        let column = structure.querySelectorAll('.col')[this.selectedColumn];

                        if (column !== undefined) {

                            if (column.children.length === 0) {

                                column.innerHTML = contentHTML;

                                /// highlight animation

                            } else {

                                this.transferComponent = contentHTML;

                                let selectedItem = this.sidebar.querySelectorAll('.selected')[0];

                                if (selectedItem !== undefined) {
                                    selectedItem.classList.remove('selected');
                                }

                                element.classList.add('selected');
                            }
                        }

                    } else {

                        console.log('you have to add a structural block first');
                    }
                }
            }

        } else {

            console.log('only in editor mode');
        }
    }

    function _handleControlClick(e) {

        if (e.target.closestClass('structure') !== null) {

            let elementBlock = e.target.closestClass('structure');

            if (elementBlock.index() !== -1) {

                this.selectedBlock = elementBlock.index();

                this.setActiveBlock();

                if (this.transferComponent !== null && elementBlock.children.length === 0) {

                    elementBlock.innerHTML = this.transferComponent;

                    this.transferComponent = null;
                }
            }
        }

        if (e.target.closestClass('control-item') !== null) {

            let element = e.target.closestClass('control-item');

            let elementBlock = element.closestClass('block');

            switch (element.dataset.command) {
                case 'move_up':
                case 'move_down':

                    var elementNode = elementBlock,
                        elementParent = elementBlock;

                    if (this.transferComponent !== null) {

                        let elementAuxiliar = document.createElement('div');
                        elementAuxiliar.innerHTML = this.transferComponent;

                        elementNode = elementAuxiliar.firstChild;

                        if (elementNode.classList[1] !== elementParent.classList[1]) {
                            return;
                        }

                    } else {

                        if (element.dataset.command === 'move_up') {

                            elementParent = elementBlock.nextElementSibling;

                        } else {

                            elementParent = elementBlock.previousElementSibling;
                        }
                    }

                    if (elementParent !== null) {

                        if (element.dataset.command === 'move_up') {

                            elementParent.parentNode.insertBefore(elementNode, elementParent);

                        } else {

                            elementParent.parentNode.insertBefore(elementNode, elementParent.nextSibling);
                        }
                    }

                    this.transferComponent = null;

                    var selectedItem = this.sidebar.querySelectorAll('.selected')[0];

                    if (selectedItem !== undefined) {
                        selectedItem.classList.remove('selected');
                    }

                    element.classList.add('selected');

                    break;
                case 'collapse':
                    break;
                case 'cut':
                case 'copy':

                    if (this.transferComponent === null) {

                        this.transferComponent = elementBlock.outerHTML;

                        if (element.dataset.command === 'cut') {

                            elementBlock.parentNode.removeChild(elementBlock);
                        }

                    } else {

                        console.log('you already have an element in your transfer area');
                    }

                    break;
                case 'view':

                    element.classList.toggle('active');

                    if (element.classList.contains('active')) {

                        elementBlock.setAttribute('data-hidden', '');

                    } else {

                        elementBlock.removeAttribute('data-hidden');
                    }

                    break;
                case 'code':
                    break;
                case 'delete':

                    if (elementBlock.classList.contains('active')) {

                        let parentNode = elementBlock.index() === 0 ? elementBlock.furthestClass('structure') : elementBlock.closestClass('structure');

                        if (parentNode !== null) {

                            parentNode.classList.add('active');

                            this.selectedBlock = parentNode.index();

                            this.setActiveBlock();
                        }
                    }

                    elementBlock.parentNode.removeChild(elementBlock);

                    if (elementBlock.classList.contains('structure')) {

                        if (this.sectionBuild.querySelectorAll('.structure').length === 0) {

                            let selectedItem = this.sidebar.querySelectorAll('.item-container.selected')[0];

                            if (selectedItem !== undefined) {
                                selectedItem.classList.remove('selected');
                            }
                        }
                    }

                    break;
            }
        }
    }

    /* ----------------------------------------------------------- */
    /* == helpers */
    /* ----------------------------------------------------------- */

    Element.prototype.index = function () {

        return Array.from(this.parentElement.children).indexOf(this);
    };

    Array.prototype.getReference = function (component) {

        let reference = '';

        this.forEach(function (item) {

            if (typeof item !== 'string' && component.toUpperCase() === item.constructor.name.toUpperCase()) {

                reference = item;

                return false;
            }

        });

        return reference;
    };

    String.prototype.insertInto = function () {

        if (window.getSelection) {

            let selection = window.getSelection();

            if (selection.getRangeAt && selection.rangeCount) {

                let range = selection.getRangeAt(0);

                range.deleteContents();

                let el = document.createElement('div');

                el.innerHTML = this;

                let lastElement = null,
                    fragment = document.createDocumentFragment();

                while (el.firstChild) {

                    lastElement = fragment.appendChild(el.firstChild);
                }

                range.insertNode(fragment);

                if (lastElement) {

                    range = range.cloneRange();

                    range.setStartAfter(lastElement);

                    range.collapse(true);

                    selection.removeAllRanges();

                    selection.addRange(range);

                    return lastElement;
                }
            }
        }
    };

    Document.prototype.createNode = function (nodeName, nodeContent, properties) {

        let node = document.createElement('div');

        node.appendNode(nodeName, nodeContent, properties);

        return node.firstChild;
    };

    Document.prototype.createElementHTML = function (contentHTML) {

        let temporary = document.createElement('template');
        temporary.innerHTML = contentHTML;

        return temporary.content.firstChild;
    };

    Element.prototype.appendNode = function (nodeName, nodeContent, properties) {

        let elementNode = document.createElement(nodeName);

        elementNode.innerHTML = nodeContent;

        Object.entries(properties).forEach(function (item, i) {

            elementNode.setAttribute(item[0], item[1]);

        });

        this.appendChild(elementNode);
    };

    Element.prototype.appendHTML = function (contentHTML) {

        this.appendChild(document.createElementHTML(contentHTML));
    };

    Element.prototype.getProperty = function () {

        let properties = [];

        [].forEach.call(this.attributes, function (attribute) {

            properties[attribute.name] = attribute.nodeValue;

        });

        return properties;
    };

    /* ----------------------------------------------------------- */
    /* == extend */
    /* ----------------------------------------------------------- */

    function initStructure(context) {
        return new structure.init(context);
    }

    function initAccordion(context) {
        return new accordion.init(context);
    }

    function initHorizontalRule() {
        return '<hr data-component="horizontal_rule" />';
    }

    function initImage(context) {
        return new image.init(context);
    }

    function initParagraph(context) {
        return new paragraph.init(context);
    }

    function initCard(context) {
        return new card.init(context);
    }

    function initTab(context) {
        return new tab.init(context);
    }

    function initList(context) {
        return new list.init(context);
    }

    function initTable(context) {
        return new table.init(context);
    }

    function initHeader(context) {
        return new header.init(context);
    }

    function initIframe(context) {
        return new iframe.init(context);
    }

    /* ----------------------------------------------------------- */
    /* == return */
    /* ----------------------------------------------------------- */

    return {
        init: Editor
    };

}));
