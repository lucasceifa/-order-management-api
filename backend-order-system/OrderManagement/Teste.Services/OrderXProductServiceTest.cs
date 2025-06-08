
using OrderManagement.API.Teste.Services.RepositorysMock;
using OrderManagement.Domain;
using OrderManagement.Domain.Requests;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Enums;
using OrderManagement.Dominio.Requests;
using OrderManagement.Dominio.Utils;
using OrderManagement.Service;
using Xunit;

namespace OrderManagement.API.Tests.Service
{
    [Collection("OrderXProduct Service Test")]
    public class OrderXProductServiceTest
    {
        private readonly CostumerRepositoryMock _costumerRepo;
        private readonly CostumerService _costumerService;
        private readonly ProductRepositoryMock _productRepo;
        private readonly ProductService _productService;
        private readonly OrderRepositoryMock _orderRepo;
        private readonly OrderXProductRepositoryMock _orderXProductRepo;

        public OrderXProductServiceTest()
        {
            _costumerRepo = new CostumerRepositoryMock();
            _productRepo = new ProductRepositoryMock();
            _productService = new ProductService(_productRepo);
            _costumerService = new CostumerService(_costumerRepo);
            _orderRepo = new OrderRepositoryMock();
            _orderXProductRepo = new OrderXProductRepositoryMock();

            var costumer = new Costumer(new CostumerInput
            {
                Name = "Lucas Trindade",
                Email = "lucas@email.com",
                Cellphone = "11999998888"
            });

            var product = new Product(new ProductInput
            {
                Name = "Notebook Dell",
                Description = "Modelo XPS 13",
                Price = 10000,
                QuantityAvailable = 5
            });

            _costumerRepo.CreateAsync(costumer).Wait();
            _productRepo.CreateAsync(product).Wait();
        }

        public OrderXProductService GetService()
        {
            return new OrderXProductService(_orderRepo, _orderXProductRepo, _costumerService, _productService);
        }

        #region Positive Scenarios

        [Fact(DisplayName = "Update existing OrderXProduct with valid data")]
        public async Task UpdateOrderXProductValid()
        {
            var service = GetService();
            var costumer = (await _costumerRepo.GetAsync(new SearchfilterCostumer { })).First();
            var product = (await _productRepo.GetAsync(new SearchfilterProduct { })).First();

            var input = new OrderXProductInput
            {
                CostumerId = costumer.Id,
                ProductXQuantities = new List<ProductXQuantities>
                {
                    new ProductXQuantities
                    {
                        ProductId = product.Id,
                        Quantity = 1
                    }
                }
            };

            var orderId = await service.CreateAsync(input);

            var updatedInput = new OrderXProductInput
            {
                CostumerId = costumer.Id,
                ProductXQuantities = new List<ProductXQuantities>
                {
                    new ProductXQuantities
                    {
                        ProductId = product.Id,
                        Quantity = 2
                    }
                }
            };

            await service.UpdateAsync(orderId, updatedInput);

            var updatedOrder = await service.GetByIdAsync(orderId);

            Assert.NotNull(updatedOrder);
        }

        [Fact(DisplayName = "Cancel Order")]
        public async Task CancelOrder()
        {
            var service = GetService();
            var costumer = (await _costumerRepo.GetAsync(new SearchfilterCostumer { })).First();
            var product = (await _productRepo.GetAsync(new SearchfilterProduct { })).First();

            var input = new OrderXProductInput
            {
                CostumerId = costumer.Id,
                ProductXQuantities = new List<ProductXQuantities>
                {
                    new ProductXQuantities
                    {
                        ProductId = product.Id,
                        Quantity = 1
                    }
                }
            };

            var orderId = await service.CreateAsync(input);

            await service.CancelAsync(orderId);
            var order = await _orderRepo.GetByIdAsync(orderId);

            Assert.Equal(IOrderStatus.Canceled, order.Status);
        }

        [Fact(DisplayName = "Reopen Order")]
        public async Task ReopenOrder()
        {
            var service = GetService();
            var costumer = (await _costumerRepo.GetAsync(new SearchfilterCostumer { })).First();
            var product = (await _productRepo.GetAsync(new SearchfilterProduct { })).First();

            var input = new OrderXProductInput
            {
                CostumerId = costumer.Id,
                ProductXQuantities = new List<ProductXQuantities>
                {
                    new ProductXQuantities
                    {
                        ProductId = product.Id,
                        Quantity = 1
                    }
                }
            };

            var orderId = await service.CreateAsync(input);
            await service.CancelAsync(orderId);
            await service.ReopenAsync(orderId);

            var order = await _orderRepo.GetByIdAsync(orderId);

            Assert.Equal(IOrderStatus.Concluded, order.Status);
        }

        [Fact(DisplayName = "Delete Order")]
        public async Task DeleteOrder()
        {
            var service = GetService();
            var costumer = (await _costumerRepo.GetAsync(new SearchfilterCostumer { })).First();
            var product = (await _productRepo.GetAsync(new SearchfilterProduct { })).First();

            var input = new OrderXProductInput
            {
                CostumerId = costumer.Id,
                ProductXQuantities = new List<ProductXQuantities>
                {
                    new ProductXQuantities
                    {
                        ProductId = product.Id,
                        Quantity = 1
                    }
                }
            };

            var orderId = await service.CreateAsync(input);

            await service.DeleteAsync(orderId);

            await Assert.ThrowsAsync<HttpRequestException>(() => service.GetByIdAsync(orderId));
        }

        [Fact(DisplayName = "Get All Orders")]
        public async Task GetAllOrders()
        {
            var service = GetService();

            var orders = await service.GetAsync();

            Assert.NotNull(orders);
        }

        [Fact(DisplayName = "Get Order by ID")]
        public async Task GetOrderById()
        {
            var service = GetService();
            var costumer = (await _costumerRepo.GetAsync(new SearchfilterCostumer { })).First();
            var product = (await _productRepo.GetAsync(new SearchfilterProduct { })).First();

            var input = new OrderXProductInput
            {
                CostumerId = costumer.Id,
                ProductXQuantities = new List<ProductXQuantities>
                {
                    new ProductXQuantities
                    {
                        ProductId = product.Id,
                        Quantity = 1
                    }
                }
            };

            var orderId = await service.CreateAsync(input);
            var order = await service.GetByIdAsync(orderId);

            Assert.NotNull(order);
        }

        #endregion

        #region Exception Scenarios

        [Fact(DisplayName = "Update Order with invalid ID")]
        public async Task UpdateOrderInvalidId()
        {
            var service = GetService();
            var input = new OrderXProductInput
            {
                CostumerId = Guid.NewGuid(),
                ProductXQuantities = new List<ProductXQuantities>()
            };

            await Assert.ThrowsAsync<HttpRequestException>(() => service.UpdateAsync(Guid.NewGuid(), input));
        }

        [Fact(DisplayName = "Cancel Order with invalid ID")]
        public async Task CancelOrderInvalidId()
        {
            var service = GetService();

            await Assert.ThrowsAsync<HttpRequestException>(() => service.CancelAsync(Guid.NewGuid()));
        }

        [Fact(DisplayName = "Reopen Order with invalid ID")]
        public async Task ReopenOrderInvalidId()
        {
            var service = GetService();

            await Assert.ThrowsAsync<HttpRequestException>(() => service.ReopenAsync(Guid.NewGuid()));
        }

        [Fact(DisplayName = "Delete Order with invalid ID")]
        public async Task DeleteOrderInvalidId()
        {
            var service = GetService();

            await Assert.ThrowsAsync<HttpRequestException>(() => service.DeleteAsync(Guid.NewGuid()));
        }

        [Fact(DisplayName = "Get Order by empty ID")]
        public async Task GetOrderByEmptyId()
        {
            var service = GetService();

            await Assert.ThrowsAsync<ArgumentException>(() => service.GetByIdAsync(Guid.Empty));
        }

        [Fact(DisplayName = "Update OrderXProduct with non-existent Product")]
        public async Task UpdateOrderXProductWithInvalidProduct()
        {
            var service = GetService();
            var costumer = (await _costumerRepo.GetAsync(new SearchfilterCostumer { })).First();
            var product = (await _productRepo.GetAsync(new SearchfilterProduct { })).First();

            var initialInput = new OrderXProductInput
            {
                CostumerId = costumer.Id,
                ProductXQuantities = new List<ProductXQuantities>
                {
                    new ProductXQuantities
                    {
                        ProductId = product.Id,
                        Quantity = 1
                    }
                }
            };

            var orderId = await service.CreateAsync(initialInput);

            var updatedInput = new OrderXProductInput
            {
                CostumerId = costumer.Id,
                ProductXQuantities = new List<ProductXQuantities>
                {
                    new ProductXQuantities
                    {
                        ProductId = Guid.NewGuid(),
                        Quantity = 1
                    }
                }
            };

            await Assert.ThrowsAsync<HttpRequestException>(() => service.UpdateAsync(orderId, updatedInput));
        }

        [Fact(DisplayName = "Update OrderXProduct with insufficient quantity")]
        public async Task UpdateOrderXProductWithInsufficientQuantity()
        {
            var service = GetService();
            var costumer = (await _costumerRepo.GetAsync(new SearchfilterCostumer { })).First();
            var product = (await _productRepo.GetAsync(new SearchfilterProduct { })).First();

            var initialInput = new OrderXProductInput
            {
                CostumerId = costumer.Id,
                ProductXQuantities = new List<ProductXQuantities>
                {
                    new ProductXQuantities
                    {
                        ProductId = product.Id,
                        Quantity = 1
                    }
                }
            };

            var orderId = await service.CreateAsync(initialInput);

            var updatedInput = new OrderXProductInput
            {
                CostumerId = costumer.Id,
                ProductXQuantities = new List<ProductXQuantities>
                {
                    new ProductXQuantities
                    {
                        ProductId = product.Id,
                        Quantity = 999 // Invalid quantity
                    }
                }
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateAsync(orderId, updatedInput));
        }

        #endregion

    }
}
