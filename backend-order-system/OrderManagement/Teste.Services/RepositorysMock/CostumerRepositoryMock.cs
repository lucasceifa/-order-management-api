using OrderManagement.Dominio;
using OrderManagement.Dominio.Interfaces;
using OrderManagement.Dominio.Utils;

namespace OrderManagement.API.Teste.Services.RepositorysMock
{
    public class CustomerRepositoryMock : ICustomerRepository
    {
        private List<Customer> _dados;

        public CustomerRepositoryMock()
        {
            if (_dados == null)
                _dados = new List<Customer>();
        }

        public async Task UpdateAsync(Customer Customer)
        {
            var index = _dados.FindIndex(c => c.Id == Customer.Id);
            _dados[index] = Customer;
        }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            return _dados.Any(c => c.Email == email);
        }

        public async Task CreateAsync(Customer Customer)
        {
            _dados.Add(Customer);
        }

        public async Task DeleteAsync(Guid id)
        {
            _dados = _dados.Where(c => c.Id != id).ToList();
        }

        public async Task<IEnumerable<Customer>> GetAsync(SearchfilterCustomer filter)
        {
            return _dados.Where(e => (!filter.CreationDate.HasValue || e.CreationDate > filter.CreationDate) && (String.IsNullOrEmpty(filter.Email) || e.Email.ToLower().Contains(filter.Email)) || (String.IsNullOrEmpty(filter.Name) || e.Name.ToLower().Contains(filter.Name)));
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return _dados.FirstOrDefault(c => c.Id == id);
        }
    }
}
