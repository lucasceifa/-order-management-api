const apiService = (() => {
    const BASE_URL = 'https://localhost:44319/api';

    const makeRequest = (config) => {
        return $.ajax(config)
            .done((_data, _textStatus, jqXHR) => {
                if (config.method !== 'GET') {
                    showToast(config.successMessage || 'Operation completed successfully!');
                }
            })
            .fail(err => {
                const rawMessage = err.responseJSON?.errors || err.responseJSON?.message || err.responseText || 'An unknown error occurred.'
                const stringified = typeof rawMessage === 'string' ? rawMessage : JSON.stringify(rawMessage)

                const match = stringified.match(/^[\w.]+Exception: (.+)$/m)
                const finalMessage = match ? match[1].trim() : stringified

                showToast(finalMessage, true)
            });
    };

    const getCustomers = () => makeRequest({ url: `${BASE_URL}/customer`, method: 'GET' });
    const getCustomerById = (id) => makeRequest({ url: `${BASE_URL}/customer/${id}`, method: 'GET' });
    const createCustomer = (data) => makeRequest({ url: `${BASE_URL}/customer`, method: 'POST', data: JSON.stringify(data), contentType: 'application/json', successMessage: 'Customer created successfully!' });
    const updateCustomer = (id, data) => makeRequest({ url: `${BASE_URL}/customer/${id}`, method: 'PUT', data: JSON.stringify(data), contentType: 'application/json', successMessage: 'Customer updated successfully!' });
    const deleteCustomer = (id) => makeRequest({ url: `${BASE_URL}/customer/${id}`, method: 'DELETE', successMessage: 'Customer deleted successfully!' });

    const getProducts = () => makeRequest({ url: `${BASE_URL}/product`, method: 'GET' });
    const getProductById = (id) => makeRequest({ url: `${BASE_URL}/product/${id}`, method: 'GET' });
    const createProduct = (data) => makeRequest({ url: `${BASE_URL}/product`, method: 'POST', data: JSON.stringify(data), contentType: 'application/json', successMessage: 'Product created successfully!' });
    const updateProduct = (id, data) => makeRequest({ url: `${BASE_URL}/product/${id}`, method: 'PUT', data: JSON.stringify(data), contentType: 'application/json', successMessage: 'Product updated successfully!' });
    const deleteProduct = (id) => makeRequest({ url: `${BASE_URL}/product/${id}`, method: 'DELETE', successMessage: 'Product deleted successfully!' });

    const getOrders = () => makeRequest({ url: `${BASE_URL}/orderxproduct`, method: 'GET' });
    const createOrder = (data) => makeRequest({ url: `${BASE_URL}/orderxproduct`, method: 'POST', data: JSON.stringify(data), contentType: 'application/json', successMessage: 'Order created successfully!' });
    const cancelOrder = (id) => makeRequest({ url: `${BASE_URL}/orderxproduct/cancel/${id}`, method: 'PUT', successMessage: 'Order canceled successfully!' });
    const reopenOrder = (id) => makeRequest({ url: `${BASE_URL}/orderxproduct/reopen/${id}`, method: 'PUT', successMessage: 'Order reactivated successfully!' });
    const deleteOrder = (id) => makeRequest({ url: `${BASE_URL}/orderxproduct/${id}`, method: 'DELETE', successMessage: 'Order deleted successfully!' });

    return {
        getCustomers, getCustomerById, createCustomer, updateCustomer, deleteCustomer,
        getProducts, getProductById, createProduct, updateProduct, deleteProduct,
        getOrders, createOrder, cancelOrder, reopenOrder, deleteOrder
    };
})();