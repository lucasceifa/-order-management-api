using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Utils;
using OrderManagement.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _servCustomer;

        public CustomerController(CustomerService servCustomer)
        {
            _servCustomer = servCustomer;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers([FromQuery] SearchfilterCustomer filter)
        {
            var Customers = await _servCustomer.GetAsync(filter);

            return Ok(Customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            var Customer = await _servCustomer.GetByIdAsync(id);
            if (Customer == null)
                return NotFound();

            return Ok(Customer);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomerInput request)
        {
            await _servCustomer.CreateAsync(request);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] CustomerInput request)
        {
            await _servCustomer.UpdateByIdAsync(id, request);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerById(Guid id)
        {
            await _servCustomer.DeleteAsync(id);

            return Ok();
        }
    }
}
