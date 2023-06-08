$(function () {
    $('.nav-link').on('click', function (event) {
        event.preventDefault();
        $('.nav-link').removeClass('active');
        $(this).addClass('active');
        location.href = this.href;
    });
});