var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#orderTable').DataTable({
        "ajax": {
            url: '/customer/order/getall'
        },
        scrollX: true,
        screenY: true,
        "columns": [
            { data: 'id', "width": "10%" },
            { data: 'orderDate', "width": "10%" },
            { data: 'shippingDate', "width": "5%" },
            { data: 'orderTotal', "width": "20%" },
            { data: 'orderStatus', "width": "10%" },
            { data: 'paymentStatus', "width": "10%" },
            { data: 'paymentDate', "width": "5%" },
            {
                data: 'orderStatus',
                "render": function (orderStatus,type, row) {
                    let cancelButton = '';
                    if (orderStatus != "Cancelled") {
                        cancelButton = `
                        <a onClick=Delete('/customer/order/cancel?id=${row.id}') class="btn btn-outline-danger mx-2" >
                            <i class="bi bi-trash-fill"></i> Cancel Order
                            </a >
                        `;
                    } else {
                        cancelButton = `
                        <a href="#" class="btn btn-outline-danger mx-2" >
                         <span class="tooltip">The order has already been canceled.</span>
                            <i class="bi bi-trash-fill"></i> Cancel Order
                            </a >
                        `;
                    }
                    return `
                        <div class="w-75 btn-group" role="group" >
                            ${cancelButton}
                            <a href="/customer/order/details?id=${row.id}" class="btn btn-outline-success mx-2">
                               <i class="bi bi-pencil-square"></i> Show Details
                            </a>
                        </div>
                    `
                },
                "width": "20%"
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
        confirmButtonText: 'Yes, Cancel Order!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'PATCH',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}


