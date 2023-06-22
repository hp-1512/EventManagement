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


//-------------------------Share an Event------------------------

function shareEvent(eventId) {
    currentUrl = window.origin + "/Methods/EventInvitation?eventId=" + eventId;
    const shareData = {
        title: "EventAlchemy",
        text: "Greetings From EventAlchemy!!!",
        url: currentUrl,
    };
    try {
        navigator.share(shareData);
    } catch (err) {
        //resultPara.textContent = `Error: ${err}`;
    }
};

//-------------------------Making Description Horizontally Scrollable on Created Events Page------------------------

$('.eventDesc').on('click', function () {
    if ($(this).css('overflow') == 'scroll') {
        $(this).scrollLeft(0);
        $(this).css({ 'overflow': 'hidden', 'text-overflow': 'ellipsis' });

    }
    else {
        $(this).css({ 'overflow': 'scroll', 'text-overflow': '' });
    }
})

//-------------------------Notifications------------------------

function loadNotification() {
    $.ajax({
        type: 'GET',
        url: '/Methods/NotifiactionData',
        success: function (data) {
            $('#notifCount').text(data.notificationCout);
            if (data.notificationData.length == 0) {
                $('#clear-all-notification').attr('disabled', true);
                $('#notifList').append(`<tr class="border-1 d-flex align-items-center" style="height: 60px;">
                                                                                <td style="width: 100%; text-align:center;">No any Notifications for Now...</td>
                                                                </tr>
                                                         `);

            }
            else {
                $.each(data.notificationData, function (index, notifData) {
                    $('#notifList').append(`<tr class="border-1 d-flex align-items-center" style="height: 60px;">
                                                                            <td style="width: 10%;"><i class="bi bi-chat-text fs-5"></i></td>
                                                                                <td style="width: 80%;">`+ notifData.notifMsg + `</td>
                                                                            <td class="text-end" id="markAsReadIcon`+ notifData.notifId + `" style="width: 10%;"></td>
                                                                </tr>
                                                         `);

                    if (notifData.isRead == 0) {
                        $("#markAsReadIcon" + notifData.notifId).append(`<button class="border-0 bg-transparent" onclick="isRead(` + notifData.notifId + `)">
                                                                            <i class="bi bi-check-circle-fill fs-5" style="color: #F88634;"></i>
                                                                             </button>`);
                    }
                    else {
                        $('#markAsReadIcon' + notifData.notifId).append(`<i class="bi bi-check-circle-fill fs-5" style="color: #b0b0b0;"></i>`);
                    }
                });
            }
        },
        error: function () {
            console.log('error');
        },
    });
}

function isRead(NotallId) {
    if (NotallId > 0) {
        $.ajax({
            type: 'POST',
            url: '/Methods/MarkNotifAsRead',
            data: { "notiId": NotallId },
            success: function (result) {
                if (result) {
                    $('#notifList').text('');
                    loadNotification();
                }
            },
            error: function () {
                console.log('error');
            },
        });
    }
}

function clearAllNotification() {
    $.ajax({
        type: 'GET',
        url: '/Methods/ClearAllNotification',
        success: function () {
            $('#notifList').text('');
            loadNotification();
        },
        error: function () {
            console.log('error');
        },
    });
}