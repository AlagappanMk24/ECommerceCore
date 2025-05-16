document.addEventListener('DOMContentLoaded', function () {
    // Configuration
    let currentPage = 1;
    const pageSize = 10;
    let currentSort = { column: 'name', direction: 'asc' };
    let totalPages = 1;
    let isLoading = false;
    let userToDeleteId = null;
    let currentStatusFilter = '';

    // Initialize
    loadUsers();

    // Search functionality
    document.getElementById('userSearch').addEventListener('input', debounce(function (e) {
        currentPage = 1;
        loadUsers();
    }, 300));

    // Role filter
    document.getElementById('roleFilter').addEventListener('change', function () {
        currentPage = 1;
        loadUsers();
    });

    // Company filter
    document.getElementById('companyFilter').addEventListener('change', function () {
        currentPage = 1;
        loadUsers();
    });

    // Sort by
    document.getElementById('sortBy').addEventListener('change', function (e) {
        const [column, direction] = e.target.value.split('-');
        currentSort = { column, direction };
        currentPage = 1;
        loadUsers();
    });

    // Reset filters
    document.getElementById('resetFilters').addEventListener('click', function () {
        document.getElementById('userSearch').value = '';
        document.getElementById('roleFilter').value = '';
        document.getElementById('companyFilter').value = '';
        document.getElementById('sortBy').value = 'name-asc';
        currentSort = { column: 'name', direction: 'asc' };
        currentStatusFilter = '';
        updateQuickFilterStyles();
        currentPage = 1;
        loadUsers();
    });

    // Reset from empty state
    document.getElementById('resetEmptyState').addEventListener('click', function () {
        document.getElementById('userSearch').value = '';
        document.getElementById('roleFilter').value = '';
        document.getElementById('companyFilter').value = '';
        document.getElementById('sortBy').value = 'name-asc';
        currentSort = { column: 'name', direction: 'asc' };
        currentStatusFilter = '';
        updateQuickFilterStyles();
        currentPage = 1;
        loadUsers();
    });

    // Sortable headers
    document.querySelectorAll('.sortable').forEach(header => {
        header.addEventListener('click', function () {
            const column = this.dataset.sort;
            let direction = 'asc';

            if (this.classList.contains('active')) {
                direction = this.classList.contains('asc') ? 'desc' : 'asc';
            }

            // Update UI
            document.querySelectorAll('.sortable').forEach(h => {
                h.classList.remove('active', 'asc', 'desc');
            });

            this.classList.add('active', direction);

            // Update sort dropdown to match
            document.getElementById('sortBy').value = `${column}-${direction}`;

            // Update sort and reload
            currentSort = { column, direction };
            currentPage = 1;
            loadUsers();
        });
    });

    // Pagination controls
    document.getElementById('prevPage').addEventListener('click', function () {
        if (currentPage > 1) {
            currentPage--;
            loadUsers();
        }
    });

    document.getElementById('nextPage').addEventListener('click', function () {
        if (currentPage < totalPages) {
            currentPage++;
            loadUsers();
        }
    });

    // Handle delete confirmation
    document.getElementById('confirmDeleteBtn').addEventListener('click', function () {
        if (userToDeleteId) {
            deleteUser(userToDeleteId);
        }
    });

    // Load users from server
    function loadUsers() {
        if (isLoading) return;

        isLoading = true;
        showLoading(true);

        // Prepare query parameters
        const queryParams = {
            PageNumber: currentPage,
            PageSize: pageSize,
            SearchTerm: document.getElementById('userSearch').value,
            SortColumn: currentSort.column,
            SortDirection: currentSort.direction,
            Role: document.getElementById('roleFilter').value || null,
            CompanyId: document.getElementById('companyFilter').value || null,
            Status: currentStatusFilter || null
        };

        // Remove null parameters
        Object.keys(queryParams).forEach(key => {
            if (queryParams[key] === null || queryParams[key] === '') {
                delete queryParams[key];
            }
        });

        // Make AJAX request
        fetch('/admin/user/get-users', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify(queryParams)
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                renderUsers(data.items);
                updatePagination(data.totalCount);
                updateCounts(data.totalCount, data.items.length);
                isLoading = false;
                showLoading(false);

                // Handle empty state
                const emptyState = document.getElementById('emptyState');
                emptyState.style.display = data.items.length === 0 ? 'block' : 'none';
            })
            .catch(error => {
                console.error('Error:', error);
                isLoading = false;
                showLoading(false);
                Swal.fire(
                    'Error!',
                    'There was a problem loading users.',
                    'error'
                );
            });
    }

    // Render users to table
    function renderUsers(users) {
        const tbody = document.getElementById('usersTableBody');
        tbody.innerHTML = '';

        users.forEach(user => {
            const tr = document.createElement('tr');
            tr.className = 'user-row';

            // Map roles to their badge classes
            const roleBadgeMap = {
                'Super Admin': 'badge-super-admin', // Deep Blue
                'Admin': 'badge-admin',           // Blue
                'Customer': 'badge-customer',     // Green
                'Company': 'badge-company',       // Cyan
                'Vendor': 'badge-vendor',         // Purple
                'Supplier': 'badge-supplier',     // Indigo
                'Customer Support': 'badge-customer-support', // Teal
                'Delivery Agent': 'badge-delivery-agent',     // Orange
                'Manager': 'badge-manager',       // Red
                'Employee': 'badge-employee'      // Gray
            };

            // Generate badge HTML for each role
            const roleBadges = user.roles.map(role => {
                const badgeClass = roleBadgeMap[role] || 'badge-secondary'; // Fallback to secondary if role not found
                return `<span class="status-badge ${badgeClass}">${role}</span>`;
            }).join(' '); // Join badges with a space for display


            tr.innerHTML = `
            <td class="checkbox-column"><input type="checkbox" class="user-checkbox" data-id="${user.id}"></td>
                <td class="name-cell">
                 <div class="user-info">
                  <img src="${user.profileImageUrl || '/images/default-avatar.png'}"
                       alt="${user.name}"
                       class="avatar" />
                  <span class="user-name">${user.name}</span>
                 </div>
                </td>
                <td>${user.email || 'Not specified'}</td>
                <td>${user.phoneNumber}</td>
                <td>${user.companyName || 'N/A'}</td>
               <td>${roleBadges}</td>
                <td class="actions">
                    <div class="action-buttons">
                        <a href="/admin/user/details?id=${user.id}" class="btn-action btn-view" title="View Details">
                            <i class="bi bi-eye"></i>
                        </a>
                        <a href="/admin/user/upsert?id=${user.id}" class="btn-action btn-edit" title="Edit">
                            <i class="bi bi-pencil-square"></i>
                        </a>
                        <button class="btn-action btn-reset-password" title="Reset Password"
                                onclick="resetPassword('${user.id}', '${user.email}')">
                            <i class="bi bi-key"></i>
                        </button>
                        <button class="btn-action btn-toggle-status" title="${user.isLocked ? 'Unlock' : 'Lock'} User"
                                onclick="toggleUserStatus('${user.id}', ${user.isLocked})">
                            <i class="bi ${user.isLocked ? 'bi-unlock' : 'bi-lock'}"></i>
                        </button>
                        <button class="btn-action btn-impersonate" title="Impersonate User"
                                onclick="impersonateUser('${user.id}')">
                            <i class="bi bi-person-check"></i>
                        </button>
                        <button class="btn-action btn-assign-roles" title="Manage Roles"
                                onclick="manageRoles('${user.id}', '${user.name.replace(/'/g, "\\'")}')">
                            <i class="bi bi-person-gear"></i>
                        </button>
                        <a href="/admin/user/activity-log?id=${user.id}" class="btn-action btn-activity-log" title="Activity Log">
                            <i class="bi bi-clock-history"></i>
                        </a>
                        <button class="btn-action btn-delete" title="Delete"
                                onclick="showDeleteModal('${user.id}', '${user.name.replace(/'/g, "\\'")}')">
                            <i class="bi bi-trash"></i>
                        </button>
                    </div>
                </td>
            `;

            tbody.appendChild(tr);
        });
    }

    // Update pagination controls
    function updatePagination(totalCount) {
        totalPages = Math.ceil(totalCount / pageSize);
        const pageNumbers = document.getElementById('pageNumbers');
        pageNumbers.innerHTML = '';

        // Always show first page
        addPageButton(1);

        // Show ellipsis if needed before current page
        if (currentPage > 3) {
            const ellipsis = document.createElement('span');
            ellipsis.className = 'page-ellipsis';
            ellipsis.textContent = '...';
            pageNumbers.appendChild(ellipsis);
        }

        // Show current page and neighbors
        for (let i = Math.max(2, currentPage - 1); i <= Math.min(totalPages - 1, currentPage + 1); i++) {
            addPageButton(i);
        }

        // Show ellipsis if needed after current page
        if (currentPage < totalPages - 2) {
            const ellipsis = document.createElement('span');
            ellipsis.className = 'page-ellipsis';
            ellipsis.textContent = '...';
            pageNumbers.appendChild(ellipsis);
        }

        // Always show last page if there's more than one page
        if (totalPages > 1) {
            addPageButton(totalPages);
        }

        // Update button states
        document.getElementById('prevPage').disabled = currentPage === 1;
        document.getElementById('nextPage').disabled = currentPage === totalPages;

        // Update total count
        document.getElementById('totalCount').textContent = `${totalCount} users`;
    }

    // Helper to add page button
    function addPageButton(page) {
        const pageBtn = document.createElement('button');
        pageBtn.className = 'page-number' + (page === currentPage ? ' active' : '');
        pageBtn.textContent = page;
        pageBtn.addEventListener('click', function () {
            if (currentPage !== page) {
                currentPage = page;
                loadUsers();
            }
        });
        document.getElementById('pageNumbers').appendChild(pageBtn);
    }

    // Update showing X to Y of Z
    function updateCounts(totalCount, currentCount) {
        const from = totalCount === 0 ? 0 : ((currentPage - 1) * pageSize) + 1;
        const to = Math.min(from + currentCount - 1, totalCount);

        document.getElementById('showingFrom').textContent = from;
        document.getElementById('showingTo').textContent = to;
        document.getElementById('totalItems').textContent = totalCount;
    }

    // Show/hide loading indicator
    function showLoading(show) {
        document.getElementById('loadingIndicator').style.display = show ? 'block' : 'none';
    }

    // Debounce function to limit how often a function is called
    function debounce(func, wait) {
        let timeout;
        return function () {
            const context = this;
            const args = arguments;
            clearTimeout(timeout);
            timeout = setTimeout(() => {
                func.apply(context, args);
            }, wait);
        };
    }

    // Toggle select all checkboxes
    function toggleSelectAll() {
        const selectAll = document.getElementById('selectAllUsers');
        const checkboxes = document.querySelectorAll('.user-checkbox');
        checkboxes.forEach(checkbox => {
            checkbox.checked = selectAll.checked;
        });
    }
    // Apply bulk action
    function applyBulkAction() {
        const action = document.getElementById('bulkAction').value;
        if (!action) {
            Swal.fire('Error!', 'Please select an action.', 'error');
            return;
        }

        const selectedUsers = Array.from(document.querySelectorAll('.user-checkbox:checked'))
            .map(checkbox => checkbox.dataset.id);

        if (!selectedUsers.length) {
            Swal.fire('Error!', 'Please select at least one user.', 'error');
            return;
        }

        Swal.fire({
            title: `Confirm ${action.charAt(0).toUpperCase() + action.slice(1)}`,
            text: `Are you sure you want to ${action} ${selectedUsers.length} user(s)?`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: `Yes, ${action}!`,
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                fetch('/admin/user/bulk-action', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                    },
                    body: JSON.stringify({ action, userIds: selectedUsers })
                })
                    .then(response => response.json())
                    .then(data => {
                        if (data.success) {
                            Swal.fire('Success!', `Bulk action ${action} completed.`, 'success');
                            document.dispatchEvent(new Event('DOMContentLoaded')); // Reload table
                        } else {
                            Swal.fire('Error!', data.message || 'Bulk action failed.', 'error');
                        }
                    })
                    .catch(error => {
                        Swal.fire('Error!', 'An error occurred during bulk action.', 'error');
                    });
            }
        });
    }

    // Export users
    function exportUsers() {
        const queryParams = {
            SearchTerm: document.getElementById('userSearch').value,
            SortColumn: currentSort.column,
            SortDirection: currentSort.direction,
            Role: document.getElementById('roleFilter').value || null,
            CompanyId: document.getElementById('companyFilter').value || null,
            Status: currentStatusFilter || null
        };

        Object.keys(queryParams).forEach(key => {
            if (queryParams[key] === null || queryParams[key] === '') {
                delete queryParams[key];
            }
        });

        fetch('/admin/user/export', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify(queryParams)
        })
            .then(response => response.blob())
            .then(blob => {
                const url = window.URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.href = url;
                a.download = `users_${new Date().toISOString().split('T')[0]}.csv`;
                document.body.appendChild(a);
                a.click();
                a.remove();
                window.URL.revokeObjectURL(url);
            })
            .catch(error => {
                Swal.fire('Error!', 'An error occurred while exporting users.', 'error');
            });
    }

    // Filter by status
    function filterByStatus(status) {
        currentStatusFilter = status;
        currentPage = 1;
        updateQuickFilterStyles();
        loadUsers();
    }

    // Update quick filter button styles
    function updateQuickFilterStyles() {
        document.querySelectorAll('.quick-filters .btn:not(.btn-export)').forEach(btn => {
            btn.classList.remove('active');
            if (btn.textContent.toLowerCase() === currentStatusFilter) {
                btn.classList.add('active');
            }
        });
    }

    // Reset Password
    function resetPassword(id, email) {
        Swal.fire({
            title: 'Reset Password',
            text: `Are you sure you want to reset the password for ${email}?`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, reset it!',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(`/admin/user/reset-password/${id}`, {
                    method: 'POST',
                    headers: {
                        'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                    }
                })
                    .then(response => response.json())
                    .then(data => {
                        if (data.success) {
                            Swal.fire('Success!', 'Password reset email sent.', 'success');
                        } else {
                            Swal.fire('Error!', data.message || 'Failed to reset password.', 'error');
                        }
                    })
                    .catch(error => {
                        Swal.fire('Error!', 'An error occurred while resetting the password.', 'error');
                    });
            }
        });
    }

    // Toggle User Status
    function toggleUserStatus(id, isLocked) {
        Swal.fire({
            title: isLocked ? 'Unlock User' : 'Lock User',
            text: `Are you sure you want to ${isLocked ? 'unlock' : 'lock'} this user?`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: `Yes, ${isLocked ? 'unlock' : 'lock'}!`,
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(`/admin/user/toggle-status/${id}`, {
                    method: 'PATCH',
                    headers: {
                        'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                    }
                })
                    .then(response => response.json())
                    .then(data => {
                        if (data.success) {
                            Swal.fire('Success!', `User has been ${isLocked ? 'unlocked' : 'locked'}.`, 'success');
                            document.dispatchEvent(new Event('DOMContentLoaded')); // Reload table
                        } else {
                            Swal.fire('Error!', data.message || 'Failed to update user status.', 'error');
                        }
                    })
                    .catch(error => {
                        Swal.fire('Error!', 'An error occurred while updating user status.', 'error');
                    });
            }
        });
    }

    // Impersonate User
    function impersonateUser(id) {
        Swal.fire({
            title: 'Impersonate User',
            text: 'Are you sure you want to impersonate this user? You will be logged in as them.',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, impersonate!',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(`/admin/user/impersonate/${id}`, {
                    method: 'POST',
                    headers: {
                        'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                    }
                })
                    .then(response => response.json())
                    .then(data => {
                        if (data.success) {
                            window.location.href = '/'; // Redirect to user dashboard
                        } else {
                            Swal.fire('Error!', data.message || 'Failed to impersonate user.', 'error');
                        }
                    })
                    .catch(error => {
                        Swal.fire('Error!', 'An error occurred while impersonating the user.', 'error');
                    });
            }
        });
    }

    // Manage Roles
    function manageRoles(id, name) {
        fetch(`/admin/user/get-roles/${id}`, {
            method: 'GET',
            headers: {
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            }
        })
            .then(response => response.json())
            .then(data => {
                let roleCheckboxes = '';
                const allRoles = ['Super Admin', 'Admin', 'Customer', 'Company', 'Vendor', 'Supplier', 'Customer Support', 'Delivery Agent', 'Manager', 'Employee'];
                allRoles.forEach(role => {
                    const isChecked = data.roles.includes(role) ? 'checked' : '';
                    roleCheckboxes += `
                    <div class="form-check">
                        <input type="checkbox" class="form-check-input" name="roles" value="${role}" ${isChecked}>
                        <label class="form-check-label">${role}</label>
                    </div>`;
                });

                Swal.fire({
                    title: `Manage Roles for ${name}`,
                    html: `
                    <form id="manageRolesForm">
                        ${roleCheckboxes}
                    </form>`,
                    showCancelButton: true,
                    confirmButtonText: 'Save Roles',
                    preConfirm: () => {
                        const form = document.getElementById('manageRolesForm');
                        const selectedRoles = Array.from(form.querySelectorAll('input[name="roles"]:checked'))
                            .map(input => input.value);
                        return fetch(`/admin/user/assign-roles/${id}`, {
                            method: 'POST',
                            headers: {
                                'Content-Type': 'application/json',
                                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                            },
                            body: JSON.stringify({ roles: selectedRoles })
                        })
                            .then(response => response.json())
                            .then(data => {
                                if (!data.success) {
                                    throw new Error(data.message || 'Failed to update roles.');
                                }
                                return data;
                            })
                            .catch(error => {
                                Swal.showValidationMessage(`Error: ${error.message}`);
                            });
                    }
                }).then((result) => {
                    if (result.isConfirmed) {
                        Swal.fire('Success!', 'User roles updated successfully.', 'success');
                        document.dispatchEvent(new Event('DOMContentLoaded')); // Reload table
                    }
                });
            })
            .catch(error => {
                Swal.fire('Error!', 'An error occurred while fetching user roles.', 'error');
            });
    }
});

// Global variable to store the ID of the user to delete
let userToDeleteId = null;

// Show delete confirmation modal
function showDeleteModal(id, name) {
    userToDeleteId = id;
    document.getElementById('userToDelete').textContent = name;

    const deleteModal = new bootstrap.Modal(document.getElementById('deleteModal'));
    deleteModal.show();
}
// Delete user function
function deleteUser(id) {
    fetch(`/admin/user/${id}`, {
        method: 'PATCH',
        headers: {
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        }
    })
        .then(response => {
            if (response.ok) {
                return response.json();
            }
            throw new Error('Network response was not ok.');
        })
        .then(data => {
            // Hide modal
            bootstrap.Modal.getInstance(document.getElementById('deleteModal')).hide();

            // Reload users to reflect deletion
            document.dispatchEvent(new Event('DOMContentLoaded'));

            // Show success message
            Swal.fire(
                'Marked as Deleted!',
                'The user has been marked as deleted.',
                'success'
            );
        })
        .catch(error => {
            Swal.fire(
                'Error!',
                'There was a problem marking the user as deleted.',
                'error'
            );
            console.error('Error:', error);
        });
}

// Bind click event to confirm delete button
document.getElementById('confirmDeleteBtn').addEventListener('click', () => {
    if (userToDeleteId) {
        deleteUser(userToDeleteId);
    }
});

//// Jquery
//$(document).ready(function () {
//    $('#confirmDeleteBtn').on('click', function () {
//        if (userToDeleteId) {
//            deleteUser(userToDeleteId);
//        }
//    });
//});