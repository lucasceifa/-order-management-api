using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Utils;
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
        public async Task<IActionResult> ObterClientes([FromQuery] ParametrosBuscaCliente filtro)
        {
            var clientes = await _servCliente.ObterAsync(filtro);
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
        public async Task<IActionResult> Post([FromBody] ClienteInput request)
        {
            await _servCliente.CriarAsync(request);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ClienteInput request)
        {
            await _servCliente.AtualizarPorIdAsync(id, request);

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
