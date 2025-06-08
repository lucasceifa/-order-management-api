using OrderManagement.API.Teste.Services.RepositorysMock;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Utils;
using OrderManagement.Repository;
using OrderManagement.Service;
using System.Data;
using Xunit;
using Xunit.Sdk;

namespace OrderManagement.API.Tests.Service
{
    [Collection("Costumer Service Test")]
    public class CostumerServiceTest
    {
        public CostumerService GetService()
        {
            var costumerRepository = new CostumerRepositoryMock();
            return new CostumerService(costumerRepository);
        }

        public static class TestData
        {
            public static IEnumerable<object[]> ValidCostumers =>
                new List<object[]>
                {
                    new object[]
                    {
                        new CostumerInput
                        {
                            Name = "Ana Souza",
                            Email = "ana.souza@email.com",
                            Cellphone = "11911112222"
                        }
                    },
                    new object[]
                    {
                        new CostumerInput
                        {
                            Name = "Carlos Mendes",
                            Email = "carlos.mendes@email.com",
                            Cellphone = "11922223333"
                        }
                    },
                    new object[]
                    {
                        new CostumerInput
                        {
                            Name = "Fernanda Lima",
                            Email = "fernanda.lima@email.com",
                            Cellphone = "11933334444"
                        }
                    },
                    new object[]
                    {
                        new CostumerInput
                        {
                            Name = "João Pedro",
                            Email = "joao.pedro@email.com",
                            Cellphone = "11944445555"
                        }
                    },
                    new object[]
                    {
                        new CostumerInput
                        {
                            Name = "Mariana Alves",
                            Email = "mariana.alves@email.com",
                            Cellphone = "11955556666"
                        }
                    }
                };

            public static IEnumerable<object[]> RepeatedEmailCostumers =>
                new List<object[]>
                {
                    new object[]
                    {
                        new CostumerInput
                        {
                            Name = "Lucas Trindade",
                            Email = "repeated@email.com",
                            Cellphone = "11911112222"
                        }
                    },
                    new object[]
                    {
                        new CostumerInput
                        {
                            Name = "Carlos Trindade",
                            Email = "repeated@email.com",
                            Cellphone = "11922223333"
                        }
                    }
                };

            public static IEnumerable<object[]> InvalidCostumers =>
                new List<object[]>
                {
                    new object[]
                    {
                        new CostumerInput
                        {
                            Name = "",
                            Email = "test1@email.com",
                            Cellphone = "11911112222"
                        }
                    },
                    new object[]
                    {
                        new CostumerInput
                        {
                            Name = "A",
                            Email = "test2@email.com",
                            Cellphone = "11911112222"
                        }
                    },
                    new object[]
                    {
                        new CostumerInput
                        {
                            Name = "Lucas",
                            Email = "test3@email.com",
                            Cellphone = "11911112222"
                        }
                    },
                    new object[]
                    {
                        new CostumerInput
                        {
                            Name = "Lucas Trindade",
                            Email = "",
                            Cellphone = "11911112222"
                        }
                    },
                    new object[]
                    {
                        new CostumerInput
                        {
                            Name = "Lucas Trindade",
                            Email = "a@b",
                            Cellphone = "11911112222"
                        }
                    },
                    new object[]
                    {
                        new CostumerInput
                        {
                            Name = "Lucas Trindade",
                            Email = "emailwithoutat.com",
                            Cellphone = "11911112222"
                        }
                    }
                };
        }

        #region Positive Validation Methods

        [Theory(DisplayName = "Creating valid Costumers")]
        [MemberData(nameof(TestData.ValidCostumers), MemberType = typeof(TestData))]
        public async Task CreateValidCostumers(CostumerInput input)
        {
            var service = GetService();
            await service.CreateAsync(input);

            var costumers = await service.GetAsync(new SearchfilterCostumer { });

            Assert.True(costumers.Any(c => c.Name == input.Name && c.Cellphone == input.Cellphone && c.Email == input.Email));
        }

        [Theory(DisplayName = "Creating and retrieving Costumer by ID")]
        [MemberData(nameof(TestData.ValidCostumers), MemberType = typeof(TestData))]
        public async Task GetByIdCostumer(CostumerInput input)
        {
            var service = GetService();
            var response = await service.CreateAsync(input);

            var costumer = await service.GetByIdAsync(response);

            Assert.True(costumer != null && costumer.Id == response && costumer.Name == input.Name && costumer.Cellphone == input.Cellphone && costumer.Email == input.Email);
        }

        [Theory(DisplayName = "Creating and deleting Costumer")]
        [MemberData(nameof(TestData.ValidCostumers), MemberType = typeof(TestData))]
        public async Task DeleteCostumerById(CostumerInput input)
        {
            var service = GetService();
            var response = await service.CreateAsync(input);

            var costumer = await service.GetByIdAsync(response);
            await service.DeleteAsync(response);
            var costumers = await service.GetAsync(new SearchfilterCostumer { });

            Assert.True(costumer != null && costumers.Count() == 0);
        }

        [Theory(DisplayName = "Creating and updating Costumer")]
        [MemberData(nameof(TestData.ValidCostumers), MemberType = typeof(TestData))]
        public async Task UpdateCostumerById(CostumerInput input)
        {
            var service = GetService();
            var baseInput = new CostumerInput
            {
                Name = "Test Update",
                Email = "test@email.com",
                Cellphone = "11111111"
            };

            var response = await service.CreateAsync(baseInput);
            await service.UpdateByIdAsync(response, input);

            var costumer = await service.GetByIdAsync(response);

            Assert.True(costumer != null && costumer.Name == input.Name && costumer.Cellphone == input.Cellphone && costumer.Email == input.Email);
        }

        #endregion

        #region Exception Validation Methods

        [Theory(DisplayName = "Creating invalid Costumers from VALIDATE method")]
        [MemberData(nameof(TestData.InvalidCostumers), MemberType = typeof(TestData))]
        public async Task CreateInvalidCostumers(CostumerInput input)
        {
            var service = GetService();
            await Assert.ThrowsAsync<ArgumentException>(async () => await service.CreateAsync(input));
        }

        [Theory(DisplayName = "Updating invalid Costumers from VALIDATE method")]
        [MemberData(nameof(TestData.InvalidCostumers), MemberType = typeof(TestData))]
        public async Task UpdateInvalidCostumers(CostumerInput input)
        {
            var service = GetService();

            var baseInput = new CostumerInput
            {
                Name = "Test Update",
                Email = "test@email.com",
                Cellphone = "11111111"
            };

            var response = await service.CreateAsync(baseInput);

            await Assert.ThrowsAsync<ArgumentException>(async () => await service.UpdateByIdAsync(response, input));
        }

        [Theory(DisplayName = "Updating Costumer with invalid ID")]
        [MemberData(nameof(TestData.ValidCostumers), MemberType = typeof(TestData))]
        public async Task UpdateCostumerWithInvalidId(CostumerInput input)
        {
            var service = GetService();
            await Assert.ThrowsAsync<HttpRequestException>(async () => await service.UpdateByIdAsync(Guid.NewGuid(), input));
        }

        [Fact(DisplayName = "Creating Costumer with duplicated email")]
        public async Task CreateCostumerWithDuplicatedEmail()
        {
            var service = GetService();

            var inputs = TestData.RepeatedEmailCostumers.Select(e => e.First() as CostumerInput).ToList();
            await service.CreateAsync(inputs.First());

            await Assert.ThrowsAsync<DuplicateNameException>(async () => await service.CreateAsync(inputs.Last()));
        }

        [Fact(DisplayName = "Updating Costumer with duplicated email")]
        public async Task UpdateCostumerWithDuplicatedEmail()
        {
            var service = GetService();

            var inputs = TestData.RepeatedEmailCostumers.Select(e => e.First() as CostumerInput).ToList();
            await service.CreateAsync(inputs.First());

            var baseInput = new CostumerInput
            {
                Name = "Test Update",
                Email = "test@email.com",
                Cellphone = "11111111"
            };

            var response = await service.CreateAsync(baseInput);

            await Assert.ThrowsAsync<DuplicateNameException>(async () => await service.UpdateByIdAsync(response, inputs.Last()));
        }

        [Fact(DisplayName = "Deleting Costumer with empty ID")]
        public async Task DeleteCostumerWithEmptyId()
        {
            var service = GetService();
            await Assert.ThrowsAsync<ArgumentException>(async () => await service.DeleteAsync(Guid.Empty));
        }

        [Fact(DisplayName = "Deleting Costumer with random ID")]
        public async Task DeleteCostumerWithRandomId()
        {
            var service = GetService();
            await Assert.ThrowsAsync<HttpRequestException>(async () => await service.DeleteAsync(Guid.NewGuid()));
        }

        [Fact(DisplayName = "Getting Costumer by empty ID")]
        public async Task GetCostumerByEmptyId()
        {
            var service = GetService();
            await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetByIdAsync(Guid.Empty));
        }

        [Fact(DisplayName = "Getting Costumer by random ID")]
        public async Task GetCostumerByRandomId()
        {
            var service = GetService();
            await Assert.ThrowsAsync<HttpRequestException>(async () => await service.GetByIdAsync(Guid.NewGuid()));
        }

        [Fact(DisplayName = "Getting Costumers with invalid filter")]
        public async Task GetCostumersWithInvalidFilter()
        {
            var service = GetService();
            await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetAsync(new SearchfilterCostumer() { CreationDate = DateTime.Now.AddDays(10) }));
        }

        #endregion
    }
}
