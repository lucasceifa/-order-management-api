using OrderManagement.Domain;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Enums;
using OrderManagement.Dominio.Interfaces;

namespace OrderManagement.API.Teste.Services.RepositorysMock
{
    public class OrderRepositoryMock : IOrderRepository
    {
        private List<Order> _orders;

        public OrderRepositoryMock()
        {
            _orders = new List<Order>();
        }

        public async Task CreateAsync(Order order)
        {
            _orders.Add(order);
        }

        public async Task DeleteAsync(Guid id)
        {
            _orders = _orders.Where(o => o.Id != id).ToList();
        }

        public async Task<IEnumerable<Order>> GetAsync()
        {
            return _orders;
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return _orders.FirstOrDefault(o => o.Id == id);
        }

        public async Task UpdateStatusAsync(Guid id, IOrderStatus status)
        {
            var index = _orders.FindIndex(o => o.Id == id);
            if (index >= 0)
            {
                _orders[index].Status = status;
            }
        }
    }
}
