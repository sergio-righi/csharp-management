
let configInstallment = null,
    isDateInstallment = false,
    isQuantityInstallment = false;

(function () {

    dateInstallment = document.getElementById('date_installment');

    installmentId = document.getElementById('hidden_installment');

    quantityInstallment = document.getElementById('quantity_installment');

    configInstallment = document.getElementById('config_installment');

    new modal.init(configInstallment, {

        beforeOpen: function () {

            let plugin = new modal.get(configInstallment);

            let quantity = plugin.trigger.previousElementSibling.value;

            getInstallment(installmentId.value === '' ? 0 : installmentId.value, dateInstallment.value, quantity);
        }

    });

    if (!quantityInstallment.hasAttribute('disabled')) {

        isDateInstallment = true;
        isQuantityInstallment = true;
    }

    quantityInstallment.addEventListener('input', function () {

        quantityInstallmentEvent(this);

    });

    quantityInstallment.addEventListener('blur', function () {

        quantityInstallmentEvent(this);

    });

    quantityInstallment.addEventListener('focus', function () {

        quantityInstallmentEvent(this);

    });

    dateInstallment.addEventListener('input', function () {

        dateInstallmentEvent(this);

    });

    dateInstallment.addEventListener('blur', function () {

        dateInstallmentEvent(this);

    });

    dateInstallment.addEventListener('focus', function () {

        dateInstallmentEvent(this);

    });

})();

/*
 * --- search installments
 */

function getInstallment(installment_id, date, quantity) {

    let xhr = new XMLHttpRequest();

    date = date.toDate().toDateString();

    if (date.isDate()) {

        xhr.open('POST', String.format(route_config_installment, installment_id, date, quantity), true);
        xhr.responseType = 'json';
        xhr.onload = function () {

            if (xhr.status === 200) {

                new modal.get(configInstallment).setContent(xhr.response.content);
            }
        };

        xhr.send();
    }
}

function setInstallment(element) {

    //let inputs = document.getElementById('config_installment_form').querySelectorAll('input[type=text]:not([readonly])');

    //if (inputs !== null) {

    //    if (element.checked) {

    //        let date = inputs[0].value.toDate();

    //        [].forEach.call(inputs, function (item) {

    //            item.value = item.value.toDate().setDate(date.getDate()).toDateString();

    //        });

    //    } else {

    //        [].forEach.call(inputs, function (item) {

    //            item.value = item.dataset.date;

    //        });
    //    }
    //}
}

function setDisabled() {

    if (isQuantityInstallment && isDateInstallment) {

        quantityInstallment.nextElementSibling.removeAttribute('disabled');

    } else {

        quantityInstallment.nextElementSibling.setAttribute('disabled', true);
    }
}

/*
 * --- Events
 */

function quantityInstallmentEvent(element) {

    if (parseInt(element.value) > 0) {

        isQuantityInstallment = true;

    } else {

        isQuantityInstallment = false;
    }

    setDisabled();
}

function dateInstallmentEvent(element) {

    let date = element.value.toDate();

    if (date.toDateString().isDate()) {

        isDateInstallment = true;

    } else {

        isDateInstallment = false;
    }

    setDisabled();
}

/*
 * --- Config installment response
 */

function saveInstallmentSuccess(response) {

    if (response.isSuccess) {

        let plugin = new modal.get(configInstallment);

        document.getElementById('message').innerHTML = response.feedback;

        plugin.close();
    }
}