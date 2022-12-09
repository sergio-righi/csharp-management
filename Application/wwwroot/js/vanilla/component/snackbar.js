
var createSnackbar = (function () {

    let previous = null;

    return function (message, actionText, action) {
        if (previous) {
            previous.dismiss();
        }
        let snackbar = document.createElement('div');
        snackbar.className = 'snackbar';
        snackbar.dismiss = function () {
            this.style.opacity = 0;
        };
        let text = document.createTextNode(message);
        snackbar.appendChild(text);
        if (actionText) {
            if (!action) {
                action = snackbar.dismiss.bind(snackbar);
            }
            let actionButton = document.createElement('button');
            actionButton.className = 'action';
            actionButton.innerHTML = actionText;
            actionButton.addEventListener('click', action);
            snackbar.appendChild(actionButton);
        }

        setTimeout(function () {
            if (previous === this) {
                previous.dismiss();
            }
        }.bind(snackbar), 1500);

        snackbar.addEventListener('transitionend', function (event, elapsed) {
            if (event.propertyName === 'opacity' && this.style.opacity === '0') {
                this.parentElement.removeChild(this);
                if (previous === this) {
                    previous = null;
                }
            }
        }.bind(snackbar));


        previous = snackbar;
        document.body.appendChild(snackbar);
        // In order for the animations to trigger, I have to force the original style to be computed, and then change it.
        getComputedStyle(snackbar).bottom;
        snackbar.style.opacity = 1;
    };
})();