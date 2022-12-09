(function (document, window, index) {
    
    [].forEach.call(document.body.querySelectorAll('input[type=file]'), function (item) {

        let label = item.nextElementSibling, labelVal = label.innerHTML;

        item.addEventListener('change', function (event) {

            event.preventDefault();

            event.stopImmediatePropagation();

            let fileName = '';

            if (this.files && this.files.length > 1) {
                
                fileName = (this.getAttribute('data-message') || '').replace('{counter}', this.files.length);

            } else {

                fileName = event.target.value.split('\\').pop();
            }

            if (fileName) {

                label.innerHTML = fileName;

            } else {

                label.innerHTML = labelVal;
            }

        });

    });

}(document, window, 0));