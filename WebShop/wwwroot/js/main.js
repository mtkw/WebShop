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
                        <div class="message-container">
                            <div class="message-item mb-3">
                                <p class="d-flex gap-1">
                                    <a class="btn btn-primary" data-bs-toggle="collapse" href="#${collapseId}" role="button" aria-expanded="false" aria-controls="collapseExample1">
                                        ${item.subject}
                                    </a>
                                </p>
                                <div class="collapse" id="${collapseId}">
                                    <div class="card card-body">
                                        <p><strong>Message:</strong>${item.message}</p>
                                        <p><strong>Created On:</strong> <span class="text-muted">${item.createDate}</span> at <span class="text-muted">${item.createTime}</span></p>
                                        <p><strong>Status:</strong> <span class="badge ${item.isRead == 0 ? 'bg-danger' : 'bg-success'}">${item.isRead == 0 ? 'Unread' : 'Read'}</span></p>
                                        <button class="btn btn-success" onclick="markAsRead(1)">Confirm as Read</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    `
                )
            });
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });

}