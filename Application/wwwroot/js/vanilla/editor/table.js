
(function (root, factory) {

    if (typeof define === 'function' && define.amd) {

        define(factory);

    } else if (typeof exports === 'object') {

        module.exports = factory();

    } else {

        root.table = factory();
    }

}(this, function () {

    const Info = {
        name: 'table',
        version: '2.0'
    };

    const Default = {
        text: 'Lorem ipsum dolor sit amet',
        remove_row: 'Lorem ipsum dolor sit amet',
        remove_column: 'Lorem ipsum dolor sit amet'
    };

    const Header = {
        none: 0,
        top: 1,
        left: 2,
        both: 3
    };

    const Form = {
        url: 'table_url',
        row: 'table_row',
        header: 'table_header',
        column: 'table_column',
        submit: 'submit_table'
    };

    /* ----------------------------------------------------------- */
    /* == component */
    /* ----------------------------------------------------------- */

    function EditorTable(context) {

        this.row = -1;
        this.column = -1;
        this.context = context;

        _bindEvent.call(this);

        return this;
    }

    EditorTable.prototype.getControl = function () {

        let configurationBlock = document.createElement('div');
        configurationBlock.className = 'configuration-block';

        configurationBlock.appendNode('button', '<i class="mdi mdi-table-row-plus-after"></i>', { 'type': 'button', 'name': 'table_add_row', 'class': 'action-item d-none' });
        configurationBlock.appendNode('button', '<i class="mdi mdi-table-row-remove"></i>', { 'type': 'button', 'name': 'table_remove_row', 'class': 'action-item d-none' });

        let configurationBlock1 = document.createElement('div');
        configurationBlock1.className = 'configuration-block';

        configurationBlock1.appendNode('button', '<i class="mdi mdi-table-column-plus-after"></i>', { 'type': 'button', 'name': 'table_add_column', 'class': 'action-item d-none' });
        configurationBlock1.appendNode('button', '<i class="mdi mdi-table-column-remove"></i>', { 'type': 'button', 'name': 'table_remove_column', 'class': 'action-item d-none' });

        let configurationBlock2 = document.createElement('div');
        configurationBlock2.className = 'configuration-block';

        configurationBlock2.appendNode('button', '<i class="mdi mdi-link-variant"></i>', { 'type': 'button', 'name': 'add_link', 'class': 'action-item d-none' });
        configurationBlock2.appendNode('button', '<i class="mdi mdi-settings"></i>', { 'type': 'button', 'name': 'style_set', 'class': 'action-item d-none' });

        return configurationBlock.outerHTML + configurationBlock1.outerHTML + configurationBlock2.outerHTML;
    };

    EditorTable.prototype.getForm = function (row, column, header, create) {

        return '<form>' +
            '<div class="container-fluid">' +
            '<div class="row">' +
            '<div class="col col-12 col-sm-4">' +
            '<div class="form-group">' +
            '<label>Number of rows</label>' +
            `<input type="text" name="${Form.row}" value="${row === null ? '' : row}" autofocus />` +
            '</div>' +
            '</div>' +
            '<div class="col col-12 col-sm-4">' +
            '<div class="form-group">' +
            '<label>Number of columns</label>' +
            `<input type="text" name="${Form.column}" value="${column === null ? '' : column}" />` +
            '</div>' +
            '</div>' +
            '<div class="col col-12 col-sm-4">' +
            '<div class="form-group">' +
            '<label>Moldura</label>' +
            `<select name="${Form.header}">` +
            `<option value="0"${header === 0 ? ' selected' : ''}>None</option>` +
            `<option value="1"${header === 1 ? ' selected' : ''}>Top</option>` +
            `<option value="1"${header === 2 ? ' selected' : ''}>Left</option>` +
            `<option value="1"${header === 3 ? ' selected' : ''}>Both</option>` +
            '</select>' +
            '</div>' +
            '</div>' +
            '<div class="col col-12">' +
            '<div class="form-group">' +
            `<button type="button" name="${Form.submit}" class="button ${create ? 'button-add' : 'button-save'}">Table</button>` +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</form>';
    };

    EditorTable.prototype.getContent = function () {

        return this.getForm(null, null, null, true);
    };

    EditorTable.prototype.setContent = function (element, content, hide) {

        let elementBlock = element.closestClass('component');

        [].forEach.call(elementBlock.children[0].querySelectorAll('button'), function (item) {

            if (hide) {

                item.classList.add('d-none');

            } else {

                item.classList.remove('d-none');
            }

        });

        elementBlock.children[1].innerHTML = content;
    };

    /* ----------------------------------------------------------- */
    /* == private methods */
    /* ----------------------------------------------------------- */

    function _build() {

    }

    function _bindEvent() {

        this._events = {
            actionClick: _handleActionClick.bind(this),
            submitClick: _handleSubmitClick.bind(this),
            contentClick: _handleContentClick.bind(this)
        };

        this.context.sectionBuild.addEventListener('click', this._events.actionClick);
        this.context.sectionBuild.addEventListener('click', this._events.submitClick);
        this.context.sectionBuild.addEventListener('click', this._events.contentClick);
    }

    function _unbindEvent() {

    }

    function _handleActionClick(e) {

        let element = e.target.closestNode('button');

        if (element !== null && /table_.*?/g.test(element.name)) {
            
            let contentHTML = [];

            let elementBlock = element.closestClass('component');

            let elementBlockComponent = elementBlock.querySelectorAll('table')[0];

            if (elementBlockComponent !== undefined) {

                let row = parseInt(elementBlockComponent.dataset.row, 10),
                    column = parseInt(elementBlockComponent.dataset.column, 10),
                    header = parseInt(elementBlockComponent.dataset.header, 10);

                let elementBody = elementBlockComponent.querySelectorAll('tbody')[0];

                switch (element.name) {
                    case 'table_add_row':

                        contentHTML.push('<tr>');

                        for (var i = 0; i < column; i++) {

                            if (i === 0 && (header === Header.left || header === Header.both)) {

                                contentHTML.push(`<th>${Default.text}</th>`);

                            } else {

                                contentHTML.push(`<td>${Default.text}</td>`);
                            }
                        }

                        contentHTML.push('</tr>');

                        if (this.row !== -1) {

                            let elementRow = elementBody.childNodes[this.row];

                            if (elementRow !== undefined) {

                                elementBody.insertBefore(document.createElementHTML(contentHTML.join('')), elementRow.nextSibling);
                            }

                        } else {

                            elementBody.appendHTML(contentHTML.join(''));
                        }

                        elementBlockComponent.setAttribute('data-row', row + 1);

                        break;
                    case 'table_remove_row':

                        if (this.row !== -1 && this.column !== -1) {

                            let elementRow = elementBody.childNodes[this.row];

                            if (elementRow !== undefined) {

                                elementRow.classList.add('active');
                                
                                setTimeout(function () {

                                    let confirmation = confirm(Default.remove_row);

                                    if (confirmation) {

                                        elementRow.parentNode.removeChild(elementRow);

                                        elementBlockComponent.setAttribute('data-row', row - 1);

                                    } else {

                                        elementRow.classList.remove('active');
                                    }

                                }, 100);
                            }
                        }

                        break;
                    case 'table_add_column':

                        this.column = (this.column === -1 ? column - 1 : this.column);

                        let index = this.column;

                        [].forEach.call(elementBlockComponent.querySelectorAll('tr'), function (item, i) {

                            let elementColumn = item.childNodes[index];

                            if (elementColumn !== undefined) {

                                if (i === 0 && (header === Header.top || header === Header.both)) {

                                    elementColumn.parentNode.insertBefore(document.createElementHTML(`<th>${Default.text}</th>`), elementColumn.nextSibling);

                                } else {

                                    elementColumn.parentNode.insertBefore(document.createElementHTML(`<td>${Default.text}</td>`), elementColumn.nextSibling);
                                }
                            }

                        });

                        elementBlockComponent.setAttribute('data-column', column + 1);

                        break;
                    case 'table_remove_column':

                        if (this.row !== -1 && this.column !== -1) {

                            let index = this.column;

                            _setColumnHighlight.call(this, elementBlockComponent, true);

                            setTimeout(function () {

                                let confirmation = confirm(Default.remove_column);

                                if (confirmation) {

                                    [].forEach.call(elementBlockComponent.querySelectorAll('tr'), function (item) {

                                        let elementColumn = item.querySelectorAll('td, th')[index];

                                        if (elementColumn !== undefined) {

                                            elementColumn.parentNode.removeChild(elementColumn);
                                        }

                                    });

                                    elementBlockComponent.setAttribute('data-column', column - 1);
                                }

                                _setColumnHighlight.call(self, elementBlockComponent, false);

                            }, 100);
                        }

                        break;
                }

                this.row = this.column = -1;

                elementBlock.querySelectorAll('button[name=table_remove_row]')[0].classList.add('disabled');

                elementBlock.querySelectorAll('button[name=table_remove_column]')[0].classList.add('disabled');
            }
        }
    }

    function _handleSubmitClick(e) {

        let element = e.target.closestNode('button');

        if (element !== null && element.name === Form.submit) {
            
            let row = element.form[0].value;

            let column = element.form[1].value;

            let header = parseInt(element.form[2].selectedIndex, 10);

            if (row && column) {

                let headerHTML = [],
                    contentHTML = [];

                let elementComponent = document.createNode('div', '', { 'class': 'table-responsive', 'data-component': Info.name, 'data-version': Info.version });

                let elementTable = document.createNode('table', '', { 'data-row': row, 'data-column': column, 'data-header': header });

                for (var i = 0; i < row; i++) {

                    if (i === 0 && header === Header.top) {

                        headerHTML.push('<tr>');

                    } else {

                        contentHTML.push('<tr>');
                    }

                    for (var j = 0; j < column; j++) {

                        switch (header) {

                            case Header.none:

                                contentHTML.push(`<td>${Default.text}</td>`);

                                break;

                            case Header.top:

                                if (i === 0) {

                                    headerHTML.push(`<th>${Default.text}</th>`);

                                } else {

                                    contentHTML.push(`<td>${Default.text}</td>`);
                                }

                                break;

                            case Header.left:

                                if (j === 0) {

                                    contentHTML.push(`<th>${Default.text}</th>`);

                                } else {

                                    contentHTML.push(`<td>${Default.text}</td>`);
                                }

                                break;

                            case Header.both:

                                if (i === 0 || j === 0) {

                                    contentHTML.push(`<th>${Default.text}</th>`);

                                } else {

                                    contentHTML.push(`<td>${Default.text}</td>`);
                                }

                                break;
                        }
                    }

                    if (i === 0 && header === Header.top) {

                        headerHTML.push('</tr>');

                    } else {

                        contentHTML.push('</tr>');
                    }
                }

                let headerTable = document.createNode('thead', headerHTML.join('').replace(new RegExp(this.texto, 'g'), ''), { 'contenteditable': true });

                let contentTable = document.createNode('tbody', contentHTML.join('').replace(new RegExp(this.texto, 'g'), ''), { 'contenteditable': true });

                elementTable.appendChild(headerTable);
                elementTable.appendChild(contentTable);

                elementComponent.appendChild(elementTable);

                this.setContent(element, elementComponent.outerHTML, false);

            } else {

                console.log('row and column are required fields');
            }
        }
    }

    function _handleContentClick(e) {

        let element = e.target.closestNode('td');

        if (element !== null) {

            let elementBlock = element.closestClass('component');

            let column = element.index(),
                row = element.parentNode.index();

            if (this.column !== column || this.row !== row) {

                this.row = row;

                this.column = column;

                elementBlock.querySelectorAll('button[name=table_remove_row]')[0].classList.remove('disabled');

                elementBlock.querySelectorAll('button[name=table_remove_column]')[0].classList.remove('disabled');

            } else {

                this.column = this.row = -1

                elementBlock.querySelectorAll('button[name=table_remove_row]')[0].classList.add('disabled');

                elementBlock.querySelectorAll('button[name=table_remove_column]')[0].classList.add('disabled');
            }
        }
    }

    function _setColumnHighlight(element, show) {

        let self = this;

        [].forEach.call(element.querySelectorAll('tr'), function (row, i) {

            [].forEach.call(row.children, function (column, j) {

                if (self.column === j) {

                    if (show) {

                        column.setAttribute('class', 'active');
                    }

                } else {

                    column.removeAttribute('class');
                }

            });

        });
    }

    /* ----------------------------------------------------------- */
    /* == return */
    /* ----------------------------------------------------------- */

    return {
        init: EditorTable
    };

}));
