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
    [Collection("Customer Service Test")]
    public class CustomerServiceTest
    {
        public CustomerService GetService()
        {
            var CustomerRepository = new CustomerRepositoryMock();
            return new CustomerService(CustomerRepository);
        }

        public static class TestData
        {
            public static IEnumerable<object[]> ValidCustomers =>
                new List<object[]>
                {
                    new object[]
                    {
                        new CustomerInput
                        {
                            Name = "Ana Souza",
                            Email = "ana.souza@email.com",
                            Cellphone = "11911112222"
                        }
                    },
                    new object[]
                    {
                        new CustomerInput
                        {
                            Name = "Carlos Mendes",
                            Email = "carlos.mendes@email.com",
                            Cellphone = "11922223333"
                        }
                    },
                    new object[]
                    {
                        new CustomerInput
                        {
                            Name = "Fernanda Lima",
                            Email = "fernanda.lima@email.com",
                            Cellphone = "11933334444"
                        }
                    },
                    new object[]
                    {
                        new CustomerInput
                        {
                            Name = "João Pedro",
                            Email = "joao.pedro@email.com",
                            Cellphone = "11944445555"
                        }
                    },
                    new object[]
                    {
                        new CustomerInput
                        {
                            Name = "Mariana Alves",
                            Email = "mariana.alves@email.com",
                            Cellphone = "11955556666"
                        }
                    }
                };

            public static IEnumerable<object[]> RepeatedEmailCustomers =>
                new List<object[]>
                {
                    new object[]
                    {
                        new CustomerInput
                        {
                            Name = "Lucas Trindade",
                            Email = "repeated@email.com",
                            Cellphone = "11911112222"
                        }
                    },
                    new object[]
                    {
                        new CustomerInput
                        {
                            Name = "Carlos Trindade",
                            Email = "repeated@email.com",
                            Cellphone = "11922223333"
                        }
                    }
                };

            public static IEnumerable<object[]> InvalidCustomers =>
                new List<object[]>
                {
                    new object[]
                    {
                        new CustomerInput
                        {
                            Name = "",
                            Email = "test1@email.com",
                            Cellphone = "11911112222"
                        }
                    },
                    new object[]
                    {
                        new CustomerInput
                        {
                            Name = "A",
                            Email = "test2@email.com",
                            Cellphone = "11911112222"
                        }
                    },
                    new object[]
                    {
                        new CustomerInput
                        {
                            Name = "Lucas",
                            Email = "test3@email.com",
                            Cellphone = "11911112222"
                        }
                    },
                    new object[]
                    {
                        new CustomerInput
                        {
                            Name = "Lucas Trindade",
                            Email = "",
                            Cellphone = "11911112222"
                        }
                    },
                    new object[]
                    {
                        new CustomerInput
                        {
                            Name = "Lucas Trindade",
                            Email = "a@b",
                            Cellphone = "11911112222"
                        }
                    },
                    new object[]
                    {
                        new CustomerInput
                        {
                            Name = "Lucas Trindade",
                            Email = "emailwithoutat.com",
                            Cellphone = "11911112222"
                        }
                    }
                };
        }

        #region Positive Validation Methods

        [Theory(DisplayName = "Creating valid Customers")]
        [MemberData(nameof(TestData.ValidCustomers), MemberType = typeof(TestData))]
        public async Task CreateValidCustomers(CustomerInput input)
        {
            var service = GetService();
            await service.CreateAsync(input);

            var Customers = await service.GetAsync(new SearchfilterCustomer { });

            Assert.True(Customers.Any(c => c.Name == input.Name && c.Cellphone == input.Cellphone && c.Email == input.Email));
        }

        [Theory(DisplayName = "Creating and retrieving Customer by ID")]
        [MemberData(nameof(TestData.ValidCustomers), MemberType = typeof(TestData))]
        public async Task GetByIdCustomer(CustomerInput input)
        {
            var service = GetService();
            var response = await service.CreateAsync(input);

            var Customer = await service.GetByIdAsync(response);

            Assert.True(Customer != null && Customer.Id == response && Customer.Name == input.Name && Customer.Cellphone == input.Cellphone && Customer.Email == input.Email);
        }

        [Theory(DisplayName = "Creating and deleting Customer")]
        [MemberData(nameof(TestData.ValidCustomers), MemberType = typeof(TestData))]
        public async Task DeleteCustomerById(CustomerInput input)
        {
            var service = GetService();
            var response = await service.CreateAsync(input);

            var Customer = await service.GetByIdAsync(response);
            await service.DeleteAsync(response);
            var Customers = await service.GetAsync(new SearchfilterCustomer { });

            Assert.True(Customer != null && Customers.Count() == 0);
        }

        [Theory(DisplayName = "Creating and updating Customer")]
        [MemberData(nameof(TestData.ValidCustomers), MemberType = typeof(TestData))]
        public async Task UpdateCustomerById(CustomerInput input)
        {
            var service = GetService();
            var baseInput = new CustomerInput
            {
                Name = "Test Update",
                Email = "test@email.com",
                Cellphone = "11111111"
            };

            var response = await service.CreateAsync(baseInput);
            await service.UpdateByIdAsync(response, input);

            var Customer = await service.GetByIdAsync(response);

            Assert.True(Customer != null && Customer.Name == input.Name && Customer.Cellphone == input.Cellphone && Customer.Email == input.Email);
        }

        #endregion

        #region Exception Validation Methods

        [Theory(DisplayName = "Creating invalid Customers from VALIDATE method")]
        [MemberData(nameof(TestData.InvalidCustomers), MemberType = typeof(TestData))]
        public async Task CreateInvalidCustomers(CustomerInput input)
        {
            var service = GetService();
            await Assert.ThrowsAsync<ArgumentException>(async () => await service.CreateAsync(input));
        }

        [Theory(DisplayName = "Updating invalid Customers from VALIDATE method")]
        [MemberData(nameof(TestData.InvalidCustomers), MemberType = typeof(TestData))]
        public async Task UpdateInvalidCustomers(CustomerInput input)
        {
            var service = GetService();

            var baseInput = new CustomerInput
            {
                Name = "Test Update",
                Email = "test@email.com",
                Cellphone = "11111111"
            };

            var response = await service.CreateAsync(baseInput);

            await Assert.ThrowsAsync<ArgumentException>(async () => await service.UpdateByIdAsync(response, input));
        }

        [Theory(DisplayName = "Updating Customer with invalid ID")]
        [MemberData(nameof(TestData.ValidCustomers), MemberType = typeof(TestData))]
        public async Task UpdateCustomerWithInvalidId(CustomerInput input)
        {
            var service = GetService();
            await Assert.ThrowsAsync<HttpRequestException>(async () => await service.UpdateByIdAsync(Guid.NewGuid(), input));
        }

        [Fact(DisplayName = "Creating Customer with duplicated email")]
        public async Task CreateCustomerWithDuplicatedEmail()
        {
            var service = GetService();

            var inputs = TestData.RepeatedEmailCustomers.Select(e => e.First() as CustomerInput).ToList();
            await service.CreateAsync(inputs.First());

            await Assert.ThrowsAsync<DuplicateNameException>(async () => await service.CreateAsync(inputs.Last()));
        }

        [Fact(DisplayName = "Updating Customer with duplicated email")]
        public async Task UpdateCustomerWithDuplicatedEmail()
        {
            var service = GetService();

            var inputs = TestData.RepeatedEmailCustomers.Select(e => e.First() as CustomerInput).ToList();
            await service.CreateAsync(inputs.First());

            var baseInput = new CustomerInput
            {
                Name = "Test Update",
                Email = "test@email.com",
                Cellphone = "11111111"
            };

            var response = await service.CreateAsync(baseInput);

            await Assert.ThrowsAsync<DuplicateNameException>(async () => await service.UpdateByIdAsync(response, inputs.Last()));
        }

        [Fact(DisplayName = "Deleting Customer with empty ID")]
        public async Task DeleteCustomerWithEmptyId()
        {
            var service = GetService();
            await Assert.ThrowsAsync<ArgumentException>(async () => await service.DeleteAsync(Guid.Empty));
        }

        [Fact(DisplayName = "Deleting Customer with random ID")]
        public async Task DeleteCustomerWithRandomId()
        {
            var service = GetService();
            await Assert.ThrowsAsync<HttpRequestException>(async () => await service.DeleteAsync(Guid.NewGuid()));
        }

        [Fact(DisplayName = "Getting Customer by empty ID")]
        public async Task GetCustomerByEmptyId()
        {
            var service = GetService();
            await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetByIdAsync(Guid.Empty));
        }

        [Fact(DisplayName = "Getting Customer by random ID")]
        public async Task GetCustomerByRandomId()
        {
            var service = GetService();
            await Assert.ThrowsAsync<HttpRequestException>(async () => await service.GetByIdAsync(Guid.NewGuid()));
        }

        [Fact(DisplayName = "Getting Customers with invalid filter")]
        public async Task GetCustomersWithInvalidFilter()
        {
            var service = GetService();
            await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetAsync(new SearchfilterCustomer() { CreationDate = DateTime.Now.AddDays(10) }));
        }

        #endregion
    }
}
