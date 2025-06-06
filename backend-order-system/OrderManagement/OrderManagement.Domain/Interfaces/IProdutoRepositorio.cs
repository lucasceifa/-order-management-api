using OrderManagement.Domain;

namespace OrderManagement.Dominio.Interfaces
{
    public interface IProdutoRepositorio
    {
        public Task CriarAsync(Produto Produto);
        public Task<Produto> ObterPorIdAsync(Guid id);
        public Task<IEnumerable<Produto>> ObterAsync();
        public Task AtualizarAsync(Produto Produto);
        public Task DeletarAsync(Guid id);
    }
}
