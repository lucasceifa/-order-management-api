using OrderManagement.Domain;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Interfaces;
using OrderManagement.Dominio.Utils;

namespace OrderManagement.API.Teste.Services.RepositorysMock
{
    public class ProductRepositoryMock : IProductRepository
    {
        private List<Product> _dados;

        public ProductRepositoryMock()
        {
            if (_dados == null)
                _dados = new List<Product>();
        }

        public async Task UpdateAsync(Product Product)
        {
            var index = _dados.FindIndex(p => p.Id == Product.Id);
            if (index >= 0)
                _dados[index] = Product;
        }

        public async Task CreateAsync(Product Product)
        {
            _dados.Add(Product);
        }

        public async Task DeleteAsync(Guid id)
        {
            _dados = _dados.Where(p => p.Id != id).ToList();
        }

        public async Task<IEnumerable<Product>> GetAsync(SearchfilterProduct filter)
        {
            return _dados.Where(p =>
                (string.IsNullOrEmpty(filter.Name) || p.Name.ToLower().Contains(filter.Name.ToLower())) &&
                (!filter.PriceMin.HasValue || p.Price >= filter.PriceMin.Value) &&
                (!filter.PriceMax.HasValue || p.Price <= filter.PriceMax.Value) &&
                (!filter.QuantityAvailable.HasValue || p.QuantityAvailable == filter.QuantityAvailable.Value));
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return _dados.FirstOrDefault(p => p.Id == id);
        }
    }
}
