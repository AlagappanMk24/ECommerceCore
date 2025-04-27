document.addEventListener('DOMContentLoaded', function () {
    // Configuration
    let currentPage = 1;
    const pageSize = 10;
    let currentSort = { column: 'title', direction: 'asc' };
    let totalPages = 1;
    let isLoading = false;

    // Initialize
    loadProducts();

    // Search functionality
    document.getElementById('productSearch').addEventListener('input', debounce(function (e) {
        currentPage = 1;
        loadProducts();
    }, 300));

    // Category filter
    document.getElementById('categoryFilter').addEventListener('change', function () {
        currentPage = 1;
        loadProducts();
    });

    // Stock filter
    document.getElementById('stockFilter').addEventListener('change', function () {
        currentPage = 1;
        loadProducts();
    });

    // Sort by
    document.getElementById('sortBy').addEventListener('change', function (e) {
        const [column, direction] = e.target.value.split('-');
        currentSort = { column, direction };
        currentPage = 1;
        loadProducts();
    });

    // Reset filters
    document.getElementById('resetFilters').addEventListener('click', function () {
        document.getElementById('productSearch').value = '';
        document.getElementById('categoryFilter').value = '';
        document.getElementById('stockFilter').value = '';
        document.getElementById('sortBy').value = 'title-asc';
        currentSort = { column: 'title', direction: 'asc' };
        currentPage = 1;
        loadProducts();
    });

    // Reset from empty state
    document.getElementById('resetEmptyState').addEventListener('click', function () {
        document.getElementById('productSearch').value = '';
        document.getElementById('categoryFilter').value = '';
        document.getElementById('stockFilter').value = '';
        document.getElementById('sortBy').value = 'title-asc';
        currentSort = { column: 'title', direction: 'asc' };
        currentPage = 1;
        loadProducts();
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
            loadProducts();
        });
    });

    // Pagination controls
    document.getElementById('prevPage').addEventListener('click', function () {
        if (currentPage > 1) {
            currentPage--;
            loadProducts();
        }
    });

    document.getElementById('nextPage').addEventListener('click', function () {
        if (currentPage < totalPages) {
            currentPage++;
            loadProducts();
        }
    });

    // Load products from server
    function loadProducts() {
        if (isLoading) return;

        isLoading = true;
        showLoading(true);

        // Prepare query parameters
        const queryParams = {
            PageNumber: currentPage,
            PageSize: pageSize,
            SearchTerm: document.getElementById('productSearch').value,
            SortColumn: currentSort.column,
            SortDirection: currentSort.direction,
            CategoryId: document.getElementById('categoryFilter').value || null,
            StockStatus: document.getElementById('stockFilter').value || null
        };

        // Remove null parameters
        Object.keys(queryParams).forEach(key => {
            if (queryParams[key] === null || queryParams[key] === '') {
                delete queryParams[key];
            }
        });

        // Make AJAX request
        fetch('/admin/product/get-products', {
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
                console.log(response, "res");
                return response.json();
            })
            .then(data => {
                console.log(data, "data");
                renderProducts(data.items);
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
                    'There was a problem loading products.',
                    'error'
                );
            });
    }

    // Render products to table
    function renderProducts(products) {
        console.log(products, "pro");
        const tbody = document.getElementById('productsTableBody');
        tbody.innerHTML = '';

        products.forEach(product => {
            const tr = document.createElement('tr');
            tr.className = 'product-row';

            // Safely get SKU
            const sku = product.sku || 'N/A';

            // Safely get category name
            const categoryName = product.categoryName || 'Uncategorized';

            // Safely get stock quantity
            const stockQuantity = product.stockQuantity ?? 0;

            // Get first product image or use default
            const productImage = product.productImages && product.productImages.length > 0 ?
                product.productImages[0].imageUrl : '/images/default-product.png';

            tr.innerHTML = `
                        <td class="item-info">
                            <div class="item-image">
                                <img src="${productImage}" alt="${product.title}">
                            </div>
                            <div class="item-details">
                                <h4>${product.title}</h4>
                                <small>SKU: ${sku}</small>
                            </div>
                        </td>
                        <td>${product.sku}</td>
                           <td>
                            <span class="category-badge">${categoryName}</span>
                        </td>
                        <td class="price">$${product.price}</td>
                         <td>
                           ${stockQuantity}
                        </td>
                        <td class="actions">
                            <div class="action-buttons">
                                <a href="/admin/product/upsert?id=${product.id}"
                                   class="btn-action btn-edit" title="Edit">
                                    <i class="bi bi-pencil-square"></i>
                                </a>
                                <a href="/admin/product/details?id=${product.id}"
                                   class="btn-action btn-view" title="View">
                                    <i class="bi bi-eye"></i>
                                </a>
                                <button class="btn-action btn-delete" title="Delete"
                                        onclick="deleteProduct('${product.id}', '${product.title.replace(/'/g, "\\'")}')">
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
                loadProducts();
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

function deleteProduct(id, title) {
    Swal.fire({
        title: 'Delete Product?',
        html: `Are you sure you want to delete <strong>${title}</strong>? This action cannot be undone.`,
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
            fetch(`/admin/product/delete/${id}`, {
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
                    // Reload products to reflect deletion
                    document.dispatchEvent(new Event('DOMContentLoaded'));

                    // Show success message
                    Swal.fire(
                        'Deleted!',
                        'The product has been deleted.',
                        'success'
                    );
                })
                .catch(error => {
                    Swal.fire(
                        'Error!',
                        'There was a problem deleting the product.',
                        'error'
                    );
                    console.error('Error:', error);
                });
        }
    });
}