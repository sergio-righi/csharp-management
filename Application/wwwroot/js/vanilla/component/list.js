
(function (document, window, index) {

    document.addEventListener('click', function (event) {

        let icon = event.target.closestClass('list-icon');

        if (icon === null) {

            let target = event.target.closestClass('list-item');

            if (event.target && target !== null && !target.classList.contains('list-disabled')) {

                let element = target.querySelectorAll('input')[0];

                if (element !== null) {

                    event.stopPropagation();

                    if (element.type === 'radio') {

                        element.checked = true;

                        event.target.parentElement.querySelectorAll(`[name=${element.name}]`).forEach(function (item) {

                            if (item !== element) {

                                item.closestClass('list-item').classList.remove('active');

                            } else {

                                item.closestClass('list-item').classList.add('active');
                            }

                        });

                    } else {

                        element.checked = !element.checked;

                        element.closestClass('list-item').classList.toggle('active');
                    }
                }
            }
        }

    });

}(document, window, 0));