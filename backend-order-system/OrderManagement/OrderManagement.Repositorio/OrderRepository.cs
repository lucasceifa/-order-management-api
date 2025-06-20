﻿using System.Data;
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
                INSERT INTO [Order] (Id, CreationDate, CustomerId, Status)
                VALUES (@Id, @CreationDate, @CustomerId, @Status)";

            var connection = CreateConnection();
            await connection.ExecuteAsync(query, new
            {
                Id = order.Id,
                CreationDate = order.CreationDate,
                CustomerId = order.CustomerId,
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

        public async Task UpdateStatusAsync(Guid id, IOrderStatus status, DateTime? cancellationDate)
        {
            var query = @"
                UPDATE [Order]
                SET 
                    Status = @Status,
                    CancellationDate = @CancellationDate
                WHERE Id = @Id";
            var connection = CreateConnection();

            await connection.ExecuteAsync(query, new
            {
                Id = id,
                Status = (int)status,
                CancellationDate = cancellationDate
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
