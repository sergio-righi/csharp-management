(function () {

    let element = document.getElementById('main_content');

    new dropzone.init(element, {

        parallel: 3,
        method: 'POST',
        multiple: true,
        removable: true,
        url: '/Drive/NewFile/',
        clickable: '#new_file'

    })

})();