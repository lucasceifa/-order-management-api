using System;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using OrderManagement.Domain;
using OrderManagement.Dominio;
using OrderXProductManagement.Dominio.Interfaces;

namespace OrderManagement.Repository
{
    public class OrderXProductRepository : IOrderXProductRepository
    {
        private readonly string _connectionString;

        public OrderXProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

        public async Task CreateAsync(OrderXProduct orderXProduct)
        {
            var query = @"
                INSERT INTO OrderXProduct (Id, CreationDate, ProductId, OrderId, QuantityPurchased, ProductValue)
                VALUES (@Id, @CreationDate, @ProductId, @OrderId, @QuantityPurchased, @ProductValue)";
            
            var connection = CreateConnection();
            await connection.ExecuteAsync(query, orderXProduct);
        }

        public async Task<OrderXProduct?> GetByIdAsync(Guid id)
        {
            var query = "SELECT * FROM OrderXProduct WHERE Id = @Id";
            var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<OrderXProduct>(query, new { Id = id });
        }

        public async Task<IEnumerable<OrderXProduct>> GetAsync()
        {
            var query = "SELECT * FROM OrderXProduct";
            var connection = CreateConnection();
            return await connection.QueryAsync<OrderXProduct>(query);
        }

        public async Task<IEnumerable<OrderXProduct>> GetByOrderIdAsync(Guid orderId)
        {
            var query = "SELECT * FROM OrderXProduct WHERE OrderId = @OrderId";
            var connection = CreateConnection();
            return await connection.QueryAsync<OrderXProduct>(query, new { OrderId = orderId });
        }

        public async Task UpdateAsync(OrderXProduct orderXProduct)
        {
            var query = @"
                UPDATE OrderXProduct SET
                    ProductId = @ProductId,
                    OrderId = @OrderId,
                    QuantityPurchased = @QuantityPurchased,
                    ProductValue = @ProductValue
                WHERE Id = @Id";

            var connection = CreateConnection();
            await connection.ExecuteAsync(query, orderXProduct);
        }

        public async Task DeleteAsync(Guid id)
        {
            var query = "DELETE FROM OrderXProduct WHERE Id = @Id";
            var connection = CreateConnection();
            await connection.ExecuteAsync(query, new { Id = id });
        }
    }
}
