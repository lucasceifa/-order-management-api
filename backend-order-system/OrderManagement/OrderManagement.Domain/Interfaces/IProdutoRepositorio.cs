using OrderManagement.Domain;
using OrderManagement.Dominio.Utils;

namespace OrderManagement.Dominio.Interfaces
{
    public interface IProdutoRepositorio
    {
        public Task CriarAsync(Produto Produto);
        public Task<Produto> ObterPorIdAsync(Guid id);
        public Task<IEnumerable<Produto>> ObterAsync(ParametrosBuscaProduto filtro);
        public Task AtualizarAsync(Produto Produto);
        public Task DeletarAsync(Guid id);
    }
}
