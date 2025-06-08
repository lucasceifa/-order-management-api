using System.Data;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Interfaces;
using OrderManagement.Dominio.Utils;

namespace OrderManagement.Servico
{
    public class ClienteService
    {
        private readonly IClienteRepositorio _repCliente;

        public ClienteService(IClienteRepositorio repCliente)
        { 
            _repCliente = repCliente;
        }

        public async Task AtualizarPorIdAsync(Guid id, ClienteInput request)
        {
            if (!request.Validate())
                throw new ArgumentException("Formulário de preenchimento inválido");

            var clienteOriginal = await _repCliente.ObterPorIdAsync(id);
            if (clienteOriginal == null)
                throw new HttpRequestException("Cliente não encontrado");

            if (request.Email != clienteOriginal.Email && await _repCliente.CheckEmailExistsAsync(request.Email))
                throw new DuplicateNameException("Email já está em uso por outro cliente");

            clienteOriginal.Email = request.Email;
            clienteOriginal.Telefone = request.Telefone;
            clienteOriginal.Nome = request.Nome;

            await _repCliente.AtualizarAsync(clienteOriginal);
        }

        public async Task<Guid> CriarAsync(ClienteInput request)
        {
            if (!request.Validate())
                throw new ArgumentException("Formulário de preenchimento inválido");

            if (await _repCliente.CheckEmailExistsAsync(request.Email))
                throw new DuplicateNameException("Email já está em uso por outro cliente");

            var newCliente = new Cliente(request);

            await _repCliente.CriarAsync(newCliente);

            return newCliente.Id;
        }

        public async Task DeletarAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID inválido");

            var cliente = await _repCliente.ObterPorIdAsync(id);
            if (cliente == null)
                throw new HttpRequestException("Cliente não encontrado");

            await _repCliente.DeletarAsync(id);
        }

        public async Task<Cliente> ObterPorIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID inválido");

            var response = await _repCliente.ObterPorIdAsync(id);
            if (response == null)
                throw new HttpRequestException("Cliente não encontrado");

            return response;
        }

        public async Task<IEnumerable<Cliente>> ObterAsync(ParametrosBuscaCliente filtro)
        {
            if (filtro.DataDeCadastro.HasValue && filtro.DataDeCadastro.Value > DateTime.Now)
                throw new ArgumentException("Data de cadastro não pode ser superior a data atual");

            return await _repCliente.ObterAsync(filtro);
        }
    }
}
