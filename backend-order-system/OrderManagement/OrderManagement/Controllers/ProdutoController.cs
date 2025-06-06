using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrderManagement.Domain;
using OrderManagement.Dominio;
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
        public async Task<IActionResult> ObterProdutos()
        {
            var Produtos = await _servProduto.ObterAsync();
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Produto request)
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
