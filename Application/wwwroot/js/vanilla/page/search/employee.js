
let searchEmployee = null,
    searchEmployeeForm = null,
    searchEmployeeResult = null;

(function () {

    searchEmployee = document.getElementById('search_employee');

    new modal.init(searchEmployee, {

        beforeOpen: function () {

            searchEmployeeForm.reset();

            searchEmployeeResult.innerHTML = '';
        },

        onOpen: function () {

            searchEmployeeForm.elements[1].focus();
        }

    });

    [].forEach.call(searchEmployee.querySelectorAll('input[name=txt_document]'), function (item) {

        new mask.init(item, {});

    });

    searchEmployeeForm = searchEmployee.querySelectorAll('#search_employee_form')[0];

    searchEmployeeResult = searchEmployee.querySelectorAll('#search_employee_result')[0];

})();

/*
 * --- Search employee response
 */

function searchEmployeeSuccess(response) {

    if (typeof response === 'object') {

        if (response.message !== null) {

            searchEmployeeResult.innerHTML = response.feedback;

        } else {

            setEmployeeInformation(response);
        }
    } else {

        searchEmployeeResult.innerHTML = response;

        searchEmployee.scrollTo(0, 0);
    }
}

/*
 * --- Select employee response
 */

function selectEmployeeSuccess(response) {

    if (response.isSuccess) {

        setEmployeeInformation(response);

    } else {

        searchEmployeeResult.innerHTML = '';
    }
}

function setEmployeeInformation(response) {

    let plugin = new modal.get(searchEmployee);

    let form = document.querySelectorAll('form')[0];

    let element = plugin.trigger.previousElementSibling;

    let hidden = form.querySelectorAll(`input[id=${element.dataset.reference}]`)[0];

    hidden.value = response.id;

    hidden.dispatchEvent(new Event('change'));

    element.value = response.name;

    plugin.close();
}