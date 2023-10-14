const password = document.getElementById('Password');
const passwordVisibilityBtn = document.getElementById('password-visibility');

passwordVisibilityBtn.addEventListener('click', e => {
    e.preventDefault();
    const icon = e.currentTarget.firstChild;

    if (password.type == 'password') {
        icon.classList.replace('fa-eye', 'fa-eye-slash');
        password.type = 'text';
    } else {
        icon.classList.replace('fa-eye-slash', 'fa-eye');
        password.type = 'password';
    }
})