document.addEventListener('DOMContentLoaded', function () {
    // Configuration
    let currentPage = 1;
    const pageSize = 10;
    let currentSort = { column: 'orderdate', direction: 'desc' };
    let totalPages = 1;
    let isLoading = false;
    let orderToDeleteId = null;
    let orderToUpdateId = null;

    // Initialize
    loadOrders();
    initDateRangePicker();

    // Order status visibility logic
    document.getElementById('newOrderStatus').addEventListener('change', function () {
        toggleShippingFields(this.value);
    });

    // Search functionality
    document.getElementById('orderSearch').addEventListener('input', debounce(function (e) {
        currentPage = 1;
        loadOrders();
    }, 300));

    // Filters
    document.getElementById('statusFilter').addEventListener('change', function () {
        currentPage = 1;
        loadOrders();
    });

    document.getElementById('paymentStatusFilter').addEventListener('change', function () {
        currentPage = 1;
        loadOrders();
    });

    document.getElementById('startDate').addEventListener('change', function () {
        currentPage = 1;
        loadOrders();
    });

    document.getElementById('endDate').addEventListener('change', function () {
        currentPage = 1;
        loadOrders();
    });

    // Sort by
    document.getElementById('sortBy').addEventListener('change', function (e) {
        const [column, direction] = e.target.value.split('-');
        currentSort = { column, direction };
        currentPage = 1;
        loadOrders();
    });

    // Reset filters
    document.getElementById('resetFilters').addEventListener('click', function () {
        resetFilters();
    });

    // Reset from empty state
    document.getElementById('resetEmptyState').addEventListener('click', function () {
        resetFilters();
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
            loadOrders();
        });
    });

    // Pagination controls
    document.getElementById('prevPage').addEventListener('click', function () {
        if (currentPage > 1) {
            currentPage--;
            loadOrders();
        }
    });

    document.getElementById('nextPage').addEventListener('click', function () {
        if (currentPage < totalPages) {
            currentPage++;
            loadOrders();
        }
    });

    // Handle delete confirmation
    document.getElementById('confirmDeleteBtn').addEventListener('click', function () {
        if (orderToDeleteId) {
            deleteOrder(orderToDeleteId);
        }
    });

    // Handle status update
    document.getElementById('updateOrderStatusBtn').addEventListener('click', function () {
        if (orderToUpdateId) {
            updateOrderStatus(orderToUpdateId);
        }
    });

    // Reset filters function
    function resetFilters() {
        document.getElementById('orderSearch').value = '';
        document.getElementById('statusFilter').value = '';
        document.getElementById('paymentStatusFilter').value = '';
        document.getElementById('startDate').value = '';
        document.getElementById('endDate').value = '';
        document.getElementById('sortBy').value = 'orderdate-desc';
        currentSort = { column: 'orderdate', direction: 'desc' };
        currentPage = 1;
        loadOrders();
    }

    // Initialize date picker with default values
    function initDateRangePicker() {
        // No default dates - user can select custom range
    }

    // Toggle shipping fields based on order status
    function toggleShippingFields(status) {
        const trackingSection = document.getElementById('trackingInfoSection');
        const carrierSection = document.getElementById('carrierSection');
        const shippingDateSection = document.getElementById('shippingDateSection');

        if (status === 'Shipped' || status === 'Delivered') {
            trackingSection.style.display = 'block';
            carrierSection.style.display = 'block';
            shippingDateSection.style.display = 'block';
        } else {
            trackingSection.style.display = 'none';
            carrierSection.style.display = 'none';
            shippingDateSection.style.display = 'none';
        }
    }

    // Load orders from server
    function loadOrders() {
        if (isLoading) return;

        isLoading = true;
        showLoading(true);

        // Prepare query parameters
        const queryParams = {
            PageNumber: currentPage,
            PageSize: pageSize,
            SearchTerm: document.getElementById('orderSearch').value,
            SortColumn: currentSort.column,
            SortDirection: currentSort.direction,
            OrderStatus: document.getElementById('statusFilter').value || null,
            PaymentStatus: document.getElementById('paymentStatusFilter').value || null,
            StartDate: document.getElementById('startDate').value || null,
            EndDate: document.getElementById('endDate').value || null
        };

        // Remove null parameters
        Object.keys(queryParams).forEach(key => {
            if (queryParams[key] === null || queryParams[key] === '') {
                delete queryParams[key];
            }
        });

        // Make AJAX request
        fetch('/admin/order/get-orders', {
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
                renderOrders(data.items);
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
                    'There was a problem loading orders.',
                    'error'
                );
            });
    }

    // Render orders to table
    function renderOrders(orders) {
        const tbody = document.getElementById('ordersTableBody');
        tbody.innerHTML = '';

        orders.forEach(order => {
            const tr = document.createElement('tr');
            tr.className = 'order-row';

            // Format the date
            const orderDate = new Date(order.orderDate).toLocaleDateString();

            // Get status badge class based on order status
            const orderStatusBadgeClass = getOrderStatusBadgeClass(order.orderStatus);
            const paymentStatusBadgeClass = getPaymentStatusBadgeClass(order.paymentStatus);

            // Get status icon based on order status
            const orderStatusIcon = getOrderStatusIcon(order.orderStatus);
            const paymentStatusIcon = getPaymentStatusIcon(order.paymentStatus);

            // Format the currency
            const formattedTotal = new Intl.NumberFormat('en-US', {
                style: 'currency',
                currency: 'USD'
            }).format(order.orderTotal);

            // Customer name could be from ApplicationUser or Customer
            const customerName = order.customerName ?? 'N/A';

            tr.innerHTML = `
                <td class="id-cell">
                    <div class="order-id">
                        <span>#${order.id}</span>
                    </div>
                </td>
                <td>${orderDate}</td>
                <td>${customerName}</td>
                <td class="numeric">${formattedTotal}</td>
                <td>
                   <span class="order-status-badge ${orderStatusBadgeClass}">
                    ${orderStatusIcon}${order.orderStatus}
                   </span>
                </td>
                <td>
                  <span class="order-status-badge ${paymentStatusBadgeClass}">
                    ${paymentStatusIcon}${order.paymentStatus}
                  </span>
                </td>
                <td class="actions">
                    <div class="action-buttons">
                        <a href="/admin/order/details?id=${order.id}"
                           class="btn-action btn-view" title="View Details">
                            <i class="bi bi-eye"></i>
                        </a>
                        <button class="btn-action btn-status" title="Update Status"
                                onclick="showStatusModal('${order.id}', '${order.orderStatus}', '${order.paymentStatus}', '${order.trackingNumber || ''}', '${order.carrier || ''}')">
                            <i class="bi bi-gear"></i>
                        </button>
                        <button class="btn-action btn-delete" title="Delete"
                                onclick="showDeleteModal('${order.id}')">
                            <i class="bi bi-trash"></i>
                        </button>
                    </div>
                </td>
            `;

            tbody.appendChild(tr);
        });
    }

    function getOrderStatusBadgeClass(status) {
        switch (status.toLowerCase()) {
            case 'pending':
                return 'badge-warning';
            case 'processing':
                return 'badge-info';
            case 'shipped':
                return 'badge-primary';
            case 'delivered':
                return 'badge-success';
            case 'cancelled':
                return 'badge-danger';
            default:
                return 'badge-secondary';
        }
    }

    function getPaymentStatusBadgeClass(status) {
        switch (status.toLowerCase()) {
            case 'pending':
                return 'payment-pending';
            case 'approved':
            case 'paid':
                return 'payment-approved';
            case 'rejected':
            case 'failed':
                return 'payment-rejected';
            case 'refunded':
                return 'payment-refunded';
            default:
                return 'badge-secondary';
        }
    }

    function getOrderStatusIcon(status) {
        switch (status.toLowerCase()) {
            case 'pending':
                return '<i class="bi bi-hourglass-split"></i>';
            case 'processing':
                return '<i class="bi bi-gear-fill"></i>';
            case 'shipped':
                return '<i class="bi bi-truck"></i>';
            case 'delivered':
                return '<i class="bi bi-check-circle-fill"></i>';
            case 'cancelled':
                return '<i class="bi bi-x-circle-fill"></i>';
            default:
                return '<i class="bi bi-question-circle"></i>';
        }
    }

    function getPaymentStatusIcon(status) {
        switch (status.toLowerCase()) {
            case 'pending':
                return '<i class="bi bi-clock-history"></i>';
            case 'approved':
            case 'paid':
                return '<i class="bi bi-check-circle-fill"></i>';
            case 'rejected':
            case 'failed':
                return '<i class="bi bi-exclamation-circle-fill"></i>';
            case 'refunded':
                return '<i class="bi bi-arrow-counterclockwise"></i>';
            default:
                return '<i class="bi bi-question-circle"></i>';
        }
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
                loadOrders();
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

    // Delete order function
    function deleteOrder(id) {
        fetch(`/admin/order/delete/${id}`, {
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

                // Reload orders to reflect deletion
                loadOrders();

                // Show success message
                Swal.fire(
                    'Deleted!',
                    'The order has been deleted.',
                    'success'
                );
            })
            .catch(error => {
                Swal.fire(
                    'Error!',
                    'There was a problem deleting the order.',
                    'error'
                );
                console.error('Error:', error);
            });
    }

    // Update order status function
    function updateOrderStatus(id) {
        const orderStatus = document.getElementById('newOrderStatus').value;
        const paymentStatus = document.getElementById('newPaymentStatus').value;
        const trackingNumber = document.getElementById('trackingNumber').value;
        const carrier = document.getElementById('carrier').value;
        const shippingDate = document.getElementById('shippingDate').value;

        const updateData = {
            OrderId: id,
            OrderStatus: orderStatus,
            PaymentStatus: paymentStatus,
            TrackingNumber: trackingNumber,
            Carrier: carrier,
            ShippingDate: shippingDate
        };

        fetch(`/admin/order/update-status`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify(updateData)
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                }
                throw new Error('Network response was not ok.');
            })
            .then(data => {
                // Hide modal
                bootstrap.Modal.getInstance(document.getElementById('orderStatusModal')).hide();

                // Reload orders to reflect status update
                loadOrders();

                // Show success message
                Swal.fire(
                    'Updated!',
                    'The order status has been updated.',
                    'success'
                );
            })
            .catch(error => {
                Swal.fire(
                    'Error!',
                    'There was a problem updating the order status.',
                    'error'
                );
                console.error('Error:', error);
            });
    }
});

// Show delete confirmation modal
function showDeleteModal(id) {
    orderToDeleteId = id;
    document.getElementById('orderToDelete').textContent = id;

    const deleteModal = new bootstrap.Modal(document.getElementById('deleteModal'));
    deleteModal.show();
}

// Show status update modal
function showStatusModal(id, orderStatus, paymentStatus, trackingNumber, carrier) {
    orderToUpdateId = id;
    document.getElementById('orderIdForStatus').textContent = id;

    // Set current values
    document.getElementById('newOrderStatus').value = orderStatus;
    document.getElementById('newPaymentStatus').value = paymentStatus;
    document.getElementById('trackingNumber').value = trackingNumber || '';
    document.getElementById('carrier').value = carrier || '';

    // Set today as default shipping date if empty
    if (!document.getElementById('shippingDate').value) {
        const today = new Date().toISOString().split('T')[0];
        document.getElementById('shippingDate').value = today;
    }

    // Toggle shipping fields based on order status
    toggleShippingFields(orderStatus);

    const statusModal = new bootstrap.Modal(document.getElementById('orderStatusModal'));
    statusModal.show();
}

// Toggle shipping fields based on order status
function toggleShippingFields(status) {
    const trackingSection = document.getElementById('trackingInfoSection');
    const carrierSection = document.getElementById('carrierSection');
    const shippingDateSection = document.getElementById('shippingDateSection');

    if (status === 'Shipped' || status === 'Delivered') {
        trackingSection.style.display = 'block';
        carrierSection.style.display = 'block';
        shippingDateSection.style.display = 'block';
    } else {
        trackingSection.style.display = 'none';
        carrierSection.style.display = 'none';
        shippingDateSection.style.display = 'none';
    }
}