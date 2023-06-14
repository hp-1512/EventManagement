$(function () {
    $('.nav-link').on('click', function (event) {
        event.preventDefault();
        $('.nav-link').removeClass('active');
        $(this).addClass('active');
        location.href = this.href;
    });
});
function gettingUserDetails() {
    $.ajax({
        url: "/Methods/LoggedUser",
        method: "GET",
        success: function (result) {
            //console.log(result);
            document.getElementById('userName').textContent = result.userName;


        },
        error: function (error) {
            console.log(error);
        },
    });
}
function gettingParticipatedEvents() {
    $.ajax({
        url: "/Home/ParticipatedEventsList",
        method: "GET",
        success: function (result) {
            //console.log(result);
            if (result.listOfEvents.length == 0) {
                $('#participatedEventsLink').addClass('d-none');
            }
            else {
                $('#participatedEventsLink').removeClass('d-none');
            }

        },
        error: function (error) {
            console.log(error);
        },
    });
}

function gettingCreatedEvents() {
    $.ajax({
        url: "/Home/CreatedEventsList",
        method: "GET",
        success: function (result) {
            //console.log(result);
            if (result.listOfEvents.length == 0) {
                $('#createdEventsLink').addClass('d-none');
            }
            else {
                $('#createdEventsLink').removeClass('d-none');
            }

        },
        error: function (error) {
            console.log(error);
        },
    });
}
