(function (document, window, index) {

    //[].forEach.call(document.body.querySelectorAll('[data-close]'), function (item) {

    //    item.addEventListener('click', function (event) {

    //        let element = this.closestClass(this.dataset.close);

    //        if (element !== undefined) {

    //            element.parentNode.removeChild(element);

    //            let customEvent = new CustomEvent(`${this.dataset.fechar}.close`, { 'detail': this });

    //            element.dispatchEvent(customEvent);
    //        }

    //    });

    //});

    document.addEventListener('click', function (event) {

        if (event.target) {

            let dataClose = event.target.closestAttribute('data-close');

            if (dataClose !== null) {

                let element = dataClose.closestClass(dataClose.dataset.close);

                if (element !== undefined) {

                    var confirmation = confirm('This action cannot be undone. Are you sure?');

                    if (confirmation) {

                        element.parentNode.removeChild(element);

                        let customEvent = new CustomEvent(`${dataClose.dataset.close}.close`, { 'detail': dataClose });

                        element.dispatchEvent(customEvent);
                    }
                }

            } else {

                let dataDismiss = event.target.closestAttribute('data-dismiss');

                if (dataDismiss !== null) {

                    let element = dataDismiss.closestClass(dataClose.dataset.dismiss);

                    if (element !== undefined) {

                        element.parentNode.removeChild(element);
                    }
                }
            }
        }

    });

}(document, window, 0));