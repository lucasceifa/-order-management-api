using OrderManagement.Dominio;
using OrderManagement.Dominio.Interfaces;
using OrderManagement.Dominio.Utils;

namespace OrderManagement.API.Teste.Services.RepositoriosMock
{
    public class ClienteRepositorioMock : IClienteRepositorio
    {
        private List<Cliente> _dados;

        public ClienteRepositorioMock()
        {
            if (_dados == null)
                _dados = new List<Cliente>();
        }

        public async Task AtualizarAsync(Cliente cliente)
        {
            var index = _dados.FindIndex(c => c.Id == cliente.Id);
            _dados[index] = cliente;
        }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            return _dados.Any(c => c.Email == email);
        }

        public async Task CriarAsync(Cliente cliente)
        {
            _dados.Add(cliente);
        }

        public async Task DeletarAsync(Guid id)
        {
            _dados = _dados.Where(c => c.Id != id).ToList();
        }

        public async Task<IEnumerable<Cliente>> ObterAsync(ParametrosBuscaCliente filtro)
        {
            return _dados.Where(e => (!filtro.DataDeCadastro.HasValue || e.DataDeCadastro > filtro.DataDeCadastro) && (String.IsNullOrEmpty(filtro.Email) || e.Email.ToLower().Contains(filtro.Email)) || (String.IsNullOrEmpty(filtro.Nome) || e.Nome.ToLower().Contains(filtro.Nome)));
        }

        public async Task<Cliente?> ObterPorIdAsync(Guid id)
        {
            return _dados.FirstOrDefault(c => c.Id == id);
        }
    }
}
