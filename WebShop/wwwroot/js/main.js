// Initialize popovers
document.querySelectorAll('[data-bs-toggle="popover"]')
    .forEach(popover => {
        new bootstrap.Popover(popover)
    })

document.getElementById("messageIconAnchor").addEventListener("click", test);

function test() {
    const element = document.getElementById("messageIconAnchor");
    const att = element.getAttribute("data-userEmail");
    getAllMessagesForUser(att);
}

function getAllMessagesForUser(userId) {
    var url = "/Customer/Message/GetAllMessageForUser";

    var data = { userId: userId };

    $.ajax({
        type: "GET",
        url: url,
        data: data,
        contentType: 'application/json',
        DataTransfer: 'json',
        success: function (data) {
            var modalBody = $('#messageModal .modal-body');
            modalBody.html('');
            $.each(data, function (index, item) {
                var collapseId = 'collapseElement' + index;
                modalBody.append(
                    `
                    <p class="d-flex gap-1">
                      <a class="btn btn-primary" data-bs-toggle="collapse" href="#${collapseId}" role="button" aria-expanded="false" aria-controls="collapseExample">
                        ${item.subject}
                      </a>
                    </p>
                    <div class="collapse" id="${collapseId}">
                      <div class="card card-body">
                        ${item.message}
                      </div>
                    </div>
                    `
                )

/*                if (item.isRead) {
                    modalBody.append('<div class="card mb-2"><div class="card-body"><h5 class="card-title">' + item.subject + '</h5><p class="card-text">' + item.message + '</p></div></div>');
                } else {
                    modalBody.append('<div class="card mb-2"><div class="card-body"><h5 class="card-title"><strong>' + item.subject + '</strong></h5><p class="card-text"><i>' + item.message + '</i></p></div></div>');
                }*/

            });
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });

}