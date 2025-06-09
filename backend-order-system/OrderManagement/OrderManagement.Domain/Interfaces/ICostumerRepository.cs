using OrderManagement.Dominio.Utils;

namespace OrderManagement.Dominio.Interfaces
{
    public interface ICustomerRepository
    {
        public Task CreateAsync(Customer Customer);
        public Task<Customer?> GetByIdAsync(Guid id);
        public Task<IEnumerable<Customer>> GetAsync(SearchfilterCustomer filter);
        public Task<bool> CheckEmailExistsAsync(string email);
        public Task UpdateAsync(Customer Customer);
        public Task DeleteAsync(Guid id);
    }
}
