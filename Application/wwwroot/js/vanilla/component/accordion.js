
(function (document, window, index) {

    [].forEach.call(document.body.querySelectorAll('.accordion'), function (item) {

        item.addEventListener('click', function (event) {

            if (event.target) {

                event.stopPropagation();

                let exclusive = this.dataset.exclusivo !== undefined;

                let element = event.target.classList.contains('accordion-item') ? event.target : event.target.closestClass('accordion-item');

                if (element !== null) {

                    element.classList.toggle('active');
                    element.children[1].classList.toggle('active');

                    if (exclusive) {

                        [].forEach.call(element.parentNode.children, function (subitem) {

                            if (subitem !== element) {

                                subitem.classList.remove('active');
                                subitem.children[1].classList.remove('active');
                            }

                        });
                    }
                }
            }

        });

    });

}(document, window, 0));