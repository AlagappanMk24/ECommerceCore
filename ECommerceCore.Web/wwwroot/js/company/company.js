document.addEventListener('DOMContentLoaded', function () {
    // Configuration
    let currentPage = 1;
    const pageSize = 10;
    let currentSort = { column: 'name', direction: 'asc' };
    let totalPages = 1;
    let isLoading = false;
    let companyToDeleteId = null;

    // Initialize
    loadCompanies();

    // Search functionality
    document.getElementById('companySearch').addEventListener('input', debounce(function (e) {
        currentPage = 1;
        loadCompanies();
    }, 300));

    // State filter
    document.getElementById('stateFilter').addEventListener('change', function () {
        currentPage = 1;
        loadCompanies();
    });

    // Sort by
    document.getElementById('sortBy').addEventListener('change', function (e) {
        const [column, direction] = e.target.value.split('-');
        currentSort = { column, direction };
        currentPage = 1;
        loadCompanies();
    });

    // Reset filters
    document.getElementById('resetFilters').addEventListener('click', function () {
        document.getElementById('companySearch').value = '';
        document.getElementById('stateFilter').value = '';
        document.getElementById('sortBy').value = 'name-asc';
        currentSort = { column: 'name', direction: 'asc' };
        currentPage = 1;
        loadCompanies();
    });

    // Reset from empty state
    document.getElementById('resetEmptyState').addEventListener('click', function () {
        document.getElementById('companySearch').value = '';
        document.getElementById('stateFilter').value = '';
        document.getElementById('sortBy').value = 'name-asc';
        currentSort = { column: 'name', direction: 'asc' };
        currentPage = 1;
        loadCompanies();
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
            loadCompanies();
        });
    });

    // Pagination controls
    document.getElementById('prevPage').addEventListener('click', function () {
        if (currentPage > 1) {
            currentPage--;
            loadCompanies();
        }
    });

    document.getElementById('nextPage').addEventListener('click', function () {
        if (currentPage < totalPages) {
            currentPage++;
            loadCompanies();
        }
    });

    // Handle delete confirmation
    document.getElementById('confirmDeleteBtn').addEventListener('click', function () {
        if (companyToDeleteId) {
            deleteCompany(companyToDeleteId);
        }
    });

    // Load companies from server
    function loadCompanies() {
        if (isLoading) return;

        isLoading = true;
        showLoading(true);

        // Prepare query parameters
        const queryParams = {
            PageNumber: currentPage,
            PageSize: pageSize,
            SearchTerm: document.getElementById('companySearch').value,
            SortColumn: currentSort.column,
            SortDirection: currentSort.direction,
            State: document.getElementById('stateFilter').value || null
        };

        // Remove null parameters
        Object.keys(queryParams).forEach(key => {
            if (queryParams[key] === null || queryParams[key] === '') {
                delete queryParams[key];
            }
        });
        console.log(queryParams);
        // Make AJAX request
        fetch('/admin/company/get-companies', {
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
                renderCompanies(data.items);
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
                    'There was a problem loading companies.',
                    'error'
                );
            });
    }

    // Render companies to table
    function renderCompanies(companies) {
        const tbody = document.getElementById('companiesTableBody');
        tbody.innerHTML = '';

        companies.forEach(company => {
            const tr = document.createElement('tr');
            tr.className = 'company-row';

            //// Format address for display
            //const address = [company.streetAddress, company.city, company.state, company.postalCode]
            //    .filter(item => item) // Remove empty values
            //    .join(', ');

            tr.innerHTML = `
                <td class="name-cell">
                    <div class="company-name">
                        <h6>${company.name}</h6>
                    </div>
                </td>
                <td>${company.streetAddress || 'N/A'}</td>
                <td>${company.city || 'N/A'}</td>
                <td>${company.state || 'N/A'}</td>
                <td>${company.phoneNumber || 'N/A'}</td>
                <td class="actions">
                    <div class="action-buttons">
                        <a href="/admin/company/upsert?id=${company.id}"
                           class="btn-action btn-edit" title="Edit">
                            <i class="bi bi-pencil-square"></i>
                        </a>
                        <button class="btn-action btn-view" title="View" 
                                onclick="window.location.href='/admin/company/details/${company.id}'">
                            <i class="bi bi-eye"></i>
                        </button>
                        <button class="btn-action btn-delete" title="Delete"
                                onclick="showDeleteModal('${company.id}', '${company.name.replace(/'/g, "\\'")}')">
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
                loadCompanies();
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
    companyToDeleteId = id;
    document.getElementById('companyToDelete').textContent = name;

    const deleteModal = new bootstrap.Modal(document.getElementById('deleteModal'));
    deleteModal.show();
}

// Delete company function
function deleteCompany(id) {
    fetch(`/admin/company/delete/${id}`, {
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

            // Reload companies to reflect deletion
            document.dispatchEvent(new Event('DOMContentLoaded'));

            // Show success message
            Swal.fire(
                'Deleted!',
                'The company has been deleted.',
                'success'
            );
        })
        .catch(error => {
            Swal.fire(
                'Error!',
                'There was a problem deleting the company.',
                'error'
            );
            console.error('Error:', error);
        });
}