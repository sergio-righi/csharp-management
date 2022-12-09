
let countdown,
    countdownDay,
    countdownHour,
    countdownMinute,
    countdownSecond;

(function () {

    countdown = document.getElementById('countdown');

    countdownDay = document.getElementById('days');
    countdownHour = document.getElementById('hours');
    countdownMinute = document.getElementById('minutes');
    countdownSecond = document.getElementById('seconds');

    startCountdown(countdown.dataset.value);

})();

function startCountdown(value) {

    let remaining = value;
    let offset = getTime(remaining);

    if (offset.day < 1) countdownDay.classList.add('d-none');
    if (offset.hour < 1) countdownHour.classList.add('d-none');

    let interval = setInterval(function () {

        offset = getTime(--remaining);

        setValue(offset);

        if (remaining < 0) {
            clearInterval(interval);
            window.location.href = '/';
        }

    }, 1000);
}

function getTime(second) {

    let day = second / (24 * 3600);
    second = second % (24 * 3600);

    let hour = second / 3600;
    second %= 3600;

    let minute = second / 60;
    second %= 60;

    return {
        day: Math.floor(day),
        hour: Math.floor(hour),
        minute: Math.floor(minute),
        second: Math.floor(second)
    };
}

function setValue(offset) {

    countdownDay.innerText = offset.day.toTwoDigitFormat();
    countdownHour.innerText = offset.hour.toTwoDigitFormat();
    countdownMinute.innerText = offset.minute.toTwoDigitFormat();
    countdownSecond.innerText = offset.second.toTwoDigitFormat();
}