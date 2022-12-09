
(function () {

    document.body.addEventListener('mousedown', function (e) {

        if (e.target) {

            let navigation = e.target.closestClass('navigation-stepper');

            if (navigation) {

                let step = e.target.closestClass('nav-step');

                if (step) {

                    if (!step.classList.contains('active')) {

                        let section = document.getElementById(step.dataset.target);

                        if (section) {

                            section.scrollIntoView({
                                behavior: 'smooth',
                                block: 'start',
                                inline: 'start',

                            });
                        }

                    } else {

                        navigation.querySelectorAll('.active').forEach(function (item) {

                            item.classList.remove('active');

                        });

                        step.classList.add('active');
                    }
                }
            }
        }

    });

    window.addEventListener('scroll', function (e) {

        let steps = document.getElementById('navigation_stepper').querySelectorAll('.nav-step');

        let reverse = [].slice.call(steps).reverse();

        reverse.forEach(function (item) {

            let section = document.getElementById(item.dataset.target);

            if (section && section.isInViewport()) {

                for (var step of steps) {

                    step.classList.remove('active');
                }

                item.classList.add('active');

                return false;
            }

        });

    }, false);

    document.getElementById('contact').addEventListener('mousedown', function (e) {

        copyToClipboard(this.textContent);
        createSnackbar(this.dataset.message);

    });

})();