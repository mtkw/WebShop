var dataTable;

$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    dataTable = $('#userTable').DataTable({
        "ajax": {
            url: '/admin/user/getall',
            dataSrc: 'data'
        },
        scrollX: true,
        "columns": [
            { data: 'id', "width": "20%" },
            { data: 'name', "width": "20%" },
            { data: 'email', "width": "20%" },
            {
                data: 'id',
                "render": function (data) {
                    return `
                        <div class="btn-group" role="group">
                            <button class="btn btn-outline-danger block-button" data-id="${data}">Block</button>
                            <button class="btn btn-outline-success unblock-button" data-id="${data}">Unblock</button>
                            <a onClick=Delete('/admin/user/delete?id=${data}') class="btn btn-outline-danger mx-2">
                                <i class="bi bi-trash-fill"></i> Delete
                            </a>
                        </div>
                    `;
                },
                "width": "40%"
            },
            {
                data: 'isBlocked',
                "render": function (data) {
                    return data ? "Blocked" : "Active";
                },
                "width": "20%"
            }
        ]
    });
}



$(document).on('click', '.block-button', function () {
    var userId = $(this).data('id');
    $.ajax({
        url: '/Admin/User/BlockUser?id=' + userId,
        type: 'PATCH',
        data: JSON.stringify({ id: userId}), 
        contentType: 'application/json',
        success: function (response) {
            if (response.success) {
                toastr.success(response.message); 
                dataTable.ajax.reload(); 
            } else {
                toastr.error(response.message); 
            }
        },
        error: function () {
            toastr.error('An error occurred while processing your request.'); 
        }
    });
});

$(document).on('click', '.unblock-button', function () {
    var userId = $(this).data('id');
    $.ajax({
        url: '/Admin/User/UnblockUser?id=' + userId,
        type: 'PATCH',
        data: JSON.stringify({ id: userId}),
        contentType: 'application/json',
        success: function (response) {
            if (response.success) {
                toastr.success(response.message); 
                dataTable.ajax.reload(); 
            } else {
                toastr.error(response.message); 
            }
        },
        error: function () {
            toastr.error('An error occurred while processing your request.'); 
        }
    });
});


// Delete function with confirmation
function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "This action is irreversible!",
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
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            });
        }
    });
}


