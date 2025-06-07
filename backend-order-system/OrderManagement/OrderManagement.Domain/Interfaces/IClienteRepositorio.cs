using OrderManagement.Dominio.Utils;

namespace OrderManagement.Dominio.Interfaces
{
    public interface IClienteRepositorio
    {
        public Task CriarAsync(Cliente cliente);
        public Task<Cliente?> ObterPorIdAsync(Guid id);
        public Task<IEnumerable<Cliente>> ObterAsync(ParametrosBuscaCliente filtro);
        public Task<bool> CheckEmailExistsAsync(string email);
        public Task AtualizarAsync(Cliente cliente);
        public Task DeletarAsync(Guid id);
    }
}
