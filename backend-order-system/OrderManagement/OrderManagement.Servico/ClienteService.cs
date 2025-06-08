using System.Data;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Interfaces;
using OrderManagement.Dominio.Utils;

namespace OrderManagement.Service
{
    public class CostumerService
    {
        private readonly ICostumerRepository _repCostumer;

        public CostumerService(ICostumerRepository repCostumer)
        { 
            _repCostumer = repCostumer;
        }

        public async Task UpdateByIdAsync(Guid id, CostumerInput request)
        {
            if (!request.Validate())
                throw new ArgumentException("Invalid filled form");

            var CostumerOriginal = await _repCostumer.GetByIdAsync(id);
            if (CostumerOriginal == null)
                throw new HttpRequestException("Costumer not found");

            if (request.Email != CostumerOriginal.Email && await _repCostumer.CheckEmailExistsAsync(request.Email))
                throw new DuplicateNameException("Email already in use");

            CostumerOriginal.Email = request.Email;
            CostumerOriginal.Cellphone = request.Cellphone;
            CostumerOriginal.Name = request.Name;

            await _repCostumer.UpdateAsync(CostumerOriginal);
        }

        public async Task<Guid> CreateAsync(CostumerInput request)
        {
            if (!request.Validate())
                throw new ArgumentException("Invalid filled form");

            if (await _repCostumer.CheckEmailExistsAsync(request.Email))
                throw new DuplicateNameException("Email already in use");

            var newCostumer = new Costumer(request);

            await _repCostumer.CreateAsync(newCostumer);

            return newCostumer.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid ID");

            var Costumer = await _repCostumer.GetByIdAsync(id);
            if (Costumer == null)
                throw new HttpRequestException("Costumer not found");

            await _repCostumer.DeleteAsync(id);
        }

        public async Task<Costumer> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid ID");

            var response = await _repCostumer.GetByIdAsync(id);
            if (response == null)
                throw new HttpRequestException("Costumer not found");

            return response;
        }

        public async Task<IEnumerable<Costumer>> GetAsync(SearchfilterCostumer filter)
        {
            if (filter.CreationDate.HasValue && filter.CreationDate.Value > DateTime.Now)
                throw new ArgumentException("Creation date cannot be later than today's date");

            return await _repCostumer.GetAsync(filter);
        }
    }
}
