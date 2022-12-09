
let searchPerson = null,
    searchPersonForm = null,
    searchPersonResult = null;

(function () {

    searchPerson = document.getElementById('search_person');

    new modal.init(searchPerson, {

        beforeOpen: function () {

            searchPersonForm.reset();

            searchPersonResult.innerHTML = '';
        },

        onOpen: function () {

            searchPersonForm.elements[2].focus();
        }

    });

    [].forEach.call(searchPerson.querySelectorAll('input[name=txt_document]'), function (item) {

        new mask.init(item, {});

    });

    searchPersonForm = searchPerson.querySelectorAll('#search_person_form')[0];

    searchPersonResult = searchPerson.querySelectorAll('#search_person_result')[0];

    [].forEach.call(searchPersonForm.querySelectorAll('input[name=rbt_person]'), function (item) {

        item.addEventListener('change', function (e) {

            let natural = searchPersonForm.querySelectorAll('#document_natural')[0];

            let juridical = searchPersonForm.querySelectorAll('#document_juridical')[0];

            if (parseInt(this.value, 10) === 1) {

                juridical.value = '';

                natural.classList.remove('d-none');
                natural.removeAttribute('disabled');

                juridical.classList.add('d-none');
                juridical.setAttribute('disabled', 'disabled');

                natural.focus();

            } else {

                natural.value = '';

                juridical.classList.remove('d-none');
                juridical.removeAttribute('disabled');

                natural.classList.add('d-none');
                natural.setAttribute('disabled', 'disabled');

                juridical.focus();
            }

        });

    });

})();

/*
 * --- Search person response
 */

function searchPersonSuccess(response) {

    if (typeof response === 'object') {

        if (response.message !== null) {

            searchPersonResult.innerHTML = response.feedback;

        } else {

            setPersonInformation(response);
        }
    } else {

        searchPersonResult.innerHTML = response;

        searchPerson.scrollTo(0, 0);
    }
}

/*
 * --- Select person response
 */

function selectPersonSuccess(response) {

    if (response.isSuccess) {

        setPersonInformation(response);

    } else {

        searchPersonResult.innerHTML = '';
    }
}

function setPersonInformation(response) {

    let plugin = new modal.get(searchPerson);
    
    let form = document.querySelectorAll('form')[0];

    let element = plugin.trigger.previousElementSibling;

    let hidden = form.querySelectorAll(`input[id=${element.dataset.reference}]`)[0];

    hidden.value = response.id;

    hidden.dispatchEvent(new Event('change'));

    element.value = response.name;

    plugin.close();
}