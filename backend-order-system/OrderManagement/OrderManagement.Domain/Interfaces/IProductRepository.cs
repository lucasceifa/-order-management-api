using OrderManagement.Domain;
using OrderManagement.Dominio.Utils;

namespace OrderManagement.Dominio.Interfaces
{
    public interface IProductRepository
    {
        public Task CreateAsync(Product Product);
        public Task<Product> GetByIdAsync(Guid id);
        public Task<IEnumerable<Product>> GetAsync(SearchfilterProduct filter);
        public Task UpdateAsync(Product Product);
        public Task DeleteAsync(Guid id);
    }
}
