using System.Data;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Interfaces;
using OrderManagement.Dominio.Utils;

namespace OrderManagement.Service
{
    public class CustomerService
    {
        private readonly ICustomerRepository _repCustomer;

        public CustomerService(ICustomerRepository repCustomer)
        { 
            _repCustomer = repCustomer;
        }

        public async Task UpdateByIdAsync(Guid id, CustomerInput request)
        {
            if (!request.Validate())
                throw new ArgumentException("Invalid filled form");

            var CustomerOriginal = await _repCustomer.GetByIdAsync(id);
            if (CustomerOriginal == null)
                throw new HttpRequestException("Customer not found");

            if (request.Email != CustomerOriginal.Email && await _repCustomer.CheckEmailExistsAsync(request.Email))
                throw new DuplicateNameException("Email already in use");

            CustomerOriginal.Email = request.Email;
            CustomerOriginal.Cellphone = request.Cellphone;
            CustomerOriginal.Name = request.Name;

            await _repCustomer.UpdateAsync(CustomerOriginal);
        }

        public async Task<Guid> CreateAsync(CustomerInput request)
        {
            if (!request.Validate())
                throw new ArgumentException("Invalid filled form");

            if (await _repCustomer.CheckEmailExistsAsync(request.Email))
                throw new DuplicateNameException("Email already in use");

            var newCustomer = new Customer(request);

            await _repCustomer.CreateAsync(newCustomer);

            return newCustomer.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid ID");

            var Customer = await _repCustomer.GetByIdAsync(id);
            if (Customer == null)
                throw new HttpRequestException("Customer not found");

            await _repCustomer.DeleteAsync(id);
        }

        public async Task<Customer> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid ID");

            var response = await _repCustomer.GetByIdAsync(id);
            if (response == null)
                throw new HttpRequestException("Customer not found");

            return response;
        }

        public async Task<IEnumerable<Customer>> GetAsync(SearchfilterCustomer filter)
        {
            if (filter.CreationDate.HasValue && filter.CreationDate.Value > DateTime.Now)
                throw new ArgumentException("Creation date cannot be later than today's date");

            return await _repCustomer.GetAsync(filter);
        }
    }
}
