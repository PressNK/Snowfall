import { Modal } from 'bootstrap';

document.addEventListener('htmx:beforeSwap', function (event) {
    if (event.detail.target.id === "app-modal-manager") {
        let existingModal = document.getElementById('app-modal');
        if (existingModal) existingModal.remove();
    }
});

document.addEventListener('htmx:afterSwap', function (event) {
    if (event.detail.target.id === "app-modal-manager") {
        let modalElement = document.getElementById('app-modal');
        let modal = new Modal(modalElement);
        modal.show();
    }
});