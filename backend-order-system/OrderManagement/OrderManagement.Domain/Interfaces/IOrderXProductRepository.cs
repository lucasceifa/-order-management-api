﻿using OrderManagement.Domain;
using OrderManagement.Dominio;

namespace OrderXProductManagement.Dominio.Interfaces
{
    public interface IOrderXProductRepository
    {
        public Task CreateAsync(OrderXProduct OrderXProduct);
        public Task<OrderXProduct?> GetByIdAsync(Guid id);
        public Task<IEnumerable<OrderXProduct>> GetAsync();
        public Task<IEnumerable<OrderXProduct>> GetByOrderIdAsync(Guid id);
        public Task UpdateAsync(OrderXProduct OrderXProduct);
        public Task DeleteAsync(Guid id);
    }
}
