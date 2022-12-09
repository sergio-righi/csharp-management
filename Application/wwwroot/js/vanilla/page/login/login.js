
(function () {

    let input = document.getElementById('password');

    if (input !== null) {

        let icon = document.getElementById('caps_lock_icon');

        input.addEventListener('keyup', function (event) {

            if (event.getModifierState('CapsLock')) {

                icon.classList.remove('d-none');

            } else {

                icon.classList.add('d-none');
            }

        });

        input.addEventListener('blur', function (event) {

            icon.classList.add('d-none');

        });
    }

}());