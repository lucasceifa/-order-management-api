using OrderManagement.Domain;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Interfaces;
using OrderManagement.Dominio.Requests;
using OrderManagement.Dominio.Utils;

namespace OrderManagement.Service
{
    public class ProductService
    {
        private readonly IProductRepository _repProduct;

        public ProductService(IProductRepository repProduct)
        {
            _repProduct = repProduct;
        }

        public async Task UpdateByIdAsync(Guid id, ProductInput request)
        {
            if (!request.Validate())
                throw new ArgumentException("Invalid filled form");

            var ProductOriginal = await _repProduct.GetByIdAsync(id);
            if (ProductOriginal == null)
                throw new HttpRequestException("Product not found");

            ProductOriginal.Name = request.Name;
            ProductOriginal.Description = request.Description;
            ProductOriginal.Price = request.Price;
            ProductOriginal.QuantityAvailable = request.QuantityAvailable;

            await _repProduct.UpdateAsync(ProductOriginal);
        }

        public async Task<Guid> CreateAsync(ProductInput request)
        {
            if (!request.Validate())
                throw new ArgumentException("Invalid filled form");

            var novoProduct = new Product(request);

            await _repProduct.CreateAsync(novoProduct);

            return novoProduct.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid ID");

            var Product = await _repProduct.GetByIdAsync(id);
            if (Product == null)
                throw new HttpRequestException("Product not found");

            await _repProduct.DeleteAsync(id);
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid ID");

            var Product = await _repProduct.GetByIdAsync(id);
            if (Product == null)
                throw new HttpRequestException("Product not found");

            return Product;
        }

        public async Task<IEnumerable<Product>> GetAsync(SearchfilterProduct filter)
        {
            if (filter.PriceMin > filter.PriceMax)
                throw new ArgumentException("The minimum price cannot be higher than the maximum price of the product");

            if (filter.PriceMax < 0 || filter.PriceMin < 0 || filter.QuantityAvailable < 0)
                throw new ArgumentException("Numeric data cannot include negative values");

            return await _repProduct.GetAsync(filter);
        }
    }
}
