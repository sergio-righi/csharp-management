
(function (document, window, index) {

    [].forEach.call(document.body.querySelectorAll('.button-float'), function (item) {

        item.addEventListener('click', function (event) {

            this.parentNode.classList.toggle('active');

        });

        item.parentNode.addEventListener('mouseleave', function (event) {

            this.classList.remove('active');

        });

    });

}(document, window, 0));