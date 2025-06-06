using OrderManagement.Dominio;
using OrderManagement.Dominio.Interfaces;

namespace OrderManagement.Servico
{
    public class ClienteService
    {
        private readonly IClienteRepositorio _repCliente;

        public ClienteService(IClienteRepositorio repCliente)
        { 
            _repCliente = repCliente;
        }

        public async Task AtualizarPorIdAsync(Cliente cliente)
        {
            await _repCliente.AtualizarAsync(cliente);
        }

        public async Task CriarAsync(Cliente cliente)
        {
            await _repCliente.CriarAsync(cliente);
        }

        public async Task DeletarAsync(Guid id)
        {
            await _repCliente.DeletarAsync(id);
        }

        public async Task<Cliente> ObterPorIdAsync(Guid id)
        {
            return await _repCliente.ObterPorIdAsync(id);
        }

        public async Task<IEnumerable<Cliente>> ObterAsync()
        {
            return await _repCliente.ObterAsync();
        }
    }
}
