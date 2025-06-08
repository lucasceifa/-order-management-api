using OrderManagement.Dominio;
using OrderManagement.Dominio.Requests;
using OrderManagement.Dominio.Utils;
using OrderManagement.Servico;
using OrderManagement.API.Teste.Services.RepositoriosMock;
using Xunit;

namespace OrderManagement.API.Testes.Service
{
    [Collection("Teste do serviço de produtos")]
    public class ProdutoServiceTest
    {
        public ProdutoService ObterService()
        {
            var produtoRepositorio = new ProdutoRepositorioMock();
            return new ProdutoService(produtoRepositorio);
        }

        public static class Dados
        {
            public static IEnumerable<object[]> ProdutosValidos =>
                new List<object[]>
                {
                    new object[]
                    {
                        new ProdutoInput
                        {
                            Nome = "Notebook Dell",
                            Descricao = "Modelo Inspiron 15",
                            Preco = 4500.00,
                            QuantidadeDisponivel = 10
                        }
                    },
                    new object[]
                    {
                        new ProdutoInput
                        {
                            Nome = "Mouse Logitech",
                            Descricao = "Sem fio",
                            Preco = 150.00,
                            QuantidadeDisponivel = 50
                        }
                    },
                    new object[]
                    {
                        new ProdutoInput
                        {
                            Nome = "Teclado Mecânico",
                            Descricao = "ABNT2 RGB",
                            Preco = 300.00,
                            QuantidadeDisponivel = 20
                        }
                    },
                    new object[]
                    {
                        new ProdutoInput
                        {
                            Nome = "Monitor LG",
                            Descricao = "27 polegadas 4K",
                            Preco = 1200.00,
                            QuantidadeDisponivel = 5
                        }
                    },
                    new object[]
                    {
                        new ProdutoInput
                        {
                            Nome = "Headset HyperX",
                            Descricao = "Cloud Stinger",
                            Preco = 400.00,
                            QuantidadeDisponivel = 15
                        }
                    }
                };

            public static IEnumerable<object[]> ProdutosInvalidos =>
                new List<object[]>
                {
                    new object[]
                    {
                        new ProdutoInput
                        {
                            Nome = "",
                            Descricao = "Teste",
                            Preco = 100,
                            QuantidadeDisponivel = 10
                        }
                    },
                    new object[]
                    {
                        new ProdutoInput
                        {
                            Nome = "A",
                            Descricao = "Teste",
                            Preco = 100,
                            QuantidadeDisponivel = 10
                        }
                    },
                    new object[]
                    {
                        new ProdutoInput
                        {
                            Nome = "Produto Sem Preço",
                            Descricao = "Teste",
                            Preco = -1,
                            QuantidadeDisponivel = 10
                        }
                    },
                    new object[]
                    {
                        new ProdutoInput
                        {
                            Nome = "Produto Sem Estoque",
                            Descricao = "Teste",
                            Preco = 100,
                            QuantidadeDisponivel = -5
                        }
                    }
                };
        }

        #region Métodos de validação com resultado positivo
        [Theory(DisplayName = "Criando produtos válidos")]
        [MemberData(nameof(Dados.ProdutosValidos), MemberType = typeof(Dados))]
        public async Task CriarProdutosValidos(ProdutoInput input)
        {
            var service = ObterService();
            await service.CriarAsync(input);

            var produtos = await service.ObterAsync(new ParametrosBuscaProduto { });

            Assert.True(produtos.Any(p => p.Nome == input.Nome && p.Preco == input.Preco && p.QuantidadeDisponivel == input.QuantidadeDisponivel));
        }

        [Theory(DisplayName = "Criando produto e dando GetPorId")]
        [MemberData(nameof(Dados.ProdutosValidos), MemberType = typeof(Dados))]
        public async Task UsandoGetByID(ProdutoInput input)
        {
            var service = ObterService();
            var response = await service.CriarAsync(input);

            var produto = await service.ObterPorIdAsync(response);

            Assert.True(produto != null && produto.Id == response);
        }

        [Theory(DisplayName = "Criando produto e excluindo")]
        [MemberData(nameof(Dados.ProdutosValidos), MemberType = typeof(Dados))]
        public async Task UsandoDeleteByID(ProdutoInput input)
        {
            var service = ObterService();
            var response = await service.CriarAsync(input);

            await service.DeletarAsync(response);
            var produtos = await service.ObterAsync(new ParametrosBuscaProduto { });

            Assert.True(produtos.All(p => p.Id != response));
        }

        [Theory(DisplayName = "Criando produto e atualizando")]
        [MemberData(nameof(Dados.ProdutosValidos), MemberType = typeof(Dados))]
        public async Task UsandoUpdateByID(ProdutoInput input)
        {
            var service = ObterService();
            var produtoBase = new ProdutoInput
            {
                Nome = "Produto Original",
                Descricao = "Descrição original",
                Preco = 999.99,
                QuantidadeDisponivel = 1
            };

            var id = await service.CriarAsync(produtoBase);
            await service.AtualizarPorIdAsync(id, input);

            var produto = await service.ObterPorIdAsync(id);
            Assert.True(produto != null && produto.Nome == input.Nome);
        }

        #endregion

        #region Métodos de validação com retorno de exceção
        [Theory(DisplayName = "Criando produtos inválidos")]
        [MemberData(nameof(Dados.ProdutosInvalidos), MemberType = typeof(Dados))]
        public async Task CriarProdutosInvalidos(ProdutoInput input)
        {
            var service = ObterService();

            await Assert.ThrowsAsync<ArgumentException>(async () => await service.CriarAsync(input));
        }

        [Theory(DisplayName = "Atualizando produtos inválidos")]
        [MemberData(nameof(Dados.ProdutosInvalidos), MemberType = typeof(Dados))]
        public async Task AtualizarProdutosInvalidos(ProdutoInput input)
        {
            var service = ObterService();
            var produtoValido = new ProdutoInput
            {
                Nome = "Produto válido",
                Descricao = "Descrição",
                Preco = 200,
                QuantidadeDisponivel = 10
            };

            var id = await service.CriarAsync(produtoValido);

            await Assert.ThrowsAsync<ArgumentException>(async () => await service.AtualizarPorIdAsync(id, input));
        }

        [Fact(DisplayName = "Atualizando produto com ID inexistente")]
        public async Task AtualizandoProdutoComIdInexistente()
        {
            var service = ObterService();
            var produtoValido = new ProdutoInput
            {
                Nome = "Produto válido",
                Descricao = "Descrição",
                Preco = 200,
                QuantidadeDisponivel = 10
            };

            await Assert.ThrowsAsync<HttpRequestException>(async () => await service.AtualizarPorIdAsync(Guid.NewGuid(), produtoValido));
        }

        [Fact(DisplayName = "Deletando produto com ID inválido")]
        public async Task DeletarProdutoComIdInvalido()
        {
            var service = ObterService();

            await Assert.ThrowsAsync<ArgumentException>(async () => await service.DeletarAsync(Guid.Empty));
        }

        [Fact(DisplayName = "Deletando produto com ID inexistente")]
        public async Task DeletarProdutoComIdInexistente()
        {
            var service = ObterService();

            await Assert.ThrowsAsync<HttpRequestException>(async () => await service.DeletarAsync(Guid.NewGuid()));
        }

        [Fact(DisplayName = "Obter produto por ID inválido")]
        public async Task GetProdutoPorIdInvalido()
        {
            var service = ObterService();

            await Assert.ThrowsAsync<ArgumentException>(async () => await service.ObterPorIdAsync(Guid.Empty));
        }

        [Fact(DisplayName = "Obter produto por ID inexistente")]
        public async Task GetProdutoPorIdInexistente()
        {
            var service = ObterService();

            await Assert.ThrowsAsync<HttpRequestException>(async () => await service.ObterPorIdAsync(Guid.NewGuid()));
        }

        [Fact(DisplayName = "Obter produtos com filtro inválido")]
        public async Task GetProdutosComFiltroInvalido()
        {
            var service = ObterService();
            var filtro = new ParametrosBuscaProduto
            {
                PrecoMin = 500,
                PrecoMax = 100,
                QuantidadeDisponivel = -1
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await service.ObterAsync(filtro));
        }
        #endregion
    }
}
