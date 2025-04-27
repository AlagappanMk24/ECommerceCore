document.addEventListener('DOMContentLoaded', function () {
    // Configuration
    let currentPage = 1;
    const pageSize = 10;
    let currentSort = { column: 'invoiceNumber', direction: 'asc' };
    let totalPages = 1;
    let isLoading = false;

    // Initialize
    loadInvoices();

    // Search functionality
    document.getElementById('invoiceSearch').addEventListener('input', debounce(function (e) {
        currentPage = 1;
        loadInvoices();
    }, 300));

    // Customer filter
    document.getElementById('customerFilter').addEventListener('change', function () {
        currentPage = 1;
        loadInvoices();
    });

    // Status filter
    document.getElementById('statusFilter').addEventListener('change', function () {
        currentPage = 1;
        loadInvoices();
    });

    // Type filter
    document.getElementById('typeFilter').addEventListener('change', function () {
        currentPage = 1;
        loadInvoices();
    });

    // Sort by
    document.getElementById('sortBy').addEventListener('change', function (e) {
        const [column, direction] = e.target.value.split('-');
        currentSort = { column, direction };
        currentPage = 1;
        loadInvoices();
    });

    // Reset filters
    document.getElementById('resetFilters').addEventListener('click', function () {
        document.getElementById('invoiceSearch').value = '';
        document.getElementById('customerFilter').value = '';
        document.getElementById('statusFilter').value = '';
        document.getElementById('typeFilter').value = '';
        document.getElementById('sortBy').value = 'invoiceNumber-asc';
        currentSort = { column: 'invoiceNumber', direction: 'asc' };
        currentPage = 1;
        loadInvoices();
    });

    // Reset from empty state
    document.getElementById('resetEmptyState').addEventListener('click', function () {
        document.getElementById('invoiceSearch').value = '';
        document.getElementById('customerFilter').value = '';
        document.getElementById('statusFilter').value = '';
        document.getElementById('typeFilter').value = '';
        document.getElementById('sortBy').value = 'invoiceNumber-asc';
        currentSort = { column: 'invoiceNumber', direction: 'asc' };
        currentPage = 1;
        loadInvoices();
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
            loadInvoices();
        });
    });

    // Pagination controls
    document.getElementById('prevPage').addEventListener('click', function () {
        if (currentPage > 1) {
            currentPage--;
            loadInvoices();
        }
    });

    document.getElementById('nextPage').addEventListener('click', function () {
        if (currentPage < totalPages) {
            currentPage++;
            loadInvoices();
        }
    });

    // Load invoices from server
    function loadInvoices() {
        if (isLoading) return;

        isLoading = true;
        showLoading(true);

        // Prepare query parameters
        const queryParams = {
            PageNumber: currentPage,
            PageSize: pageSize,
            SearchTerm: document.getElementById('invoiceSearch').value,
            SortColumn: currentSort.column,
            SortDirection: currentSort.direction,
            CustomerId: document.getElementById('customerFilter').value || null,
            Status: document.getElementById('statusFilter').value || null,
            Type: document.getElementById('typeFilter').value || null
        };

        // Remove null parameters
        Object.keys(queryParams).forEach(key => {
            if (queryParams[key] === null || queryParams[key] === '') {
                delete queryParams[key];
            }
        });

        // Make AJAX request
        fetch('/admin/invoice/get-invoices', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify(queryParams)
        })
            .then(response => {
                console.log(response, "res");
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                console.log(data, "data");
                renderInvoices(data.items);
                updatePagination(data.totalCount);
                updateCounts(data.totalCount, data.items.length);
                isLoading = false;
                showLoading(false);

                // Safely handle emptyState element
                const emptyState = document.getElementById('emptyState');
                if (emptyState) {
                    emptyState.style.display = data.items.length === 0 ? 'block' : 'none';
                }
            })
            .catch(error => {
                console.error('Error:', error);
                isLoading = false;
                showLoading(false);
                Swal.fire(
                    'Error!',
                    'There was a problem loading invoices.',
                    'error'
                );
            });
    }

    // Render invoices to table
    function renderInvoices(invoices) {
        console.log(invoices, "invoices");
        const tbody = document.getElementById('invoicesTableBody');
        tbody.innerHTML = '';

        invoices.forEach(invoice => {
            const tr = document.createElement('tr');
            tr.className = 'invoice-row';

            // Format issue date
            const issueDate = new Date(invoice.issueDate).toLocaleDateString();

            // Get the status and format it for display
            const displayStatus = invoice.status.replace(/([A-Z])/g, ' $1').trim();

            // Get status badge class
            const statusClass = {
                'Draft': 'badge-draft',
                'Sent': 'badge-sent',
                'PartiallyPaid': 'badge-partially-paid',
                'Paid': 'badge-paid',
                'Overdue': 'badge-overdue',
                'Void': 'badge-void'
            }[invoice.status] || 'badge-default';

            // Get type display name
            const typeDisplay = {
                'Standard': 'Standard',
                'Recurring': 'Recurring',
                'Proforma': 'Proforma',
                'CreditNote': 'Credit Note'
            }[invoice.invoiceType] || invoice.invoiceType;

            tr.innerHTML = `
                <td>${invoice.invoiceNumber}</td>
                <td>${invoice.customerName}</td>
                <td>${issueDate}</td>
                <td class="total-amount">$${invoice.totalAmount.toFixed(2)}</td>
                <td><span class="badge ${statusClass}">${displayStatus}</span></td>
                <td>${typeDisplay}</td>
                <td class="actions">
                    <div class="action-buttons">
                        <a href="/admin/invoice/upsert?id=${invoice.id}"
                           class="btn-action btn-edit" title="Edit">
                            <i class="bi bi-pencil-square"></i>
                        </a>
                        <a href="/admin/invoice/details?id=${invoice.id}"
                           class="btn-action btn-view" title="View">
                            <i class="bi bi-eye"></i>
                        </a>
                        <button class="btn-action btn-delete" title="Delete"
                                onclick="deleteInvoice('${invoice.id}', '${invoice.invoiceNumber.replace(/'/g, "\\'")}')">
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
                loadInvoices();
            }
        });
        document.getElementById('pageNumbers').appendChild(pageBtn);
    }

    // Update showing X to Y of Z
    function updateCounts(totalCount, currentCount) {
        const from = ((currentPage - 1) * pageSize) + 1;
        const to = from + currentCount - 1;

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

function deleteInvoice(id, invoiceNumber) {
    Swal.fire({
        title: 'Delete Invoice?',
        html: `Are you sure you want to delete invoice <strong>${invoiceNumber}</strong>? This action cannot be undone.`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'Cancel',
        reverseButtons: true,
        backdrop: `
            rgba(0,0,0,0.4)
            url("/images/trash-icon-animated.gif")
            left top
            no-repeat
        `
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`/admin/invoice/delete/${id}`, {
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
                    // Reload invoices to reflect deletion
                    document.dispatchEvent(new Event('DOMContentLoaded'));

                    // Show success message
                    Swal.fire(
                        'Deleted!',
                        'The invoice has been deleted.',
                        'success'
                    );
                })
                .catch(error => {
                    Swal.fire(
                        'Error!',
                        'There was a problem deleting the invoice.',
                        'error'
                    );
                    console.error('Error:', error);
                });
        }
    });
}