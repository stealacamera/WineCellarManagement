const baseUrl = 'WorkForce';

// Triggers toasts
const toasts = Array.from(document.getElementsByClassName('toast')).map(toast => new bootstrap.Toast(toast));

toasts.forEach(toast => toast.show());

// Sets delete functionality
const employeeListContainer = document.getElementById('employees-list');
const deleteBtns = employeeListContainer.querySelectorAll('button.btn-danger');

for (const btn of deleteBtns) {
    btn.addEventListener('click', e => {
        const id = e.target.dataset.id;

        remove(baseUrl + `/Delete/${id}`, () => document.getElementById(`e-${id}`).remove());
    })
}