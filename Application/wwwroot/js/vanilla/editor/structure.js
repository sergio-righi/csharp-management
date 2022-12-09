
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.structure = factory();
    }

}(this, function () {

    /* ----------------------------------------------------------- */
    /* == component */
    /* ----------------------------------------------------------- */

    function Structure(context) {

        this.context = context;

        _bindEvent.call(this);

        return this;
    }

    Structure.prototype.getControl = function () {

        let configurationBlock = document.createElement('div');
        configurationBlock.className = 'configuration-block';

        configurationBlock.appendNode('button', 'Uma', { 'type': 'button', 'name': 'column_1', 'class': 'action-item', 'value': 'col col-12' });
        configurationBlock.appendNode('button', 'Duas', { 'type': 'button', 'name': 'column_2', 'class': 'action-item', 'value': 'col col-12 col-sm-6' });
        configurationBlock.appendNode('button', 'Três', { 'type': 'button', 'name': 'column_3', 'class': 'action-item', 'value': 'col col-12 col-md-4' });
        configurationBlock.appendNode('button', 'Quatro', { 'type': 'button', 'name': 'column_4', 'class': 'action-item', 'value': 'col col-12 col-sm-6 col-md-3' });

        return configurationBlock.outerHTML;
    };

    Structure.prototype.getComponent = function (cls, component, control, content, hidden) {

        return `<div class="block ${cls}" ${hidden ? `data-hidden` : ''}>` +
            '<div class="toolbar">' +
            `<div class="menu-action">${control}</div>` +
            '<div class="menu-control">' +
            '<span class="control-item arrow move-up" data-command="move_up"></span>' +
            '<span class="control-item arrow move-down" data-command="move_down"></span>' +
            (cls === 'component' ?
                '<span class="control-item cut" data-command="cut"></span>' +
                '<span class="control-item copy" data-command="copy"></span>' +
                `<span class="control-item view ${hidden ? `active` : ''}" data-command="view"></span>` +
                '<span class="control-item code" data-command="code"></span>'
                :
                '') +//'<span class="control-item collapse" data-command="collapse"></span>') +
                '<span class="control-item delete" data-command="delete"></span>' +
            '</div>' +
            '</div>' +
            `<div class="content">${content}</div>` +
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
            columnClick: _handleColumnClick.bind(this)
        };

        this.context.sectionBuild.addEventListener('click', this._events.actionClick);
        this.context.sectionBuild.addEventListener('click', this._events.columnClick);
    }

    function _unbindEvent() {

    }

    function _handleActionClick(e) {

        if (/column_.*?/g.test(e.target.name)) {

            e.stopPropagation();

            let elementBlockContent = e.target.closestClass('structure').children[1];

            let columns = elementBlockContent.children;

            for (var i = 0; i < columns.length; i++) {

                columns[i].classList.remove('active');
                columns[i].setAttribute('class', e.target.value);
            }

            let numberOfColumns = e.target.name.split('_')[1];

            if (columns.length < numberOfColumns) {

                let contentHTML = [],
                    left = numberOfColumns - columns.length;

                for (i = 0; i < left; i++) {

                    contentHTML.push(`<div class="${e.target.value}"></div>`);
                }

                elementBlockContent.innerHTML += contentHTML.join('');

            } else if (columns.length > numberOfColumns) {

                let quantity = columns.length - numberOfColumns;

                for (i = 0; i < quantity; i++) {

                    elementBlockContent.removeChild(elementBlockContent.lastChild);
                }
            }

            elementBlockContent.querySelectorAll('.col')[0].classList.add('active');
        }
    }

    function _handleColumnClick(e) {

        if (e.target.closestClass('component') === null) {

            let element = e.target.closestClass('col');

            if (element !== null) {

                let elementBlock = element.closestClass('structure');

                if (elementBlock !== null) {

                    e.stopPropagation();

                    this.context.selectedBlock = elementBlock.index();

                    this.context.selectedColumn = element.index();

                    elementBlock.querySelectorAll('.col.active')[0].classList.remove('active');

                    element.classList.add('active');

                    this.context.setActiveBlock();

                    if (element.innerHTML === '' && this.context.transferComponent !== null) {

                        element.innerHTML += this.context.transferComponent;

                        this.context.transferComponent = null;
                    }
                }
            }
        }
    }

    /* ----------------------------------------------------------- */
    /* == return */
    /* ----------------------------------------------------------- */

    return {
        init: Structure
    };

}));
