using OrderManagement.Dominio;
using OrderXProductManagement.Dominio.Interfaces;

namespace OrderManagement.API.Teste.Services.RepositorysMock
{
    public class OrderXProductRepositoryMock : IOrderXProductRepository
    {
        private List<OrderXProduct> _orderXProducts;

        public OrderXProductRepositoryMock()
        {
            _orderXProducts = new List<OrderXProduct>();
        }

        public async Task CreateAsync(OrderXProduct orderXProduct)
        {
            _orderXProducts.Add(orderXProduct);
        }

        public async Task DeleteAsync(Guid id)
        {
            _orderXProducts = _orderXProducts.Where(x => x.Id != id).ToList();
        }

        public async Task<IEnumerable<OrderXProduct>> GetAsync()
        {
            return _orderXProducts;
        }

        public async Task<OrderXProduct?> GetByIdAsync(Guid id)
        {
            return _orderXProducts.FirstOrDefault(x => x.Id == id);
        }

        public async Task UpdateAsync(OrderXProduct orderXProduct)
        {
            var index = _orderXProducts.FindIndex(x => x.Id == orderXProduct.Id);
            if (index >= 0)
            {
                _orderXProducts[index] = orderXProduct;
            }
        }
    }
}
