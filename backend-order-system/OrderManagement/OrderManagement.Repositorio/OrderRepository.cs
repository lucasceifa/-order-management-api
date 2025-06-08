using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OrderManagement.Domain;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Enums;
using OrderManagement.Dominio.Interfaces;

namespace OrderManagement.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

        public async Task CreateAsync(Order order)
        {
            var query = @"
                INSERT INTO [Order] (Id, CreationDate, CostumerId, Status)
                VALUES (@Id, @CreationDate, @CostumerId, @Status)";

            var connection = CreateConnection();
            await connection.ExecuteAsync(query, new
            {
                Id = order.Id,
                CreationDate = order.CreationDate,
                CostumerId = order.CostumerId,
                Status = (int)order.Status
            });
        }

        public async Task<Order> GetByIdAsync(Guid id)
        {
            var query = "SELECT * FROM [Order] WHERE Id = @Id";
            var connection = CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<Order>(query, new { Id = id });
        }

        public async Task<IEnumerable<Order>> GetAsync()
        {
            var query = "SELECT * FROM [Order]";
            var connection = CreateConnection();

            return await connection.QueryAsync<Order>(query);
        }

        public async Task UpdateStatusAsync(Guid id, IOrderStatus status)
        {
            var query = "UPDATE [Order] SET Status = @Status WHERE Id = @Id";
            var connection = CreateConnection();

            await connection.ExecuteAsync(query, new
            {
                Id = id,
                Status = (int)status
            });
        }

        public async Task DeleteAsync(Guid id)
        {
            var query = "DELETE FROM [Order] WHERE Id = @Id";
            var connection = CreateConnection();

            await connection.ExecuteAsync(query, new { Id = id });
        }
    }
}
