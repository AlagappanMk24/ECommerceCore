document.addEventListener('DOMContentLoaded', function () { // Ensure DOM is fully loaded
    var recaptchaContainer = document.getElementById('recaptcha-container');
    var siteKey = recaptchaContainer.dataset.sitekey; // Get siteKey // Get siteKey from Razor
    grecaptcha.render('html_element', { 'sitekey': siteKey });

    var form = document.getElementById('contactForm'); // Get the form by ID

    form.addEventListener('submit', function (event) {  // Attach event listener
        var token = grecaptcha.getResponse();
        document.getElementById("recaptchaTokenInputId").value = token;

        if (!token) {
            event.preventDefault(); // Prevent form submission
            Swal.fire({
                title: 'Error!',
                text: 'Please complete the reCAPTCHA.',
                icon: 'error',
                confirmButtonText: 'OK'
            });
            return;
        }
    });
});