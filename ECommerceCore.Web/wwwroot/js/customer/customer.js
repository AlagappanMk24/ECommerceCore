document.addEventListener('DOMContentLoaded', function () {
    // Configuration
    let currentPage = 1;
    const pageSize = 10;
    let currentSort = { column: 'name', direction: 'asc' };
    let totalPages = 1;
    let isLoading = false;
    let customerToDeleteId = null;

    // Initialize
    loadCustomers();

    // Search functionality
    document.getElementById('customerSearch').addEventListener('input', debounce(function (e) {
        currentPage = 1;
        loadCustomers();
    }, 300));

    // Company filter
    document.getElementById('companyFilter').addEventListener('change', function () {
        currentPage = 1;
        loadCustomers();
    });

    // Sort by
    document.getElementById('sortBy').addEventListener('change', function (e) {
        const [column, direction] = e.target.value.split('-');
        currentSort = { column, direction };
        currentPage = 1;
        loadCustomers();
    });

    // Reset filters
    document.getElementById('resetFilters').addEventListener('click', function () {
        document.getElementById('customerSearch').value = '';
        document.getElementById('companyFilter').value = '';
        document.getElementById('sortBy').value = 'name-asc';
        currentSort = { column: 'name', direction: 'asc' };
        currentPage = 1;
        loadCustomers();
    });

    // Reset from empty state
    document.getElementById('resetEmptyState').addEventListener('click', function () {
        document.getElementById('customerSearch').value = '';
        document.getElementById('companyFilter').value = '';
        document.getElementById('sortBy').value = 'name-asc';
        currentSort = { column: 'name', direction: 'asc' };
        currentPage = 1;
        loadCustomers();
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
            loadCustomers();
        });
    });

    // Pagination controls
    document.getElementById('prevPage').addEventListener('click', function () {
        if (currentPage > 1) {
            currentPage--;
            loadCustomers();
        }
    });

    document.getElementById('nextPage').addEventListener('click', function () {
        if (currentPage < totalPages) {
            currentPage++;
            loadCustomers();
        }
    });

    // Handle delete confirmation
    document.getElementById('confirmDeleteBtn').addEventListener('click', function () {
        if (customerToDeleteId) {
            deleteCustomer(customerToDeleteId);
        }
    });

    // Load customers from server
    function loadCustomers() {
        if (isLoading) return;

        isLoading = true;
        showLoading(true);

        // Prepare query parameters
        const queryParams = {
            PageNumber: currentPage,
            PageSize: pageSize,
            SearchTerm: document.getElementById('customerSearch').value,
            SortColumn: currentSort.column,
            SortDirection: currentSort.direction,
            CompanyId: document.getElementById('companyFilter').value || null
        };

        // Remove null parameters
        Object.keys(queryParams).forEach(key => {
            if (queryParams[key] === null || queryParams[key] === '') {
                delete queryParams[key];
            }
        });

        // Make AJAX request
        fetch('/admin/customer/get-customers', {
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
                renderCustomers(data.items);
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
                    'There was a problem loading customers.',
                    'error'
                );
            });
    }

    // Render customers to table
    function renderCustomers(customers) {
        const tbody = document.getElementById('customersTableBody');
        tbody.innerHTML = '';

        customers.forEach(customer => {
            const tr = document.createElement('tr');
            tr.className = 'customer-row';

            tr.innerHTML = `
                <td class="name-cell">
                    <div class="customer-name">
                        <h4>${customer.name}</h4>
                        <small>${customer.email}</small>
                    </div>
                </td>
                <td>${customer.phoneNumber || 'N/A'}</td>
                <td>${customer.companyName}</td>
                <td>${formatAddress(customer.address)}</td>
                <td class="actions">
                    <div class="action-buttons">
                        <a href="/admin/customer/upsert?id=${customer.id}"
                           class="btn-action btn-edit" title="Edit">
                            <i class="bi bi-pencil-square"></i>
                        </a>
                        <button class="btn-action btn-delete" title="Delete"
                                onclick="showDeleteModal('${customer.id}', '${customer.name.replace(/'/g, "\\'")}')">
                            <i class="bi bi-trash"></i>
                        </button>
                        <a href="/admin/customer/details?id=${customer.id}"
                           class="btn-action btn-details" title="Details">
                            <i class="bi bi-info-circle"></i>
                        </a>
                    </div>
                </td>
            `;

            tbody.appendChild(tr);
        });
    }

    // Format address for display
    function formatAddress(address) {
        if (!address) return 'No address';

        const parts = [];
        if (address.streetAddress) parts.push(address.streetAddress);
        if (address.city) parts.push(address.city);
        if (address.state) parts.push(address.state);
        if (address.postalCode) parts.push(address.postalCode);

        return parts.length > 0 ? parts.join(', ') : 'No address';
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
        document.getElementById('totalCount').textContent = `${totalCount} items`;
    }

    // Helper to add page button
    function addPageButton(page) {
        const pageBtn = document.createElement('button');
        pageBtn.className = 'page-number' + (page === currentPage ? ' active' : '');
        pageBtn.textContent = page;
        pageBtn.addEventListener('click', function () {
            if (currentPage !== page) {
                currentPage = page;
                loadCustomers();
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
});

// Show delete confirmation modal
function showDeleteModal(id, name) {
    customerToDeleteId = id;
    document.getElementById('customerToDelete').textContent = name;

    const deleteModal = new bootstrap.Modal(document.getElementById('deleteModal'));
    deleteModal.show();
}

// Delete customer function
function deleteCustomer(id) {
    fetch(`/admin/customer/delete/${id}`, {
        method: 'DELETE',
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

            // Reload customers to reflect deletion
            document.dispatchEvent(new Event('DOMContentLoaded'));

            // Show success message
            Swal.fire(
                'Deleted!',
                'The customer has been deleted.',
                'success'
            );
        })
        .catch(error => {
            Swal.fire(
                'Error!',
                'There was a problem deleting the customer.',
                'error'
            );
            console.error('Error:', error);
        });
}