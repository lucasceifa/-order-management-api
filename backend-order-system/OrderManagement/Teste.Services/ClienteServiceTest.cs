using OrderManagement.API.Teste.Services.RepositoriosMock;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Utils;
using OrderManagement.Repositorio;
using OrderManagement.Servico;
using System.Data;
using Xunit;
using Xunit.Sdk;

namespace OrderManagement.API.Testes.Service
{

    [Collection("Teste do serviço de clientes")]
    public class ClienteServiceTest
    {
        public ClienteService ObterService()
        {
            var clienteRepositorio = new ClienteRepositorioMock();

            return new ClienteService(clienteRepositorio);
        }

        public static class Dados
        {
            public static IEnumerable<object[]> ClientesValidos =>
                new List<object[]>
                {
                    new object[]
                    {
                        new ClienteInput
                        {
                            Nome = "Ana Souza",
                            Email = "ana.souza@email.com",
                            Telefone = "11911112222"
                        }
                    },
                    new object[]
                    {
                        new ClienteInput
                        {
                            Nome = "Carlos Mendes",
                            Email = "carlos.mendes@email.com",
                            Telefone = "11922223333"
                        }
                    },
                    new object[]
                    {
                        new ClienteInput
                        {
                            Nome = "Fernanda Lima",
                            Email = "fernanda.lima@email.com",
                            Telefone = "11933334444"
                        }
                    },
                    new object[]
                    {
                        new ClienteInput
                        {
                            Nome = "João Pedro",
                            Email = "joao.pedro@email.com",
                            Telefone = "11944445555"
                        }
                    },
                    new object[]
                    {
                        new ClienteInput
                        {
                            Nome = "Mariana Alves",
                            Email = "mariana.alves@email.com",
                            Telefone = "11955556666"
                        }
                    }
                };

            public static IEnumerable<object[]> ClientesInvalidosEmailRepetido =>
                new List<object[]>
                {
                    new object[]
                    {
                        new ClienteInput
                        {
                            Nome = "Lucas Trindade",
                            Email = "repetido@email.com",
                            Telefone = "11911112222"
                        }
                    },
                    new object[]
                    {
                        new ClienteInput
                        {
                            Nome = "Carlos Trindade",
                            Email = "repetido@email.com",
                            Telefone = "11922223333"
                        }
                    }
                        };

          public static IEnumerable<object[]> ClientesInvalidos =>
                new List<object[]>
                {
                    new object[]
                    {
                        new ClienteInput
                        {
                            Nome = "",
                            Email = "teste1@email.com",
                            Telefone = "11911112222"
                        }
                    },
                    new object[]
                    {
                        new ClienteInput
                        {
                            Nome = "A",
                            Email = "teste2@email.com",
                            Telefone = "11911112222"
                        }
                    },
                    new object[]
                    {
                        new ClienteInput
                        {
                            Nome = "Lucas",
                            Email = "teste3@email.com",
                            Telefone = "11911112222"
                        }
                    },
                    new object[]
                    {
                        new ClienteInput
                        {
                            Nome = "Lucas Trindade",
                            Email = "",
                            Telefone = "11911112222"
                        }
                    },
                    new object[]
                    {
                        new ClienteInput
                        {
                            Nome = "Lucas Trindade",
                            Email = "a@b",
                            Telefone = "11911112222"
                        }
                    },
                    new object[]
                    {
                        new ClienteInput
                        {
                            Nome = "Lucas Trindade",
                            Email = "emailsemarroba.com",
                            Telefone = "11911112222"
                        }
                    }
                };
        }

        #region Metodos de validação com resultado positivo
        [Theory(DisplayName = "Criando clientes válidos")]
        [MemberData(nameof(Dados.ClientesValidos), MemberType = typeof(Dados))]
        public async Task CriarClientesValidos(ClienteInput input)
        {
            var service = ObterService();
            await service.CriarAsync(input);

            var clientes = await service.ObterAsync(new ParametrosBuscaCliente { });

            Assert.True(clientes.Any(c => c.Nome == input.Nome && c.Telefone == input.Telefone && c.Email == input.Email));
        }

        [Theory(DisplayName = "Criando cliente e dando GetPorId")]
        [MemberData(nameof(Dados.ClientesValidos), MemberType = typeof(Dados))]
        public async Task UsandoGetByID(ClienteInput input)
        {
            var service = ObterService();
            var response = await service.CriarAsync(input);

            var clienteGetById = await service.ObterPorIdAsync(response);

            Assert.True(clienteGetById != null && clienteGetById.Id == response && clienteGetById.Nome == input.Nome && clienteGetById.Telefone == input.Telefone && clienteGetById.Email == input.Email);
        }

        [Theory(DisplayName = "Criando cliente e excluindo")]
        [MemberData(nameof(Dados.ClientesValidos), MemberType = typeof(Dados))]
        public async Task UsandoDeleteByID(ClienteInput input)
        {
            var service = ObterService();
            var response = await service.CriarAsync(input);

            var cliente = await service.ObterPorIdAsync(response);

            await service.DeletarAsync(response);

            var clientes = await service.ObterAsync(new ParametrosBuscaCliente { });

            Assert.True(cliente != null && clientes.Count() == 0);
        }

        [Theory(DisplayName = "Criando cliente e atualizando")]
        [MemberData(nameof(Dados.ClientesValidos), MemberType = typeof(Dados))]
        public async Task UsandoUpdateByID(ClienteInput input)
        {
            var service = ObterService();
            var inputPadrao = new ClienteInput
            {
                Nome = "Teste para atualizar",
                Email = "teste@email.com",
                Telefone = "11111111"
            };

            var response = await service.CriarAsync(inputPadrao);
            await service.AtualizarPorIdAsync(response, input);

            var cliente = await service.ObterPorIdAsync(response);

            Assert.True(cliente != null && cliente.Nome == input.Nome && cliente.Telefone == input.Telefone && cliente.Email == input.Email);
        }
        #endregion

        #region Metodos de validação com retorno de excessão
        [Theory(DisplayName = "Criando clientes inválidos do método VALIDATE")]
        [MemberData(nameof(Dados.ClientesInvalidos), MemberType = typeof(Dados))]
        public async Task CriarClientesInvalidos(ClienteInput input)
        {
            var service = ObterService();

            await Assert.ThrowsAsync<ArgumentException>(async () => await service.CriarAsync(input));
        }

        [Theory(DisplayName = "Atualizando clientes inválidos do método VALIDATE")]
        [MemberData(nameof(Dados.ClientesInvalidos), MemberType = typeof(Dados))]
        public async Task AtualizandoClientesInvalidos(ClienteInput input)
        {
            var service = ObterService();

            var inputPadrao = new ClienteInput
            {
                Nome = "Teste para atualizar",
                Email = "teste@email.com",
                Telefone = "11111111"
            };
            var response = await service.CriarAsync(inputPadrao);

            await Assert.ThrowsAsync<ArgumentException>(async () => await service.AtualizarPorIdAsync(response, input));
        }

        [Theory(DisplayName = "Atualizando clientes com Id inválido")]
        [MemberData(nameof(Dados.ClientesValidos), MemberType = typeof(Dados))]
        public async Task AtualizandoClientesComId(ClienteInput input)
        {
            var service = ObterService();

            await Assert.ThrowsAsync<HttpRequestException>(async () => await service.AtualizarPorIdAsync(Guid.NewGuid(), input));
        }

        [Fact(DisplayName = "Criando clientes com email duplicado")]
        public async Task CriarClientesInvalidosComEmailDuplicado()
        {
            var service = ObterService();

            var inputs = Dados.ClientesInvalidosEmailRepetido.Select(e => e.First() as ClienteInput).ToList();
            await service.CriarAsync(inputs.First());

            await Assert.ThrowsAsync<DuplicateNameException>(async () => await service.CriarAsync(inputs.Last()));
        }

        [Fact(DisplayName = "Atualizando clientes com email duplicado")]
        public async Task AtualizandoClientesInvalidosComEmailDuplicado()
        {
            var service = ObterService();

            var inputs = Dados.ClientesInvalidosEmailRepetido.Select(e => e.First() as ClienteInput).ToList();
            await service.CriarAsync(inputs.First());

            var inputPadrao = new ClienteInput
            {
                Nome = "Teste para atualizar",
                Email = "teste@email.com",
                Telefone = "11111111"
            };
            var response = await service.CriarAsync(inputPadrao);


            await Assert.ThrowsAsync<DuplicateNameException>(async () => await service.AtualizarPorIdAsync(response, inputs.Last()));
        }

        [Fact(DisplayName = "Deletando clientes com Id vazio")]
        public async Task DeletandoClientesComIdVazio()
        {
            var service = ObterService();

            await Assert.ThrowsAsync<ArgumentException>(async () => await service.DeletarAsync(Guid.Empty));
        }

        [Fact(DisplayName = "Deletando clientes com Id inválido aleatório")]
        public async Task DeletandoClientesComIdAleatorio()
        {
            var service = ObterService();

            await Assert.ThrowsAsync<HttpRequestException>(async () => await service.DeletarAsync(Guid.NewGuid()));
        }

        [Fact(DisplayName = "Obter por ID clientes com Id vazio")]
        public async Task GetByIdClientesComIdVazio()
        {
            var service = ObterService();

            await Assert.ThrowsAsync<ArgumentException>(async () => await service.ObterPorIdAsync(Guid.Empty));
        }

        [Fact(DisplayName = "Obter por ID clientes com Id aleatório")]
        public async Task GetByIdClientesComIdAleatorio()
        {
            var service = ObterService();

            await Assert.ThrowsAsync<HttpRequestException>(async () => await service.ObterPorIdAsync(Guid.NewGuid()));
        }

        [Fact(DisplayName = "Obter clientes com filtro inválido")]
        public async Task GetClientesComFiltroInvalido()
        {
            var service = ObterService();

            await Assert.ThrowsAsync<ArgumentException>(async () => await service.ObterAsync(new ParametrosBuscaCliente() { DataDeCadastro = DateTime.Now.AddDays(10) }));
        }
        #endregion
    }
}
