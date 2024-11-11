
async function deleteProduct(id) {
    const response = await fetch(`/Admin/Category/${id}`,
        {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
            }
        });

    if (response.ok) {
        const result = await response.json();
        alert(result.message);
        window.location.href = '/Admin/Category';
    } else {
        const error = await response.json();
        alert(error.message);
    }
}