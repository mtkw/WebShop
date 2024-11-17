var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#productTable').DataTable({
        "ajax": {
            url: '/admin/product/getall'
        },
        "columns": [
            { data: 'name', "width": "10%" },
            { data: 'price', "width": "10%" },
            { data: 'currency', "width": "5%" },
            { data: 'description', "width": "30%" },
            {
                data: 'imgPath',
                "render": function (data) {
                    return `<img class="card-img-top" src="/img/${data}" alt="${data}" style="height: 100px;"/>`
                },
                "width": "10%"
            },
            { data: 'category.name', "width": "10%" },
            { data: 'supplier.name', "width": "10%" },
            { data: 'quantity', "width": "5%" },
            {
                data: 'id',
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group" >
                            <a onClick=Delete('/admin/product/delete?id=${data}') class="btn btn-outline-danger mx-2" >
                            <i class="bi bi-trash-fill"></i> Delete
                            </a >
                            <a href="/admin/product/upsert?id=${data}" class="btn btn-outline-success mx-2">
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
/*
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
});*/

