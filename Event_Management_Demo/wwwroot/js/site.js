// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('.card-text').on('click', function () {
    if ($(this).css('overflow-y') == 'scroll') {
        $(this).scrollTop(0);
        $(this).css({
            'height': 'auto', '-webkit-line-clamp': '2'
        });
        $(this).css('overflow-y', 'hidden');

    }
    else {
        $(this).css({
            'height': '48px', '-webkit-line-clamp': ''
        });
        $(this).css('overflow-y', 'scroll');
    }
})
$('.card-title').on('click', function () {
    if ($(this).css('overflow-y') == 'scroll') {
        $(this).scrollTop(0);
        $(this).css({
            'height': 'auto', '-webkit-line-clamp': '2'
        });
        $(this).css('overflow-y', 'hidden');

    }
    else {
        $(this).css({
            'height': '25px', '-webkit-line-clamp': ''
        });
        $(this).css('overflow-y', 'scroll');
    }
})