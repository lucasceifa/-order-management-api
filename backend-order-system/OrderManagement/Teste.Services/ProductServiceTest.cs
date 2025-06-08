using OrderManagement.Dominio;
using OrderManagement.Dominio.Requests;
using OrderManagement.Dominio.Utils;
using OrderManagement.Service;
using OrderManagement.API.Teste.Services.RepositorysMock;
using Xunit;

namespace OrderManagement.API.Tests.Service
{
    [Collection("Product Service Test")]
    public class ProductServiceTest
    {
        public ProductService GetService()
        {
            var productRepository = new ProductRepositoryMock();
            return new ProductService(productRepository);
        }

        public static class TestData
        {
            public static IEnumerable<object[]> ValidProducts =>
                new List<object[]>
                {
                    new object[]
                    {
                        new ProductInput
                        {
                            Name = "Dell Notebook",
                            Description = "Inspiron 15 Model",
                            Price = 4500.00,
                            QuantityAvailable = 10
                        }
                    },
                    new object[]
                    {
                        new ProductInput
                        {
                            Name = "Logitech Mouse",
                            Description = "Wireless",
                            Price = 150.00,
                            QuantityAvailable = 50
                        }
                    },
                    new object[]
                    {
                        new ProductInput
                        {
                            Name = "Mechanical Keyboard",
                            Description = "ABNT2 RGB",
                            Price = 300.00,
                            QuantityAvailable = 20
                        }
                    },
                    new object[]
                    {
                        new ProductInput
                        {
                            Name = "LG Monitor",
                            Description = "27 inches 4K",
                            Price = 1200.00,
                            QuantityAvailable = 5
                        }
                    },
                    new object[]
                    {
                        new ProductInput
                        {
                            Name = "HyperX Headset",
                            Description = "Cloud Stinger",
                            Price = 400.00,
                            QuantityAvailable = 15
                        }
                    }
                };

            public static IEnumerable<object[]> InvalidProducts =>
                new List<object[]>
                {
                    new object[]
                    {
                        new ProductInput
                        {
                            Name = "",
                            Description = "Test",
                            Price = 100,
                            QuantityAvailable = 10
                        }
                    },
                    new object[]
                    {
                        new ProductInput
                        {
                            Name = "A",
                            Description = "Test",
                            Price = 100,
                            QuantityAvailable = 10
                        }
                    },
                    new object[]
                    {
                        new ProductInput
                        {
                            Name = "No Price Product",
                            Description = "Test",
                            Price = -1,
                            QuantityAvailable = 10
                        }
                    },
                    new object[]
                    {
                        new ProductInput
                        {
                            Name = "No Stock Product",
                            Description = "Test",
                            Price = 100,
                            QuantityAvailable = -5
                        }
                    }
                };
        }

        #region Positive Test Cases

        [Theory(DisplayName = "Creating valid Products")]
        [MemberData(nameof(TestData.ValidProducts), MemberType = typeof(TestData))]
        public async Task CreateValidProducts(ProductInput input)
        {
            var service = GetService();
            await service.CreateAsync(input);

            var products = await service.GetAsync(new SearchfilterProduct { });

            Assert.True(products.Any(p => p.Name == input.Name && p.Price == input.Price && p.QuantityAvailable == input.QuantityAvailable));
        }

        [Theory(DisplayName = "Creating Product and retrieving by ID")]
        [MemberData(nameof(TestData.ValidProducts), MemberType = typeof(TestData))]
        public async Task RetrieveProductById(ProductInput input)
        {
            var service = GetService();
            var response = await service.CreateAsync(input);

            var product = await service.GetByIdAsync(response);

            Assert.True(product != null && product.Id == response);
        }

        [Theory(DisplayName = "Creating and deleting Product")]
        [MemberData(nameof(TestData.ValidProducts), MemberType = typeof(TestData))]
        public async Task DeleteProductById(ProductInput input)
        {
            var service = GetService();
            var response = await service.CreateAsync(input);

            await service.DeleteAsync(response);
            var products = await service.GetAsync(new SearchfilterProduct { });

            Assert.True(products.All(p => p.Id != response));
        }

        [Theory(DisplayName = "Creating and updating Product")]
        [MemberData(nameof(TestData.ValidProducts), MemberType = typeof(TestData))]
        public async Task UpdateProductById(ProductInput input)
        {
            var service = GetService();
            var baseProduct = new ProductInput
            {
                Name = "Original Product",
                Description = "Original description",
                Price = 999.99,
                QuantityAvailable = 1
            };

            var id = await service.CreateAsync(baseProduct);
            await service.UpdateByIdAsync(id, input);

            var product = await service.GetByIdAsync(id);
            Assert.True(product != null && product.Name == input.Name);
        }

        #endregion

        #region Exception Test Cases

        [Theory(DisplayName = "Creating invalid Products")]
        [MemberData(nameof(TestData.InvalidProducts), MemberType = typeof(TestData))]
        public async Task CreateInvalidProducts(ProductInput input)
        {
            var service = GetService();

            await Assert.ThrowsAsync<ArgumentException>(async () => await service.CreateAsync(input));
        }

        [Theory(DisplayName = "Updating invalid Products")]
        [MemberData(nameof(TestData.InvalidProducts), MemberType = typeof(TestData))]
        public async Task UpdateInvalidProducts(ProductInput input)
        {
            var service = GetService();
            var validProduct = new ProductInput
            {
                Name = "Valid Product",
                Description = "Description",
                Price = 200,
                QuantityAvailable = 10
            };

            var id = await service.CreateAsync(validProduct);

            await Assert.ThrowsAsync<ArgumentException>(async () => await service.UpdateByIdAsync(id, input));
        }

        [Fact(DisplayName = "Updating Product with non-existent ID")]
        public async Task UpdateProductWithNonExistentId()
        {
            var service = GetService();
            var validProduct = new ProductInput
            {
                Name = "Valid Product",
                Description = "Description",
                Price = 200,
                QuantityAvailable = 10
            };

            await Assert.ThrowsAsync<HttpRequestException>(async () => await service.UpdateByIdAsync(Guid.NewGuid(), validProduct));
        }

        [Fact(DisplayName = "Deleting Product with empty ID")]
        public async Task DeleteProductWithEmptyId()
        {
            var service = GetService();

            await Assert.ThrowsAsync<ArgumentException>(async () => await service.DeleteAsync(Guid.Empty));
        }

        [Fact(DisplayName = "Deleting Product with non-existent ID")]
        public async Task DeleteProductWithNonExistentId()
        {
            var service = GetService();

            await Assert.ThrowsAsync<HttpRequestException>(async () => await service.DeleteAsync(Guid.NewGuid()));
        }

        [Fact(DisplayName = "Getting Product by empty ID")]
        public async Task GetProductByEmptyId()
        {
            var service = GetService();

            await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetByIdAsync(Guid.Empty));
        }

        [Fact(DisplayName = "Getting Product by non-existent ID")]
        public async Task GetProductByNonExistentId()
        {
            var service = GetService();

            await Assert.ThrowsAsync<HttpRequestException>(async () => await service.GetByIdAsync(Guid.NewGuid()));
        }

        [Fact(DisplayName = "Getting Products with invalid filter")]
        public async Task GetProductsWithInvalidFilter()
        {
            var service = GetService();
            var filter = new SearchfilterProduct
            {
                PriceMin = 500,
                PriceMax = 100,
                QuantityAvailable = -1
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetAsync(filter));
        }

        #endregion
    }
}
