using OrderManagement.Domain;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Interfaces;
using OrderManagement.Dominio.Requests;
using OrderManagement.Dominio.Utils;

namespace OrderManagement.Servico
{
    public class ProdutoService
    {
        private readonly IProdutoRepositorio _repProduto;

        public ProdutoService(IProdutoRepositorio repProduto)
        {
            _repProduto = repProduto;
        }

        public async Task AtualizarPorIdAsync(Guid id, ProdutoInput request)
        {
            if (!request.Validate())
                throw new ArgumentException("Formulário de preenchimento inválido");

            var produtoOriginal = await _repProduto.ObterPorIdAsync(id);
            if (produtoOriginal == null)
                throw new HttpRequestException("Produto não encontrado");

            produtoOriginal.Nome = request.Nome;
            produtoOriginal.Descricao = request.Descricao;
            produtoOriginal.Preco = request.Preco;
            produtoOriginal.QuantidadeDisponivel = request.QuantidadeDisponivel;

            await _repProduto.AtualizarAsync(produtoOriginal);
        }

        public async Task<Guid> CriarAsync(ProdutoInput request)
        {
            if (!request.Validate())
                throw new ArgumentException("Formulário de preenchimento inválido");

            var novoProduto = new Produto(request);

            await _repProduto.CriarAsync(novoProduto);

            return novoProduto.Id;
        }

        public async Task DeletarAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID inválido");

            var produto = await _repProduto.ObterPorIdAsync(id);
            if (produto == null)
                throw new HttpRequestException("Produto não encontrado");

            await _repProduto.DeletarAsync(id);
        }

        public async Task<Produto> ObterPorIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID inválido");

            var produto = await _repProduto.ObterPorIdAsync(id);
            if (produto == null)
                throw new HttpRequestException("Produto não encontrado");

            return produto;
        }

        public async Task<IEnumerable<Produto>> ObterAsync(ParametrosBuscaProduto filtro)
        {
            if (filtro.PrecoMin > filtro.PrecoMax)
                throw new ArgumentException("O preço mínimo não pode ser maior que o preço máximo do produto");

            if (filtro.PrecoMax < 0 || filtro.PrecoMin < 0 || filtro.QuantidadeDisponivel < 0)
                throw new ArgumentException("Os numéricos não podem incluir valores negativos");

            return await _repProduto.ObterAsync(filtro);
        }
    }
}
