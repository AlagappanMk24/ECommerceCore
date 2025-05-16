// Phone number validation patterns for each country,
const phonePatterns = {
    US: { regex: /^\+1[2-9]\d{2}[2-9](?!11)\d{6}$/, example: '+12125551212' },
    IN: { regex: /^\+91[6-9]\d{9}$/, example: '+919876543210' },
    CA: { regex: /^\+1[2-9]\d{2}[2-9](?!11)\d{6}$/, example: '+12125551212' },
    AU: { regex: /^\+614\d{8}$/, example: '+61412345678' },
    UK: { regex: /^\+447\d{9}$/, example: '+447912345678' },
    RU: { regex: /^\+7[3489]\d{9}$/, example: '+79123456789' },
    CN: { regex: /^\+86(1[3-9]\d{9}|[2-9]\d{1,2}\d{7,8})$/, example: '+8613812345678' }
};

// Country code prefixes
const countryPrefixes = {
    US: '+1',
    CA: '+1',
    IN: '+91',
    AU: '+61',
    UK: '+44',
    RU: '+7',
    CN: '+86'
};

// Helper function to set error
const setError = (input, errorElementId, message) => {
    if (input && document.getElementById(errorElementId)) {
        document.getElementById(errorElementId).textContent = message;
        input.classList.add('is-invalid');
        return false;
    }
    return true;
};

// Helper function to clear error
const clearError = (input, errorElementId) => {
    if (input && document.getElementById(errorElementId)) {
        document.getElementById(errorElementId).textContent = '';
        input.classList.remove('is-invalid');
    }
};

// Format and validate phone number based on country
function validatePhoneNumber(phoneNumber, countryCode) {
    if (!phoneNumber) {
        return { isValid: false, message: 'Phone number is required' };
    }
    // Strip all non-digits except the leading +
    let digitsOnly = phoneNumber.replace(/[^\d+]/g, '');
    console.log(`Initial digitsOnly: ${digitsOnly}`);

    const prefix = countryPrefixes[countryCode];

    // If the number doesn't start with the correct prefix, reformat it
    if (!digitsOnly.startsWith(prefix)) {
        // Remove any existing prefix and prepend the correct one
        digitsOnly = digitsOnly.replace(/^\+\d*/, '');
        console.log(`After stripping prefix: ${digitsOnly}`);
        // Extract only the digits needed for the country
        let localNumber = digitsOnly.replace(/\D/g, '');
        // Adjust length based on country requirements
        if (countryCode === 'IN') {
            localNumber = localNumber.slice(0, 10); // India: 10 digits
        } else if (countryCode === 'US' || countryCode === 'CA') {
            localNumber = localNumber.slice(0, 10); // US/CA: 10 digits
        } else if (countryCode === 'AU') {
            localNumber = localNumber.slice(0, 9); // AU: 9 digits
        } else if (countryCode === 'UK') {
            localNumber = localNumber.slice(0, 10); // UK: 10 digits
        } else if (countryCode === 'RU') {
            localNumber = localNumber.slice(0, 10); // RU: 10 digits
        } else if (countryCode === 'CN') {
            localNumber = localNumber.slice(0, 11); // CN: typically 11 digits for mobile
        }
        digitsOnly = prefix + localNumber;
    }

    console.log(`Final digitsOnly: ${digitsOnly}`);

    const pattern = phonePatterns[countryCode];
    if (!pattern || !pattern.regex.test(digitsOnly)) {
        return {
            isValid: false,
            message: `Invalid phone number format. Example: ${pattern?.example || 'unknown'}`
        };
    }

    return { isValid: true, formattedNumber: digitsOnly };
}

// Email validation, aligned with backend
function validateEmail(email) {
    if (!email) {
        return { isValid: false, message: 'Email is required' };
    }

    const normalizedEmail = email.trim().toLowerCase();
    const emailRegex = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$/;
    if (!emailRegex.test(normalizedEmail)) {
        return { isValid: false, message: 'Invalid email format' };
    }

    const allowedDomains = ['gmail.com', 'yahoo.com', 'outlook.com', 'icloud.com',
        'company.com', 'edu.in', 'mail.ru', 'qq.com', '163.com'];
    const domain = normalizedEmail.split('@')[1];
    if (!allowedDomains.includes(domain)) {
        return { isValid: false, message: `We don't accept emails from ${domain}` };
    }

    return { isValid: true };
}

// Name validation
function validateName(name) {
    if (!name) {
        return { isValid: false, message: 'Name is required' };
    }
    if (!/^[a-zA-Z\s\.'-]{2,}$/.test(name)) {
        return { isValid: false, message: 'Name should only contain letters, spaces, dots, apostrophes, or hyphens' };
    }
    return { isValid: true };
}

// Address Line 1 validation
function validateAddress1(address1) {
    if (!address1) {
        return { isValid: false, message: 'Address Line 1 is required' };
    }
    return { isValid: true };
}

// City validation
function validateCity(city) {
    if (!city) {
        return { isValid: false, message: 'City is required' };
    }
    return { isValid: true };
}

// State validation
function validateState(state) {
    if (!state) {
        return { isValid: false, message: 'State is required' };
    }
    return { isValid: true };
}

// Postal Code validation
function validatePostalCode(postalCode) {
    if (!postalCode) {
        return { isValid: false, message: 'Postal Code is required' };
    }
    return { isValid: true };
}

// Company validation
function validateCompany(company) {
    if (!company) {
        return { isValid: false, message: 'Please select a company' };
    }
    return { isValid: true };
}

// Password validation
function validatePassword(password) {
    if (!password) {
        return { isValid: false, message: 'Password is required' };
    }
    return { isValid: true };
}

// Confirm Password validation
function validateConfirmPassword(confirmPassword, password) {
    if (!confirmPassword) {
        return { isValid: false, message: 'Confirm Password is required' };
    }
    if (confirmPassword !== password) {
        return { isValid: false, message: 'Passwords do not match' };
    }
    return { isValid: true };
}

// Form validation
const form = document.getElementById('upsertForm');
if (form) {
    form.addEventListener('submit', function (e) {
        e.preventDefault();
        let isValid = true;

        // Reset previous errors
        document.querySelectorAll('.invalid-feedback').forEach(el => el.textContent = '');
        document.querySelectorAll('.is-invalid').forEach(el => el.classList.remove('is-invalid'));

        // Cache DOM elements
        const elements = {
            name: document.getElementById('Name'),
            email: document.getElementById('Email'),
            phone: document.getElementById('PhoneNumber'),
            country: document.getElementById('CountryCode'),
            address1: document.getElementById('Address1'),
            city: document.getElementById('City'),
            state: document.getElementById('State'),
            postalCode: document.getElementById('PostalCode'),
            company: document.getElementById('CompanyId'),
            password: document.getElementById('Password'),
            confirmPassword: document.getElementById('ConfirmPassword'),
            phoneGroup: document.getElementById('PhoneNumber')?.closest('.input-group')
        };

        // Validate name
        if (elements.name) {
            const name = elements.name.value.trim();
            const nameValidation = validateName(name);
            if (!nameValidation.isValid) {
                isValid = setError(elements.name, 'name-error', nameValidation.message) && isValid;
            }
        }

        // Validate email
        if (elements.email) {
            const email = elements.email.value.trim();
            const emailValidation = validateEmail(email);
            if (!emailValidation.isValid) {
                isValid = setError(elements.email, 'email-error', emailValidation.message) && isValid;
            }
        }

        // Validate phone number
        if (elements.phone && elements.country && elements.phoneGroup) {
            const phoneNumber = elements.phone.value.trim();
            const countryCode = elements.country.value.toUpperCase();
            const phoneValidation = validatePhoneNumber(phoneNumber, countryCode);
            if (!phoneValidation.isValid) {
                document.getElementById('phone-error').textContent = phoneValidation.message;
                elements.phoneGroup.classList.add('is-invalid');
                isValid = false;
            } else {
                document.getElementById('phone-error').textContent = '';
                elements.phoneGroup.classList.remove('is-invalid');
                elements.phone.value = phoneValidation.formattedNumber;
            }
        }

        // Validate Address Line 1
        if (elements.address1) {
            const address1 = elements.address1.value.trim();
            const address1Validation = validateAddress1(address1);
            if (!address1Validation.isValid) {
                isValid = setError(elements.address1, 'address1-error', address1Validation.message) && isValid;
            }
        }

        // Validate City
        if (elements.city) {
            const city = elements.city.value.trim();
            const cityValidation = validateCity(city);
            if (!cityValidation.isValid) {
                isValid = setError(elements.city, 'city-error', cityValidation.message) && isValid;
            }
        }

        // Validate State
        if (elements.state) {
            const state = elements.state.value.trim();
            const stateValidation = validateState(state);
            if (!stateValidation.isValid) {
                isValid = setError(elements.state, 'state-error', stateValidation.message) && isValid;
            }
        }

        // Validate Postal Code
        if (elements.postalCode) {
            const postalCode = elements.postalCode.value.trim();
            const postalCodeValidation = validatePostalCode(postalCode);
            if (!postalCodeValidation.isValid) {
                isValid = setError(elements.postalCode, 'postalcode-error', postalCodeValidation.message) && isValid;
            }
        }

        // Validate Company
        if (elements.company) {
            const company = elements.company.value.trim();
            const companyValidation = validateCompany(company);
            if (!companyValidation.isValid) {
                isValid = setError(elements.company, 'company-error', companyValidation.message) && isValid;
            }
        }

        // Validate Password
        if (elements.password) {
            const password = elements.password.value.trim();
            const passwordValidation = validatePassword(password);
            if (!passwordValidation.isValid) {
                isValid = setError(elements.password, 'password-error', passwordValidation.message) && isValid;
            }
        }

        // Validate Confirm Password
        if (elements.confirmPassword && elements.password) {
            const confirmPassword = elements.confirmPassword.value.trim();
            const password = elements.password.value.trim();
            const confirmPasswordValidation = validateConfirmPassword(confirmPassword, password);
            if (!confirmPasswordValidation.isValid) {
                isValid = setError(elements.confirmPassword, 'confirmpassword-error', confirmPasswordValidation.message) && isValid;
            }
        }

        if (isValid) {
            this.submit();
        }
    });
} else {
    console.error('Form with id "upsertForm" not found');
}

// Live validation for phone as user types
const phoneInput = document.getElementById('PhoneNumber');
const phoneGroup = phoneInput?.closest('.input-group');
if (phoneInput && phoneGroup) {
    phoneInput.addEventListener('blur', function () {
        const phoneNumber = this.value.trim();
        const countrySelect = document.getElementById('CountryCode');
        if (!countrySelect) {
            console.error('CountryCode select not found');
            return;
        }
        const countryCode = countrySelect.value.toUpperCase();

        const validation = validatePhoneNumber(phoneNumber, countryCode);
        if (!validation.isValid) {
            document.getElementById('phone-error').textContent = validation.message;
            phoneGroup.classList.add('is-invalid');
        } else {
            document.getElementById('phone-error').textContent = '';
            phoneGroup.classList.remove('is-invalid');
            if (validation.formattedNumber) {
                this.value = validation.formattedNumber;
            }
        }
    });
} else {
    console.error('Element with id "PhoneNumber" or its input-group not found');
}

// Update validation when country changes
const countrySelect = document.getElementById('CountryCode');
if (countrySelect) {
    countrySelect.addEventListener('change', function () {
        if (phoneInput) {
            phoneInput.dispatchEvent(new Event('blur'));
        }
        // Update format hint immediately
        const formatHints = {
            US: 'Format: +12125551212',
            IN: 'Format: +919876543210',
            UK: 'Format: +447912345678',
            AU: 'Format: +61412345678',
            CA: 'Format: +12125551212',
            RU: 'Format: +79123456789',
            CN: 'Format: +8613812345678'
        };
        const hint = formatHints[this.value.toUpperCase()] || 'Enter international format';
        const phoneHint = document.querySelector('[data-phone-hint]');
        if (phoneHint) {
            phoneHint.textContent = hint;
        } else {
            console.warn('Phone hint element with [data-phone-hint] not found');
        }
    });
} else {
    console.error('Element with id "CountryCode" not found');
}

// Live validation for Name
const nameInput = document.getElementById('Name');
if (nameInput) {
    nameInput.addEventListener('input', function () {
        const name = this.value.trim();
        const validation = validateName(name);
        if (!validation.isValid) {
            setError(this, 'name-error', validation.message);
        } else {
            clearError(this, 'name-error');
        }
    });
} else {
    console.error('Element with id "Name" not found');
}

// Live validation for Address Line 1
const address1Input = document.getElementById('Address1');
if (address1Input) {
    address1Input.addEventListener('input', function () {
        const address1 = this.value.trim();
        const validation = validateAddress1(address1);
        if (!validation.isValid) {
            setError(this, 'address1-error', validation.message);
        } else {
            clearError(this, 'address1-error');
        }
    });
} else {
    console.error('Element with id "Address1" not found');
}

// Live validation for City
const cityInput = document.getElementById('City');
if (cityInput) {
    cityInput.addEventListener('input', function () {
        const city = this.value.trim();
        const validation = validateCity(city);
        if (!validation.isValid) {
            setError(this, 'city-error', validation.message);
        } else {
            clearError(this, 'city-error');
        }
    });
} else {
    console.error('Element with id "City" not found');
}

// Live validation for State
const stateInput = document.getElementById('State');
if (stateInput) {
    stateInput.addEventListener('input', function () {
        const state = this.value.trim();
        const validation = validateState(state);
        if (!validation.isValid) {
            setError(this, 'state-error', validation.message);
        } else {
            clearError(this, 'state-error');
        }
    });
} else {
    console.error('Element with id "State" not found');
}

// Live validation for Postal Code
const postalCodeInput = document.getElementById('PostalCode');
if (postalCodeInput) {
    postalCodeInput.addEventListener('input', function () {
        const postalCode = this.value.trim();
        const validation = validatePostalCode(postalCode);
        if (!validation.isValid) {
            setError(this, 'postalcode-error', validation.message);
        } else {
            clearError(this, 'postalcode-error');
        }
    });
} else {
    console.error('Element with id "PostalCode" not found');
}

// Live validation for Company
const companyInput = document.getElementById('CompanyId');
if (companyInput) {
    companyInput.addEventListener('change', function () {
        const company = this.value.trim();
        const validation = validateCompany(company);
        if (!validation.isValid) {
            setError(this, 'company-error', validation.message);
        } else {
            clearError(this, 'company-error');
        }
    });
} else {
    console.error('Element with id "CompanyId" not found');
}

// Live validation for Password
const passwordInput = document.getElementById('Password');
if (passwordInput) {
    passwordInput.addEventListener('input', function () {
        const password = this.value.trim();
        const validation = validatePassword(password);
        if (!validation.isValid) {
            setError(this, 'password-error', validation.message);
        } else {
            clearError(this, 'password-error');
        }

        // Re-validate Confirm Password if it exists
        const confirmPasswordInput = document.getElementById('ConfirmPassword');
        if (confirmPasswordInput && confirmPasswordInput.value.trim()) {
            confirmPasswordInput.dispatchEvent(new Event('input'));
        }
    });
} else {
    console.error('Element with id "Password" not found');
}

// Live validation for Confirm Password
const confirmPasswordInput = document.getElementById('ConfirmPassword');
if (confirmPasswordInput && passwordInput) {
    confirmPasswordInput.addEventListener('input', function () {
        const confirmPassword = this.value.trim();
        const password = passwordInput.value.trim();
        const validation = validateConfirmPassword(confirmPassword, password);
        if (!validation.isValid) {
            setError(this, 'confirmpassword-error', validation.message);
        } else {
            clearError(this, 'confirmpassword-error');
        }
    });
} else {
    console.error('Element with id "ConfirmPassword" or "Password" not found');
}
// Live validation for email
const emailInput = document.getElementById('Email');
if (emailInput) {
    emailInput.addEventListener('input', function () {
        const emailErrorSpan = document.querySelector('[data-valmsg-for="Email"]');
        if (emailErrorSpan) {
            emailErrorSpan.textContent = '';
        }
        const email = this.value.trim();
        const validation = validateEmail(email);
        if (!validation.isValid) {
            document.getElementById('email-error').textContent = validation.message;
            this.classList.add('is-invalid');
        } else {
            document.getElementById('email-error').textContent = '';
            this.classList.remove('is-invalid');
        }
    });
} else {
    console.error('Element with id "Email" not found');
}

// Toggle password visibility
const togglePassword = document.getElementById('togglePassword');
if (togglePassword) {
    togglePassword.addEventListener('click', function () {
        const passwordInput = document.getElementById('Password');
        const icon = this.querySelector('i');
        if (passwordInput && icon) {
            if (passwordInput.type === 'password') {
                passwordInput.type = 'text';
                icon.classList.remove('bi-eye');
                icon.classList.add('bi-eye-slash');
            } else {
                passwordInput.type = 'password';
                icon.classList.remove('bi-eye-slash');
                icon.classList.add('bi-eye');
            }
        }
    });
} else {
    console.error('Element with id "togglePassword" not found');
}

const toggleConfirmPassword = document.getElementById('toggleConfirmPassword');
if (toggleConfirmPassword) {
    toggleConfirmPassword.addEventListener('click', function () {
        const confirmPasswordInput = document.getElementById('ConfirmPassword');
        const icon = this.querySelector('i');
        if (confirmPasswordInput && icon) {
            if (confirmPasswordInput.type === 'password') {
                confirmPasswordInput.type = 'text';
                icon.classList.remove('bi-eye');
                icon.classList.add('bi-eye-slash');
            } else {
                confirmPasswordInput.type = 'password';
                icon.classList.remove('bi-eye-slash');
                icon.classList.add('bi-eye');
            }
        }
    });
} else {
    console.error('Element with id "toggleConfirmPassword" not found');
}

// Image preview functionality
function previewImage(input) {
    if (input.files && input.files[0]) {
        const reader = new FileReader();
        reader.onload = function (e) {
            const preview = document.getElementById('profileImagePreview');
            if (preview) {
                preview.src = e.target.result;
            } else {
                console.error('Element with id "profileImagePreview" not found');
            }
        };
        reader.readAsDataURL(input.files[0]);
    }
}

// Remove image
const removeImage = document.getElementById('removeImage');
if (removeImage) {
    removeImage.addEventListener('click', function () {
        const preview = document.getElementById('profileImagePreview');
        const input = document.getElementById('profileImage');
        if (preview) {
            preview.src = '/images/no-profile.png';
        }
        if (input) {
            input.value = '';
        }
    });
} else {
    console.error('Element with id "removeImage" not found');
}