document.addEventListener('DOMContentLoaded', function () {
    // Configuration
    let currentPage = 1;
    const pageSize = 10;
    let currentSort = { column: 'name', direction: 'asc' };
    let totalPages = 1;
    let isLoading = false;
    let categoryToDeleteId = null;

    // Initialize
    loadCategories();

    // Search functionality
    document.getElementById('categorySearch').addEventListener('input', debounce(function (e) {
        currentPage = 1;
        loadCategories();
    }, 300));

    // Status filter
    document.getElementById('statusFilter').addEventListener('change', function () {
        currentPage = 1;
        loadCategories();
    });

    // Sort by
    document.getElementById('sortBy').addEventListener('change', function (e) {
        const [column, direction] = e.target.value.split('-');
        currentSort = { column, direction };
        currentPage = 1;
        loadCategories();
    });

    // Reset filters
    document.getElementById('resetFilters').addEventListener('click', function () {
        document.getElementById('categorySearch').value = '';
        document.getElementById('statusFilter').value = '';
        document.getElementById('sortBy').value = 'name-asc';
        currentSort = { column: 'name', direction: 'asc' };
        currentPage = 1;
        loadCategories();
    });

    // Reset from empty state
    document.getElementById('resetEmptyState').addEventListener('click', function () {
        document.getElementById('categorySearch').value = '';
        document.getElementById('statusFilter').value = '';
        document.getElementById('sortBy').value = 'name-asc';
        currentSort = { column: 'name', direction: 'asc' };
        currentPage = 1;
        loadCategories();
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
            loadCategories();
        });
    });

    // Pagination controls
    document.getElementById('prevPage').addEventListener('click', function () {
        if (currentPage > 1) {
            currentPage--;
            loadCategories();
        }
    });

    document.getElementById('nextPage').addEventListener('click', function () {
        if (currentPage < totalPages) {
            currentPage++;
            loadCategories();
        }
    });

    // Handle delete confirmation
    document.getElementById('confirmDeleteBtn').addEventListener('click', function () {
        if (categoryToDeleteId) {
            deleteCategory(categoryToDeleteId);
        }
    });

    // Load categories from server
    function loadCategories() {
        if (isLoading) return;

        isLoading = true;
        showLoading(true);

        // Prepare query parameters
        const queryParams = {
            PageNumber: currentPage,
            PageSize: pageSize,
            SearchTerm: document.getElementById('categorySearch').value,
            SortColumn: currentSort.column,
            SortDirection: currentSort.direction,
            IsActive: document.getElementById('statusFilter').value || null
        };

        // Remove null parameters
        Object.keys(queryParams).forEach(key => {
            if (queryParams[key] === null || queryParams[key] === '') {
                delete queryParams[key];
            }
        });

        // Make AJAX request
        fetch('/admin/category/get-categories', {
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
                renderCategories(data.items);
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
                    'There was a problem loading categories.',
                    'error'
                );
            });
    }

    // Render categories to table
    function renderCategories(categories) {
        const tbody = document.getElementById('categoriesTableBody');
        tbody.innerHTML = '';

        categories.forEach(category => {
            const tr = document.createElement('tr');
            tr.className = 'category-row';

            // Get status badge class based on active status
            const statusBadgeClass = category.isActive ? 'badge-success' : 'badge-danger';
            const statusText = category.isActive ? 'Active' : 'Inactive';

            tr.innerHTML = `
                <td class="name-cell">
                    <div class="category-name">
                        <h6>${category.name}</h6>
                    </div>
                </td>
                <td>${category.description || 'No description'}</td>
                <td class="numeric">${category.displayOrder}</td>
                <td>
                    <span class="status-badge ${statusBadgeClass}">${statusText}</span>
                </td>
                <td class="actions">
                    <div class="action-buttons">
                        <a href="/admin/category/upsert?id=${category.id}"
                           class="btn-action btn-edit" title="Edit">
                            <i class="bi bi-pencil-square"></i>
                        </a>
                        <button class="btn-action btn-delete" title="Delete"
                                onclick="showDeleteModal('${category.id}', '${category.name.replace(/'/g, "\\'")}')">
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
                loadCategories();
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
    categoryToDeleteId = id;
    document.getElementById('categoryToDelete').textContent = name;

    const deleteModal = new bootstrap.Modal(document.getElementById('deleteModal'));
    deleteModal.show();
}

// Delete category function
function deleteCategory(id) {
    fetch(`/admin/category/delete/${id}`, {
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

            // Reload categories to reflect deletion
            document.dispatchEvent(new Event('DOMContentLoaded'));

            // Show success message
            Swal.fire(
                'Deleted!',
                'The category has been deleted.',
                'success'
            );
        })
        .catch(error => {
            Swal.fire(
                'Error!',
                'There was a problem deleting the category.',
                'error'
            );
            console.error('Error:', error);
        });
}