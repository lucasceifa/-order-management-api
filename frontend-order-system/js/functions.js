const showToast = (message, isError = false) => {
    const toastId = 'toast-' + Date.now();
    const toastHeaderClass = isError ? 'bg-danger text-white' : 'bg-success text-white';
    const toastHtml = `
        <div id="${toastId}" class="toast" role="alert" aria-live="assertive" aria-atomic="true">
            <div class="toast-header ${toastHeaderClass}">
                <strong class="me-auto">${isError ? 'Error' : 'Success'}</strong>
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
            <div class="toast-body">
                ${message}
            </div>
        </div>`;
    
    $('.toast-container').append(toastHtml);
    
    const toastElement = new bootstrap.Toast($(`#${toastId}`)[0], { autohide: true, delay: 5000 });
    toastElement.show();

    $(`#${toastId}`).on('hidden.bs.toast', function () {
        $(this).remove();
    });
};