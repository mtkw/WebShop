var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#categoryTable').DataTable({
        "ajax": {
            url: '/admin/category/getall'
        },
        "columns": [
            { data: 'id', "width": "30%" },
            { data: 'name', "width": "50%" },
            {
                data: 'id',
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group" >
                            <a onClick=Delete('/admin/category/delete?id=${data}') class="btn btn-outline-danger mx-2" >
                            <i class="bi bi-trash-fill"></i> Delete
                            </a >
                            <a href="/admin/category/edit?id=${data}" class="btn btn-outline-success mx-2">
                               <i class="bi bi-pencil-square"></i> Edit
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

document.getElementById('updateCategoryForm').addEventListener('submit', async function (event) {
    event.preventDefault(); // Prevent the default form submission
    const productId = document.getElementById('productCategoryId').value;
    const productName = document.getElementById('categoryName').value;

    const updatedProduct = {
        id: productId,
        name: productName
    };
    try {
        const response = await fetch(`/admin/category/edit?id=${productId}"`, {
            method: 'PATCH', // or 'PUT' for complete updates
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(updatedProduct),
        });

        if (!response.ok) {
            throw new Error(`Error: ${response.status} ${response.statusText}`);
        }

        const result = await response.json();
        console.log('Product updated successfully:', result);
        window.location.href = "https://localhost:7194/admin/category/";
    } catch (error) {
        console.error('Error updating product:', error);

    }
});

