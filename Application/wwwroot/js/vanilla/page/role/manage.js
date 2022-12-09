
let listRoleResult = null,
    listRoleMessage = null,
    feedbackMessage = null;

(function () {

    listRoleResult = document.getElementById('list_result');

    listRoleMessage = document.getElementById('list_message');

    feedbackMessage = document.getElementById('feedback_message');

})();

function addPersonSuccess(response) {

    if (response.isSuccess) {

        feedbackMessage.innerHTML = '';

        listRoleMessage.classList.add('d-none');

        if (listRoleResult.children.length === 0) {

            listRoleResult.classList.remove('d-none');
        }

        listRoleResult.innerHTML = response.content + listRoleResult.innerHTML;

    } else {

        feedbackMessage.innerHTML = response.feedback;
    }
}