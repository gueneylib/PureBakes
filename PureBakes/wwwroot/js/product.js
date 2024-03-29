function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    console.log('success function called with data:', data);
                    console.log('controller called with url:', url);

                    if(data.success)
                    {
                        toastr.success(data.message);
                    }
                    else
                    {
                        toastr.error(data.message);
                    }

                    // Todo find a better solution
                    setTimeout(function() {
                        location.reload();
                    }, 2000);
                }
            })
        }
    })
}