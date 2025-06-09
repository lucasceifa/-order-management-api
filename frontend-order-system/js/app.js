$(document).ready(function () {

    const pages = ['#page-orders', '#page-customers', '#page-products', '#page-order-form'];
    const customerModal = new bootstrap.Modal(document.getElementById('customer-modal'));
    const productModal = new bootstrap.Modal(document.getElementById('product-modal'));
    
    const navigateTo = (pageId) => {
        pages.forEach(page => $(page).addClass('d-none'));
        $(pageId).removeClass('d-none');
        
        $('.nav-link').removeClass('active');
        if (pageId === '#page-orders' || pageId === '#page-order-form') {
            $('#nav-orders').addClass('active');
        } else if (pageId === '#page-customers') {
            $('#nav-customers').addClass('active');
        } else if (pageId === '#page-products') {
            $('#nav-products').addClass('active');
        }
    };
    
    $('#nav-orders').on('click', function(e) { e.preventDefault(); loadOrders(); navigateTo('#page-orders'); });
    $('#nav-customers').on('click', function(e) { e.preventDefault(); loadCustomers(); navigateTo('#page-customers'); });
    $('#nav-products').on('click', function(e) { e.preventDefault(); loadProducts(); navigateTo('#page-products'); });
    $('#btn-new-order').on('click', function() { initOrderForm(); });
    $('#btn-back-to-orders').on('click', function() { loadOrders(); navigateTo('#page-orders'); });

    const formatCurrency = (value) => {
        if (typeof value !== 'number') return '$ 0,00';
        return value.toLocaleString('en-EN', { style: 'currency', currency: 'USD' });
    };

    const loadCustomers = () => {
        apiService.getCustomers().done(customers => {
            const tableBody = $('#customers-table-body');
            const filterCustomer = $('#filter-customer');
            tableBody.empty();
            
            customers
                .filter(e => {
                    if (filterCustomer.val() !== '' && filterCustomer.val().length > 0) {
                        if (e.name.toLowerCase().includes(filterCustomer.val().toLowerCase())) {
                            return e
                        }
                        if (e.email.toLowerCase().includes(filterCustomer.val().toLowerCase())) {
                            return e
                        }
                        return null
                    }

                    return e
                })
                .sort((a, b) => {
                    if (a.name < b.name) {
                        return -1;
                    }
                    if (a.name > b.name) {
                        return 1;
                    }
                    return 0;
                })
                .forEach(c => {
                    tableBody.append(`
                        <tr data-id="${c.id}">
                            <td>${c.name || ''}</td>
                            <td>${c.email || ''}</td>
                            <td>${c.cellphone || ''}</td>
                            <td>
                                <button class="btn btn-sm btn-outline-primary btn-edit-customer"><i class="bi bi-pencil"></i></button>
                                <button class="btn btn-sm btn-outline-danger btn-delete-customer"><i class="bi bi-trash"></i></button>
                            </td>
                        </tr>
                    `);
            });
        });
    };

    $('#customer-form').on('submit', function (e) {
        e.preventDefault();
        const id = $('#customer-id').val();
        const data = {
            name: $('#customer-name').val(),
            email: $('#customer-email').val(),
            cellphone: $('#customer-cellphone').val()
        };
        const promise = id ? apiService.updateCustomer(id, data) : apiService.createCustomer(data);
        promise.done(() => {
            customerModal.hide();
            loadCustomers();
        });
    });
    
    $('#btn-new-customer').on('click', () => {
        $('#customer-form')[0].reset();
        $('#customer-id').val('');
        $('#customer-modal-title').text('New Customer');
    });

    $('#customers-table-body').on('click', '.btn-edit-customer', function() {
        const id = $(this).closest('tr').data('id');
        apiService.getCustomerById(id).done(customer => {
            $('#customer-id').val(customer.id);
            $('#customer-name').val(customer.name);
            $('#customer-email').val(customer.email);
            $('#customer-cellphone').val(customer.cellphone);
            $('#customer-modal-title').text('Edit Customer');
            customerModal.show();
        });
    });

    $('#customers-table-body').on('click', '.btn-delete-customer', function() {
        const id = $(this).closest('tr').data('id');
        if (confirm('Are you sure you want to delete this customer?')) {
            apiService.deleteCustomer(id).done(() => loadCustomers());
        }
    });

     const loadProducts = () => {
        apiService.getProducts().done(products => {
            const tableBody = $('#products-table-body');
            const filterProduct = $('#filter-product');
            tableBody.empty();
            products
                .filter(e => {
                    if (filterProduct.val() !== '' && filterProduct.val().length > 0) {
                        if (e.name.toLowerCase().includes(filterProduct.val().toLowerCase())) {
                            return e
                        }
                        
                        return null
                    }

                    return e
                })
                .sort((a, b) => {
                    if (a.name < b.name) {
                        return -1;
                    }
                    if (a.name > b.name) {
                        return 1;
                    }
                    return 0;
                })
                .forEach(p => {
                    tableBody.append(`
                        <tr data-id="${p.id}">
                            <td>${p.name || ''}</td>
                            <td>${p.description || ''}</td>
                            <td>${formatCurrency(p.price)}</td>
                            <td>${p.quantityAvailable}</td>
                            <td>
                                <button class="btn btn-sm btn-outline-primary btn-edit-product"><i class="bi bi-pencil"></i></button>
                                <button class="btn btn-sm btn-outline-danger btn-delete-product"><i class="bi bi-trash"></i></button>
                            </td>
                        </tr>
                    `);
                });
        });
    };
    
    $('#product-form').on('submit', function(e) {
        e.preventDefault();
        const id = $('#product-id').val();
        
        const priceString = $('#product-price').val();

        const data = {
            name: $('#product-name').val(),
            description: $('#product-description').val(),
            price: parseFloat(priceString),
            quantityAvailable: parseInt($('#product-quantity').val())
        };

        const promise = id ? apiService.updateProduct(id, data) : apiService.createProduct(data);
        promise.done(() => {
            productModal.hide();
            loadProducts();
        });
    });

    $('#btn-new-product').on('click', () => {
        $('#product-form')[0].reset();
        $('#product-id').val('');
        $('#product-modal-title').text('New Product');
    });
    
    $('#products-table-body').on('click', '.btn-edit-product', function() {
        const id = $(this).closest('tr').data('id');
        apiService.getProductById(id).done(product => {
            $('#product-id').val(product.id);
            $('#product-name').val(product.name);
            $('#product-description').val(product.description);
            $('#product-price').val(product.price.toString().replace('.', ','));
            $('#product-quantity').val(product.quantityAvailable);
            $('#product-modal-title').text('Edit Product');
            productModal.show();
        });
    });

    $('#products-table-body').on('click', '.btn-delete-product', function() {
        const id = $(this).closest('tr').data('id');
        if (confirm('Are you sure you want to delete this product?')) {
            apiService.deleteProduct(id).done(() => loadProducts());
        }
    });

    $('#orders-table-body').on('click', '.btn-delete-order', function () {
        const id = $(this).closest('tr').data('id');
        if (confirm('Are you sure you want to delete this order?')) {
            apiService.deleteOrder(id).done(() => loadOrders());
        }
    });

    $('#orders-table-body').on('click', '.btn-cancel-order', function () {
        const id = $(this).closest('tr').data('id');
        if (confirm('Do you want to cancel this order?')) {
            apiService.cancelOrder(id).done(() => loadOrders());
        }
    });

    $('#orders-table-body').on('click', '.btn-reopen-order', function () {
        const id = $(this).closest('tr').data('id');
        if (confirm('Do you want to reactivate this order?')) {
            apiService.reopenOrder(id).done(() => loadOrders());
        }
    });

    const loadOrders = () => {
        apiService.getOrders().done(orders => {
            const tableBody = $('#orders-table-body');
            const filterOrder = $('#filter-order-customer');
            const filterOrderStatus = $('#filter-order-status');
            tableBody.empty();
            orders
                .filter(e => {
                    if (filterOrder.val() !== '' && filterOrder.val().length > 0) {
                        if (e.customerName.toLowerCase().includes(filterOrder.val().toLowerCase())) {
                            return e;
                        }
                        
                        return null;
                    }

                    return e;
                })
                .filter(e => {
                    if (filterOrderStatus.val() !== '') {
                        if (e.status === filterOrderStatus.val()) {
                            return e;
                        }

                        return null;
                    }

                    return e;
                })
                .sort((a, b) => new Date(b.creationDate) - new Date(a.creationDate))
                .forEach(order => {
                    const cancelBtn = `
                        <button class="btn btn-sm btn-outline-warning btn-cancel-order" data-bs-toggle="tooltip" title="Cancel Operation">
                            <i class="bi bi-x-circle"></i>
                        </button>`;
                    
                    const reopenBtn = `
                        <button class="btn btn-sm btn-outline-success btn-reopen-order" data-bs-toggle="tooltip" title="Reactivate Operation">
                            <i class="bi bi-arrow-clockwise"></i>
                        </button>`;

                    const deleteBtn = `
                        <button class="btn btn-sm btn-outline-danger btn-delete-order" data-bs-toggle="tooltip" title="Delete Order">
                            <i class="bi bi-trash"></i>
                        </button>`;

                    const statusActions = order.cancellationDate
                        ? reopenBtn + deleteBtn
                        : cancelBtn + deleteBtn;

                    $('#orders-table-body').append(`
                        <tr data-id="${order.id}">
                            <td>${order.customerName || 'N/A'}</td>
                            <td>${new Date(order.creationDate).toLocaleDateString('pt-BR')}</td>
                            <td>${formatCurrency(order.totalValue)}</td>
                            <td><span class="badge ${order.status === 'Concluded' ? 'bg-success' : 'bg-danger'} text-light">${order.status}</span></td>
                            <td>
                                ${statusActions}
                            </td>
                        </tr>
                    `);
                });

                const newTooltips = document.querySelectorAll('[data-bs-toggle="tooltip"]');
                newTooltips.forEach(el => new bootstrap.Tooltip(el));
        });
    };

    const initOrderForm = () => {
        navigateTo('#page-order-form');
        $('#order-form')[0].reset();
        $('#order-total').text(formatCurrency(0));

        const customerPromise = apiService.getCustomers();
        const productPromise = apiService.getProducts();

        $.when(customerPromise, productPromise).done((customerResult, productResult) => {
            const customerSelect = $('#order-customer-select');
            customerSelect.empty().append('<option value="">Select a customer...</option>');
            customerResult[0].forEach(c => {
                customerSelect.append(`<option value="${c.id}">${c.name}</option>`);
            });

            const productList = $('#product-selection-list');
            productList.empty();
            productResult[0].forEach(p => {
                if(p.quantityAvailable > 0) {
                     productList.append(`
                        <tr class="product-item-row" data-product-id="${p.id}" data-price="${p.price}" data-stock="${p.quantityAvailable}">
                            <td>${p.name}</td>
                            <td>${formatCurrency(p.price)}</td>
                            <td>${p.quantityAvailable}</td>
                            <td>
                                <input type="number" class="form-control form-control-sm product-quantity-input" min="0" max="${p.quantityAvailable}" value="0">
                            </td>
                        </tr>
                    `);
                }
            });
        }).fail(() => {
            showToast("Could not load data to create an order. Please try again.", true);
            navigateTo('#page-orders');
        });
    };

    $('#product-selection-list').on('input', '.product-quantity-input', function() {
        const input = $(this);
        const stock = parseInt(input.closest('tr').data('stock'));
        let quantity = parseInt(input.val());

        if (isNaN(quantity) || quantity < 0) {
            quantity = 0;
            input.val(0);
        }

        if (quantity > stock) {
            input.addClass('is-invalid');
            input.val(stock);
        } else {
            input.removeClass('is-invalid');
        }
        updateOrderTotal();
    });

    const updateOrderTotal = () => {
        let total = 0;
        $('.product-item-row').each(function() {
            const row = $(this);
            const price = parseFloat(row.data('price'));
            const quantity = parseInt(row.find('.product-quantity-input').val());
            if (!isNaN(quantity) && quantity > 0) {
                total += price * quantity;
            }
        });
        $('#order-total').text(formatCurrency(total));
    };

    $('#order-form').on('submit', function(e) {
        e.preventDefault();
        
        const customerId = $('#order-customer-select').val();
        if (!customerId) {
            showToast('Please select a customer.', true);
            return;
        }

        const productXQuantities = [];
        $('.product-item-row').each(function() {
            const quantity = parseInt($(this).find('.product-quantity-input').val());
            if (quantity > 0) {
                productXQuantities.push({
                    productId: $(this).data('product-id'),
                    quantity: quantity
                });
            }
        });

        if (productXQuantities.length === 0) {
            showToast('Please add at least one product to the order.', true);
            return;
        }

        const orderData = { customerId, productXQuantities };

        apiService.createOrder(orderData).done(() => {
            loadOrders();
            navigateTo('#page-orders');
        });
    });

    loadOrders();

    $('#filter-customer').on('input', function () {
        loadCustomers();
    });
    
    $('#filter-product').on('input', function () {
        loadProducts();
    });
    
    $('#filter-order-customer').on('input', function () {
        loadOrders();
    });
    
    $('#filter-order-status').on('input', function () {
        loadOrders();
    });
});