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
            console.log(result);
            document.getElementById('userName').textContent = result.userName;


        },
        error: function (error) {
            console.log(error);
        },
    });
}