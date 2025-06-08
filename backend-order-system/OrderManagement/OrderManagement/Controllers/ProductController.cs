using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrderManagement.Domain;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Requests;
using OrderManagement.Dominio.Utils;
using OrderManagement.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _servProduct;

        public ProductController(ProductService servProduct)
        {
            _servProduct = servProduct;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] SearchfilterProduct filter)
        {
            var Products = await _servProduct.GetAsync(filter);
            if (Products.IsNullOrEmpty())
                return NotFound();

            return Ok(Products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var Product = await _servProduct.GetByIdAsync(id);
            if (Product == null)
                return NotFound();

            return Ok(Product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductInput request)
        {
            await _servProduct.UpdateByIdAsync(id, request);

            return Ok();
        }

        [HttpPost()]
        public async Task<IActionResult> Post([FromBody] ProductInput request)
        {
            await _servProduct.CreateAsync(request);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductById(Guid id)
        {
            await _servProduct.DeleteAsync(id);

            return Ok();
        }
    }
}
