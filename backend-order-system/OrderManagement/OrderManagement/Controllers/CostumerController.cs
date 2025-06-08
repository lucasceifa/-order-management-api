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
    public class CostumerController : ControllerBase
    {
        private readonly CostumerService _servCostumer;

        public CostumerController(CostumerService servCostumer)
        {
            _servCostumer = servCostumer;
        }

        [HttpGet]
        public async Task<IActionResult> GetCostumers([FromQuery] SearchfilterCostumer filter)
        {
            var Costumers = await _servCostumer.GetAsync(filter);

            return Ok(Costumers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCostumerById(Guid id)
        {
            var Costumer = await _servCostumer.GetByIdAsync(id);
            if (Costumer == null)
                return NotFound();

            return Ok(Costumer);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CostumerInput request)
        {
            await _servCostumer.CreateAsync(request);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] CostumerInput request)
        {
            await _servCostumer.UpdateByIdAsync(id, request);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCostumerById(Guid id)
        {
            await _servCostumer.DeleteAsync(id);

            return Ok();
        }
    }
}
