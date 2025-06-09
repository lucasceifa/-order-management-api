using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrderManagement.Domain.Requests;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Requests;
using OrderManagement.Service;

namespace OrderManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderXProductController : ControllerBase
    {
        private readonly OrderXProductService _servOrderXProduct;

        public OrderXProductController(OrderXProductService servOrderXProduct)
        {
            _servOrderXProduct = servOrderXProduct;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _servOrderXProduct.GetAsync();

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _servOrderXProduct.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpGet("{id}/Products")]
        public async Task<IActionResult> GetProductsByOrderId(Guid id)
        {
            var products = await _servOrderXProduct.GetProductsByOrderIdAsync(id);
            if (products == null)
                return NotFound();

            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderXProductInput request)
        {
            var id = await _servOrderXProduct.CreateAsync(request);
            return Ok(id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] OrderXProductInput request)
        {
            await _servOrderXProduct.UpdateAsync(id, request);
            return Ok();
        }

        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> CancelOrder(Guid id)
        {
            await _servOrderXProduct.CancelAsync(id);
            return Ok();
        }

        [HttpPut("reopen/{id}")]
        public async Task<IActionResult> ReopenOrder(Guid id)
        {
            await _servOrderXProduct.ReopenAsync(id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            await _servOrderXProduct.DeleteAsync(id);
            return Ok();
        }
    }
}
