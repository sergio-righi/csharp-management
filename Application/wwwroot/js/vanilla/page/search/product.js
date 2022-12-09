
let float_component = null;

(function () {

    document.addEventListener('visibilitychange', function (e) {

        if (!document.hidden) {

            let confirmation = confirm('Deseja sincronizar?')

            if (confirmation) {

                updateProduct(document.getElementById('search_tool').value, document.getElementById('search_id').value);
            }
        }

    });

    float_component = document.getElementById('search_product_float');

    float_component.addEventListener('click', function (e) {

        if (e.target) {

            let float_header = e.target.closestClass('float-header');

            if (float_header !== null) {

                let float_item = this.querySelectorAll('.float-item')[0];

                if (float_item !== null && float_item.children.length > 0) {

                    this.classList.toggle('active');
                }
            }
        }

    });

})();

/*
 * add product
 */

function addProductSubmit(form) {

    form.reset();
}

function addProductSuccess() {

    float_component.classList.add('active');
}

/*
 * save product
 */

function saveProductSuccess() {

    float_component.classList.remove('active');
}

/*
 * remove product
 */

function removeProductSuccess() {

    if (float_component.querySelectorAll('.float-item')[0].children.length === 0) {

        float_component.classList.remove('active');
    }
}

/*
 * reset product
 */

function resetProductSuccess() {

    if (float_component.querySelectorAll('.float-item')[0].children.length === 0) {

        float_component.classList.remove('active');
    }
}

/*
 * sync product
 */

function updateProduct(tool_id, id) {

    let search_product_resume = document.getElementById('search_product_resume');

    let xhr = new XMLHttpRequest();

    xhr.open('POST', String.format(route_search_product_sync, tool_id, id), true);
    xhr.responseType = 'html';
    xhr.onload = function () {

        if (xhr.status === 200) {

            if (xhr.response.result !== undefined) {

                search_product_resume.innerHTML = xhr.response.result;
            }
        }
    };

    xhr.send();

}