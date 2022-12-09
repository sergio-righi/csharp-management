
(function (document, window, index) {

    [].forEach.call(document.body.querySelectorAll('.tab-header > .tab-item'), function (item) {

        if (item.nodeName !== 'A') {

            item.addEventListener('click', function (event) {

                if (event.target) {

                    const target = event.target.classList.contains('tab-item') ? event.target : event.target.closestClass('tab-item');

                    const header = target.parentNode.children;

                    const element = [].filter.call(header, function (item) {

                        if (item.classList.contains('active')) {

                            return item;
                        }

                    })[0];

                    const content = target.parentNode.nextElementSibling.children;

                    const selected = Array.prototype.slice.call(header).indexOf(element);

                    const index = Array.prototype.slice.call(header).indexOf(target);

                    if (index !== -1 && target !== element) {

                        header[index].classList.toggle('active');
                        content[index].classList.toggle('active');

                        if (selected !== -1) {

                            header[selected].classList.remove('active');
                            content[selected].classList.remove('active');
                        }
                    }
                }

            });
        }

    });

}(document, window, 0));