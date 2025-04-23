document.addEventListener('DOMContentLoaded', function () {
    // Change main image when thumbnail is clicked
    window.changeMainImage = function (thumbnail) {
        const mainImage = document.getElementById('mainImage');
        const newSrc = thumbnail.getAttribute('src');

        // Fade out current image
        mainImage.style.opacity = 0;

        // After fade out completes, change src and fade in
        setTimeout(() => {
            mainImage.setAttribute('src', newSrc);
            mainImage.style.opacity = 1;
        }, 300);
    };

    // Load related products
    loadRelatedProducts();

    function loadRelatedProducts() {
        const categoryId = '@Model.Category.Id';
        const currentProductId = '@Model.Id';

        fetch(`/api/products/related?categoryId=${categoryId}&excludeProductId=${currentProductId}&count=4`)
            .then(response => response.json())
            .then(products => {
                const container = document.getElementById('relatedProducts');

                if (products.length === 0) {
                    container.innerHTML = '<p>No related products found.</p>';
                    return;
                }

                container.innerHTML = products.map(product => `
                    <div class="related-product-card">
                        <a href="/admin/product/details?id=${product.id}">
                            <div class="related-product-image">
                                <img src="${product.productImages && product.productImages.length > 0 ?
                        product.productImages[0].imageUrl : '/images/default-product.png'}" 
                                    alt="${product.title}">
                            </div>
                            <div class="related-product-info">
                                <h4 class="related-product-title">${product.title}</h4>
                                <div class="related-product-price">$${product.listPrice.toFixed(2)}</div>
                                <div class="related-product-stock">
                                    ${product.stockQuantity > 0 ?
                        `<span class="in-stock">In Stock (${product.stockQuantity})</span>` :
                        '<span class="out-of-stock">Out of Stock</span>'}
                                </div>
                            </div>
                        </a>
                    </div>
                `).join('');
            })
            .catch(error => {
                console.error('Error loading related products:', error);
                document.getElementById('relatedProducts').innerHTML =
                    '<p>Unable to load related products at this time.</p>';
            });
    }

    // Reuse the deleteProduct function from the index page
    window.deleteProduct = function (id, title) {
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
                        // Redirect to product index after deletion
                        window.location.href = '/admin/product';
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
    };
});