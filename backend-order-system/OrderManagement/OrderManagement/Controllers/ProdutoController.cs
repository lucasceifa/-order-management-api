using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrderManagement.Domain;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Requests;
using OrderManagement.Dominio.Utils;
using OrderManagement.Servico;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoService _servProduto;

        public ProdutoController(ProdutoService servProduto)
        {
            _servProduto = servProduto;
        }

        [HttpGet]
        public async Task<IActionResult> ObterProdutos([FromBody] ParametrosBuscaProduto filtro)
        {
            var Produtos = await _servProduto.ObterAsync(filtro);
            if (Produtos.IsNullOrEmpty())
                return NotFound();

            return Ok(Produtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterProdutoPorId(Guid id)
        {
            var Produto = await _servProduto.ObterPorIdAsync(id);
            if (Produto == null)
                return NotFound();

            return Ok(Produto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarProduto(Guid id, [FromBody] ProdutoInput request)
        {
            await _servProduto.AtualizarPorIdAsync(id, request);

            return Ok();
        }

        [HttpPost()]
        public async Task<IActionResult> Post([FromBody] ProdutoInput request)
        {
            await _servProduto.CriarAsync(request);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarProdutoPorId(Guid id)
        {
            await _servProduto.DeletarAsync(id);

            return Ok();
        }
    }
}
