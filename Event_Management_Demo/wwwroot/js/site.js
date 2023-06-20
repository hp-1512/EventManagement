// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('.card-text').on('click', function () {
    if ($(this).css('overflow-y') == 'scroll') {
        $(this).scrollTop(0);
        $(this).css({
            'height': 'auto', '-webkit-line-clamp': '2', 'overflow-y': 'hidden'
        });

    }
    else {
        $(this).css({
            'height': '48px', '-webkit-line-clamp': '', 'overflow-y': 'scroll'
        });
    }
})
$('.card-title').on('click', function () {
    if ($(this).css('overflow-y') == 'scroll') {
        $(this).scrollTop(0);
        $(this).css({
            'height': 'auto', '-webkit-line-clamp': '2', 'overflow-y': 'hidden'
        });

    }
    else {
        $(this).css({
            'height': '25px', '-webkit-line-clamp': '', 'overflow-y': 'scroll'
        });
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
        $(this).css({ 'overflow': 'hidden', 'text-overflow': 'ellipsis' });

    }
    else {
        $(this).css({'overflow': 'scroll', 'text-overflow': ''});
    }
})




// notification

//function isRead(NotallId) {
//    if (NotallId > 0) {
//        $.ajax({
//            type: 'POST',
//            url: '/Notification/MarkNotAsRead',
//            data: { "notId": NotallId },
//            success: function () {
//                $('#notifType').text('');
//                $('#notifList').text('');
//                $('#notifListOld').text('');
//                loadNotification();
//            },
//            error: function () {
//                console.log('error');
//            },
//        });
//    }
//}

function loadNotification() {
    $.ajax({
        type: 'GET',
        url: '/Methods/NotifiactionData',
        success: function (data) {
            $('#notifCount').text(data.length);
            $.each(data, function (index, notifType) {
                $('#notifList').append(`<tr class="border-1 d-flex align-items-center" style="height: 60px;">
                                                                            <td style="width: 10%;"><i class="bi bi-chat-text fs-5"></i></td>
                                                                                <td style="width: 80%;">`+notifType+`</td>
                                                                </tr>
                                                         `);
                //$('#notifList').append(`<li class="form-check mb-2 p-0 d-flex align-items-center justify-content-between">
                //                                                        <label class="form-check-label ms-3" for="">`+ notifType + `</label>
                //                                                </li>
                //                                 `);
            });
        },
        error: function () {
            console.log('error');
        },
    });
}
