using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrderManagement.Dominio;
using OrderManagement.Servico;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _servCliente;

        public ClienteController(ClienteService servCliente)
        {
            _servCliente = servCliente;
        }

        [HttpGet]
        public async Task<IActionResult> ObterClientes()
        {
            var clientes = await _servCliente.ObterAsync();
            if (clientes.IsNullOrEmpty())
                return NotFound();

            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterClientePorId(Guid id)
        {
            var cliente = await _servCliente.ObterPorIdAsync(id);
            if (cliente == null)
                return NotFound();

            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cliente request)
        {
            await _servCliente.CriarAsync(request);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarClientePorId(Guid id)
        {
            await _servCliente.DeletarAsync(id);

            return Ok();
        }
    }
}
