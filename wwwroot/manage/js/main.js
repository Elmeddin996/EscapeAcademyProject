$(document).on("click", ".delete-btn", function (e) {
    e.preventDefault();
    let url = $(this).attr("href");

    Swal.fire({
        title: 'Silmək istədiyinizdən əminsiniz?',
        text: "Bunu geri qaytara bilməyəcəksiniz!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        cancelButtonText: 'Geri',
        confirmButtonText: 'Hə, Sil!'
    }).then((result) => {
        if (result.isConfirmed) {

            fetch(url).then(response => {
                if (response.ok) {
                    Swal.fire(
                        'Silindi!',
                        'Silmək istədiyiniz məlumatlar silindi.',
                        'success'
                    ).then(() => location.reload())
                }
                else {
                    Swal.fire(
                        'Xəta!',
                        'error'
                    )
                }
            });

            
        }
    })
 })

