﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderManagement.Domain.Requests;
using OrderManagement.Domain;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Enums;
using OrderManagement.Dominio.Interfaces;
using OrderManagement.Dominio.Utils;
using OrderXProductManagement.Dominio.Interfaces;
using OrderManagement.Dominio.Requests;
using OrderManagement.Domain.Responses;

namespace OrderManagement.Service
{
    public class OrderXProductService
    {
        private readonly IOrderRepository _repOrder;
        private readonly IOrderXProductRepository _repOrderXProduct;
        private readonly CustomerService _servCustomer;
        private readonly ProductService _servProduct;

        public OrderXProductService(
            IOrderRepository orderRepository,
            IOrderXProductRepository orderXProductRepository,
            CustomerService CustomerRepository,
            ProductService productRepository)
        {
            _repOrder = orderRepository;
            _repOrderXProduct = orderXProductRepository;
            _servCustomer = CustomerRepository;
            _servProduct = productRepository;
        }

        public async Task<Guid> CreateAsync(OrderXProductInput input)
        {
            if (input == null || input.ProductXQuantities == null || !input.ProductXQuantities.Any())
                throw new ArgumentException("Invalid input data");

            var Customer = await _servCustomer.GetByIdAsync(input.CustomerId);
            if (Customer == null)
                throw new HttpRequestException("Customer not found");

            var order = new Order
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                CustomerId = input.CustomerId,
                Status = IOrderStatus.Concluded
            };

            await _repOrder.CreateAsync(order);

            foreach (var item in input.ProductXQuantities)
            {
                var product = await _servProduct.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new HttpRequestException("Product not found");

                if (product.QuantityAvailable < item.Quantity)
                    throw new InvalidOperationException($"Insufficient stock for product {product.Name}");

                var orderXProduct = new OrderXProduct
                {
                    Id = Guid.NewGuid(),
                    CreationDate = DateTime.UtcNow,
                    OrderId = order.Id,
                    ProductId = product.Id,
                    QuantityPurchased = item.Quantity,
                    ProductValue = product.Price
                };

                await _repOrderXProduct.CreateAsync(orderXProduct);

                await _servProduct.UpdateByIdAsync(product.Id, new ProductInput { Description = product.Description, Name = product.Name, Price = product.Price, QuantityAvailable = product.QuantityAvailable - item.Quantity });
            }

            return order.Id;
        }

        public async Task UpdateAsync(Guid orderId, OrderXProductInput input)
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("Invalid order ID");

            if (input == null || input.ProductXQuantities == null)
                throw new ArgumentException("Invalid input data");

            var order = await _repOrder.GetByIdAsync(orderId);
            if (order == null)
                throw new HttpRequestException("Order not found");

            var existingOrderXProducts = (await _repOrderXProduct.GetAsync())
                .Where(oxp => oxp.OrderId == orderId)
                .ToList();

            var inputProductIds = input.ProductXQuantities.Select(p => p.ProductId).ToList();

            foreach (var existing in existingOrderXProducts)
            {
                var inputItem = input.ProductXQuantities.FirstOrDefault(p => p.ProductId == existing.ProductId);
                if (inputItem == null || inputItem.Quantity == 0)
                {
                    await _repOrderXProduct.DeleteAsync(existing.Id);
                }
            }

            foreach (var item in input.ProductXQuantities)
            {
                var product = await _servProduct.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new HttpRequestException($"Product with ID {item.ProductId} not found");

                if (product.QuantityAvailable < item.Quantity)
                    throw new InvalidOperationException($"Insufficient stock for product {product.Name}");

                var existing = existingOrderXProducts.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (existing != null)
                {
                    existing.QuantityPurchased = item.Quantity;
                    existing.ProductValue = product.Price;
                    await _repOrderXProduct.UpdateAsync(existing);
                }
                else
                {
                    var newOrderXProduct = new OrderXProduct
                    {
                        Id = Guid.NewGuid(),
                        CreationDate = DateTime.UtcNow,
                        OrderId = order.Id,
                        ProductId = product.Id,
                        QuantityPurchased = item.Quantity,
                        ProductValue = product.Price
                    };
                    await _repOrderXProduct.CreateAsync(newOrderXProduct);
                }
            }
        }

        public async Task CancelAsync(Guid orderId)
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("Invalid order ID");

            var order = await _repOrder.GetByIdAsync(orderId);
            if (order == null)
                throw new HttpRequestException("Order not found");

            order.Status = IOrderStatus.Canceled;
            order.CancellationDate = DateTime.Now;
            await _repOrder.UpdateStatusAsync(order.Id, order.Status, order.CancellationDate);
        }

        public async Task ReopenAsync(Guid orderId)
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("Invalid order ID");

            var order = await _repOrder.GetByIdAsync(orderId);
            if (order == null)
                throw new HttpRequestException("Order not found");

            order.Status = IOrderStatus.Concluded;
            order.CancellationDate = null;
            await _repOrder.UpdateStatusAsync(order.Id, order.Status, order.CancellationDate);
        }

        public async Task DeleteAsync(Guid orderId)
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("Invalid order ID");

            var order = await _repOrder.GetByIdAsync(orderId);
            if (order == null)
                throw new HttpRequestException("Order not found");

            var orderXProducts = (await _repOrderXProduct.GetAsync())
                .Where(oxp => oxp.OrderId == orderId)
                .ToList();

            foreach (var oxp in orderXProducts)
            {
                await _repOrderXProduct.DeleteAsync(oxp.Id);
            }

            await _repOrder.DeleteAsync(orderId);
        }

        public async Task<Order> GetByIdAsync(Guid orderId)
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("Invalid order ID");

            var order = await _repOrder.GetByIdAsync(orderId);
            if (order == null)
                throw new HttpRequestException("Order not found");

            return order;
        }

        public async Task<List<ProductsOrderResponse>> GetProductsByOrderIdAsync(Guid orderId)
        {
            var order = await _repOrder.GetByIdAsync(orderId);
            var orderXProductItems = await _repOrderXProduct.GetByOrderIdAsync(orderId);
            var products = await _servProduct.GetAsync(new SearchfilterProduct { });

            return orderXProductItems.Select(e => new ProductsOrderResponse(e, products.FirstOrDefault(p => p.Id == e.ProductId))).ToList();
        }

        public async Task<IEnumerable<OrderResponse>> GetAsync()
        {
            var orders = await _repOrder.GetAsync();
            var customers = await _servCustomer.GetAsync(new SearchfilterCustomer { });
            var orderXProductItems = await _repOrderXProduct.GetAsync();

            return orders.Select(e => new OrderResponse(e, customers.FirstOrDefault(c => c.Id == e.CustomerId), orderXProductItems.Where(oXp => oXp.OrderId == e.Id).ToList()));
        }
    }
}
