
let searchItem = null,
    searchItemForm = null,
    searchItemResult = null;

(function () {

    searchItem = document.getElementById('search_item');

    new modal.init(searchItem, {

        beforeOpen: function () {

            searchItemForm.reset();

            searchItemResult.innerHTML = '';
        },

        onOpen: function () {

            searchItemForm.elements[0].focus();
        }

    });

    searchItemForm = searchItem.querySelectorAll('#search_item_form')[0];

    searchItemResult = searchItem.querySelectorAll('#search_item_result')[0];

})();

/*
 * --- Search item response
 */

function searchItemSuccess(response) {

    if (typeof response === 'object') {

        if (response.message !== null) {

            searchItemResult.innerHTML = response.feedback;

        } else {

            setItemInformation(response);
        }

    } else {

        searchItemResult.innerHTML = response;

        searchItem.scrollTo(0, 0);
    }
}

/*
 * --- Select item response
 */

function selectItemSuccess(response) {

    if (response.isSuccess) {

        setItemInformation(response);

    } else {

        searchItemResult.innerHTML = '';
    }
}

function setItemInformation(response) {

    let plugin = new modal.get(searchItem)

    let form = document.querySelectorAll('form')[0];

    let element = plugin.trigger.previousElementSibling;

    form.querySelectorAll(`input[id=${element.dataset.reference}]`)[0].value = response.id;

    element.value = response.name;

    plugin.close();
}