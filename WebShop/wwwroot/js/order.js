var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#orderTable').DataTable({
        "ajax": {
            url: '/customer/order/getall'
        },
        "columns": [
            { data: 'id', "width": "10%" },
            { data: 'orderDate', "width": "10%" },
            { data: 'shippingDate', "width": "5%" },
            { data: 'orderTotal', "width": "30%" },
            { data: 'orderStatus', "width": "10%" },
            { data: 'paymentStatus', "width": "10%" },
            { data: 'paymentDate', "width": "5%" },
            {
                data: 'id',
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group" >
                            <a onClick=Delete('/customer/order/delete?id=${data}') class="btn btn-outline-danger mx-2" >
                            <i class="bi bi-trash-fill"></i> Delete
                            </a >
                            <a href="/customer/order/upsert?id=${data}" class="btn btn-outline-success mx-2">
                               <i class="bi bi-pencil-square"></i> Edit
                            </a>
                        </div>
                    `
                },
                "width": "5%"
            }
        ]
    });
}

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
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}
