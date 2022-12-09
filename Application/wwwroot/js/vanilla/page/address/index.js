
(function () {

    let hidden_person = document.getElementById('hidden_person');

    if (hidden_person !== null) {

        hidden_person.addEventListener('change', function () {

            /// list_address
            list_request(route_address_list_address, this.value, 'dropdownlist_address');

        });
    }

    let dropdownlist_state = document.getElementById('dropdownlist_state');

    if (dropdownlist_state !== null) {

        dropdownlist_state.addEventListener('change', function () {

            /// list_city_state
            list_request(route_address_list_city_state, this.value, 'dropdownlist_city');

        });
    }

    let dropdownlist_country = document.getElementById('dropdownlist_country');

    if (dropdownlist_country !== null) {

        dropdownlist_country.addEventListener('change', function () {

            /// list_city_state
            list_request(route_address_list_state, this.value, 'dropdownlist_state');

        });
    }

})();

function list_request(route, param, element) {

    if (param !== null) {

        let xhr = new XMLHttpRequest();

        xhr.open('POST', String.format(route, param), true);
        xhr.responseType = 'json';
        xhr.onload = function () {

            if (xhr.status === 200) {

                if (xhr.response.result.length > 0) {

                    buildDropdownlist(xhr.response.result, element);

                } else {

                    list_request(route_address_list_city_country, param, 'dropdownlist_city');
                }
            }
        };

        xhr.send();
    }
}

//function list_state(country_id) {

//    if (country_id !== null) {

//        let xhr = new XMLHttpRequest();

//        xhr.open('POST', String.format(route_address_list_state, country_id), true);
//        xhr.responseType = 'json';
//        xhr.onload = function () {

//            if (xhr.status === 200) {

//                if (xhr.response.result.length > 0) {

//                    buildDropdownlist(xhr.response.result, 'dropdownlist_state');

//                } else {

//                    list_country_city(country_id);
//                }
//            }
//        };

//        xhr.send();
//    }
//}

//function list_city(state_id) {

//    if (state_id !== null) {

//        let xhr = new XMLHttpRequest();

//        xhr.open('POST', String.format(route_address_list_city_state, state_id), true);
//        xhr.responseType = 'json';
//        xhr.onload = function () {

//            if (xhr.status === 200) {

//                buildDropdownlist(xhr.response.result, 'dropdownlist_city');
//            }
//        };

//        xhr.send();
//    }
//}

//function list_country_city(country_id) {

//    if (country_id !== null) {

//        let xhr = new XMLHttpRequest();

//        xhr.open('POST', String.format(route_address_list_city_country, country_id), true);
//        xhr.responseType = 'json';
//        xhr.onload = function () {

//            if (xhr.status === 200) {

//                buildDropdownlist(xhr.response.result, 'dropdownlist_city');
//            }
//        };

//        xhr.send();
//    }
//}

//function list_address(person_id) {

//    if (person_id !== null) {

//        let xhr = new XMLHttpRequest();

//        xhr.open('POST', String.format(route_address_list_address, person_id), true);
//        xhr.responseType = 'json';
//        xhr.onload = function () {

//            if (xhr.status === 200) {

//                buildDropdownlist(xhr.response.result, 'dropdownlist_address');
//            }
//        };

//        xhr.send();
//    }
//}

function buildDropdownlist(json, element_id) {

    let dropdownlist = [];
    let element = document.getElementById(element_id);

    if (element !== null) {

        [].forEach.call(json, function (item, i) {

            dropdownlist.push(new Option(item.name, item.id).outerHTML);

        });

        element.innerHTML = dropdownlist.join('');
    }
}