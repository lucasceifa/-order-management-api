using OrderManagement.Domain;
using OrderManagement.Dominio.Enums;
using OrderManagement.Dominio.Utils;

namespace OrderManagement.Dominio.Interfaces
{
    public interface IOrderRepository
    {
        public Task CreateAsync(Order Order);
        public Task<Order> GetByIdAsync(Guid id);
        public Task<IEnumerable<Order>> GetAsync();
        public Task UpdateStatusAsync(Guid id, IOrderStatus status);
        public Task DeleteAsync(Guid id);
    }
}
