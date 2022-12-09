
let searchFile = null,
    searchFileForm = null,
    searchFileResult = null;

(function () {

    searchFile = document.getElementById('search_file');

    new modal.init(searchFile, {

        beforeOpen: function () {

            searchFileForm.reset();

            searchFileResult.innerHTML = '';
        },

        onOpen: function () {

            searchFileForm.elements[0].focus();
        }

    });

    searchFileForm = searchFile.querySelectorAll('#search_file_form')[0];

    searchFileResult = searchFile.querySelectorAll('#search_file_result')[0];

})();

/*
 * --- Search file response
 */

function searchFileSuccess(response) {

    if (typeof response === 'object') {

        if (response.message !== null) {

            searchFileResult.innerHTML = response.feedback;

        } else {

            setFileInformation(response);
        }

    } else {

        searchFileResult.innerHTML = response;

        searchFile.scrollTo(0, 0);
    }
}

/*
 * --- Select file response
 */

function selectFileSuccess(result) {

    if (result.isSuccess) {

        setFileInformation(result);

    } else {

        searchFileResult.innerHTML = '';
    }
}

function setFileInformation(result) {

    let plugin = new modal.get(searchFile);

    let form = document.querySelectorAll('form')[0];

    let element = plugin.trigger.previousElementSibling;

    form.querySelectorAll(`input[id=${element.dataset.reference}]`)[0].value = result.id;

    element.value = result.name;

    plugin.close();
}