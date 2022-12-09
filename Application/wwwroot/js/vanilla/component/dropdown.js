
(function (document, window, index) {

    document.addEventListener('mouseup', function (event) {

        if (event.target) {

            let trigger = event.target.closestClass('dropdown-trigger');

            if (trigger !== null) {

                trigger.parentNode.classList.toggle('active');

            } else {

                let elements = document.body.querySelectorAll('.dropdown');

                [].forEach.call(elements, function (item) {

                    if (item !== event.target && !item.contains(event.target)) {

                        item.classList.remove('active');
                    }

                });
            }
        }

    });

}(document, window, 0));