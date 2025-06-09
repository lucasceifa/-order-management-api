using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Interfaces;
using OrderManagement.Dominio.Utils;

namespace OrderManagement.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;
        public CustomerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            var query = "SELECT * FROM Customer " +
                "WHERE Email = @Email";
            var connection = CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<Customer>(query, new { Email = email }) != null;
        }

        public async Task UpdateAsync(Customer Customer)
        {
            var query = @"
                UPDATE Customer SET
                    CreationDate = @CreationDate,
                    Name = @Name,
                    Email = @Email,
                    Cellphone = @Cellphone
                WHERE ID = @ID";
            var connection = CreateConnection();

            await connection.QueryAsync<Customer>(query, Customer);
        }

        public async Task CreateAsync(Customer Customer)
        {
            var query = "INSERT INTO Customer (ID, CreationDate, Name, Email, Cellphone)" +
                "VALUES (@ID, @CreationDate, @Name, @Email, @Cellphone)";
            var connection = CreateConnection();

            await connection.QueryAsync<Customer>(query, Customer);
        }

        public async Task DeleteAsync(Guid id)
        {
            var query = "DELETE FROM Customer " +
                "WHERE ID = @Id";
            var connection = CreateConnection();

            await connection.QueryAsync<Customer>(query, new { Id = id });
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            var query = "SELECT * FROM Customer " +
                "WHERE ID = @Id";
            var connection = CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<Customer>(query, new { Id = id });
        }

        public async Task<IEnumerable<Customer>> GetAsync(SearchfilterCustomer filter)
        {
            var query = @"SELECT * FROM Customer WHERE 1=1";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query += " AND LOWER(Name) LIKE @Name";
                parameters.Add("Name", $"%{filter.Name.ToLower()}%");
            }

            if (!string.IsNullOrWhiteSpace(filter.Email))
            {
                query += " AND LOWER(Email) LIKE @Email";
                parameters.Add("Email", $"%{filter.Email.ToLower()}%");
            }

            if (filter.CreationDate.HasValue)
            {
                query += " AND CAST(CreationDate AS DATE) = @CreationDate";
                parameters.Add("CreationDate", filter.CreationDate.Value.Date);
            }

            var connection = CreateConnection();

            return await connection.QueryAsync<Customer>(query, parameters);
        }
    }
}
