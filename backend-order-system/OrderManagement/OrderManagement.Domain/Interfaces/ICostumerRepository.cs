using OrderManagement.Dominio.Utils;

namespace OrderManagement.Dominio.Interfaces
{
    public interface ICostumerRepository
    {
        public Task CreateAsync(Costumer Costumer);
        public Task<Costumer?> GetByIdAsync(Guid id);
        public Task<IEnumerable<Costumer>> GetAsync(SearchfilterCostumer filter);
        public Task<bool> CheckEmailExistsAsync(string email);
        public Task UpdateAsync(Costumer Costumer);
        public Task DeleteAsync(Guid id);
    }
}
