
let feedbackMessage = null,
    manageAttendance = null,
    manageAttendanceForm = null,
    manageAttendanceFeedback = null;

(function () {

    feedbackMessage = document.getElementById('feedback_message');
    manageAttendance = document.getElementById('manage_attendance');

    new modal.init(manageAttendance, {

        onClose: resetInformation,
        beforeOpen: searchAttendance

    });

})();

/*
 * --- Search attendance
 */

function searchAttendance() {

    let plugin = new modal.get(manageAttendance);

    if (plugin !== null) {

        let formData = new FormData();

        formData.append('id', plugin.trigger.dataset.id);
        formData.append('date', plugin.trigger.dataset.date);

        let xhr = new XMLHttpRequest();

        xhr.open('POST', '/Attendance/SearchAttendance/');
        xhr.onload = function () {

            if (xhr.status === 200) {

                let json = JSON.parse(xhr.response);

                if (json.message !== undefined) {

                    plugin.setHeader(json.header);
                    plugin.setContent(json.content);

                    manageAttendanceForm = document.getElementById('manage_attendance_form');
                    manageAttendanceFeedback = document.getElementById('manage_attendance_feedback');
                }
            }
        };

        feedbackMessage.innerHTML = '';

        xhr.send(formData);
    }
}

/*
 * --- Reset information
 */

function resetInformation() {

    manageAttendanceFeedback.innerHTML = '';
}

/*
 * --- Manage attendance response
 */

function manageAttendanceSuccess(response) {

    if (response.success) {

        let plugin = new modal.get(manageAttendance);

        manageAttendanceForm.reset();

        feedbackMessage.innerHTML = response.feedback;

        plugin.close();

    } else {

        manageAttendanceFeedback.innerHTML = response.feedback;
    }
}