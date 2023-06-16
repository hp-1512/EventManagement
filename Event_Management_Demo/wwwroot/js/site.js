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




function shareEvent(eventId) {
    currentUrl = window.origin+ "/Methods/EventInvitation?eventId=" + eventId;
    const shareData = {
        title: "EventAlchemy",
        text: "Greetings From EventAlchemy!!!",
        url: currentUrl,
    };
    try {
         navigator.share(shareData);
        resultPara.textContent = "Event shared successfully";
    } catch (err) {
        resultPara.textContent = `Error: ${err}`;
    }
};



$('.eventDesc').on('click', function () {
    if ($(this).css('overflow') == 'scroll') {
        $(this).scrollLeft(0);
        $(this).css('overflow', 'hidden');
        $(this).css('text-overflow', 'ellipsis');

    }
    else {
        $(this).css('overflow', 'scroll');
        $(this).css('text-overflow', '');
    }
})