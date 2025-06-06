using OrderManagement.Domain;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Interfaces;

namespace OrderManagement.Servico
{
    public class ProdutoService
    {
        private readonly IProdutoRepositorio _repProduto;

        public ProdutoService(IProdutoRepositorio repProduto)
        { 
            _repProduto = repProduto;
        }

        public async Task AtualizarPorIdAsync(Produto Produto)
        {
            await _repProduto.AtualizarAsync(Produto);
        }

        public async Task CriarAsync(Produto Produto)
        {
            await _repProduto.CriarAsync(Produto);
        }

        public async Task DeletarAsync(Guid id)
        {
            await _repProduto.DeletarAsync(id);
        }

        public async Task<Produto> ObterPorIdAsync(Guid id)
        {
            return await _repProduto.ObterPorIdAsync(id);
        }

        public async Task<IEnumerable<Produto>> ObterAsync()
        {
            return await _repProduto.ObterAsync();
        }
    }
}
