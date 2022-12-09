
(function () {

    //new dialog.init(document.getElementById('confirm_dialog'), {});

    //$(document).ajaxStart(function () {

    //    $('#animacao_processo', document.body).addClass('ativo');

    //});

    //$(document).ajaxStop(function () {

    //    $('#animacao_processo', document.body).removeClass('ativo');

    //});

    //window.addEventListener('error', function () {

    //    document.getElementById('animation_process').classList.remove('active');

    //});

    //$(window).on('error', function () {

    //    $('#animacao_processo', document.body).removeClass('ativo');

    //});

    //window.addEventListener('submit', function () {

    //    document.getElementById('animation_process').classList.remove('active');

    //});

    //$(window).on('submit', function () {

    //    $('#animacao_processo', document.body).removeClass('ativo');

    //});

    //window.addEventListener('beforeunload', function () {

    //    document.getElementById('animation_process').classList.add('active');

    //});

    //$(window).bind('beforeunload', function () {

    //    $('#animacao_processo', document.body).addClass('ativo');

    //});

})();

function copyToClipboard(value) {

    let input = document.createElement('input');
    input.value = value;

    document.body.appendChild(input);

    input.select();
    input.setSelectionRange(0, 99999);

    document.execCommand('copy');

    input.parentNode.removeChild(input);
}