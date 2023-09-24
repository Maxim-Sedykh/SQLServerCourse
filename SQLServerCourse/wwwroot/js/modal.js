function openModal(parameters) {
    const url = parameters.url;
    const modalId = parameters.modalId;
    const modal = $('#' + modalId);

    if (url === undefined) {
        alert('Извиняемся.. Возникла ошибка!')
        return;
    }

    $.ajax({
        type: 'GET',
        url: url,
        success: function (response) {
            modal.find(".modal-body").html(response);
            modal.modal('show')
        },
        failure: function () {
            modal.modal('hide')
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
};