
(function () {

    document.addEventListener('visibilitychange', function (e) {

        if (!document.hidden) {

            updateSaleProduct(document.getElementById('hidden_id').value);
        }

    });

})();

/*
 * update product
 */

function updateSaleProduct(id) {

    let rent_result = document.getElementById('sale_product');

    let xhr = new XMLHttpRequest();

    xhr.open('POST', String.format(route_sale_sync, id), true);
    xhr.responseType = 'html';
    xhr.onload = function () {

        if (xhr.status === 200) {

            rent_result.innerHTML = xhr.response;
        }
    };

    xhr.send();

}