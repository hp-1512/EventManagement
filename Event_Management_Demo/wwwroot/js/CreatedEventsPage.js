function deleteEvent(eventId) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: false
    }).then((result) => {
        if (result.isConfirmed) {
            const { value: text } = Swal.fire({
                input: 'textarea',
                inputLabel: 'Reason To Delete the Event',
                inputPlaceholder: 'Type your reason here...',
                inputAttributes: {
                    'aria-label': 'Type your reason here',
                    required: 'true',
                },
                showCancelButton: true
            }).then((text) => {
                if (text.isConfirmed) {
                    $.ajax({
                        url: "/Home/DeleteEvent",
                        method: "GET",
                        data: {
                            'eventId': eventId,
                            'resasonToDelete': text.value,
                        },
                        success: function (result) {
                            console.log(result);
                            document.getElementById('createdBy').value = result.userId;
                            document.getElementById('createdByForDisplay').value = result.userName;


                        },
                        error: function (error) {
                            console.log(error);
                        },
                    });
                }
            })
        }
    });
}