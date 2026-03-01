import { Toast } from 'bootstrap';

document.addEventListener("DOMContentLoaded", function () {
    let toastEl = document.getElementById("flash-message");
    if (toastEl) {
        let toast = new Toast(toastEl, { delay: 5000 });
        toast.show();
    }
});