using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Interfaces;
using OrderManagement.Dominio.Utils;

namespace OrderManagement.Repository
{
    public class CostumerRepository : ICostumerRepository
    {
        private readonly string _connectionString;
        public CostumerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            var query = "SELECT * FROM Costumer " +
                "WHERE Email = @Email";
            var connection = CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<Costumer>(query, new { Email = email }) != null;
        }

        public async Task UpdateAsync(Costumer Costumer)
        {
            var query = @"
                UPDATE Costumer SET
                    CreationDate = @CreationDate,
                    Name = @Name,
                    Email = @Email,
                    Cellphone = @Cellphone
                WHERE ID = @ID";
            var connection = CreateConnection();

            await connection.QueryAsync<Costumer>(query, Costumer);
        }

        public async Task CreateAsync(Costumer Costumer)
        {
            var query = "INSERT INTO Costumer (ID, CreationDate, Name, Email, Cellphone)" +
                "VALUES (@ID, @CreationDate, @Name, @Email, @Cellphone)";
            var connection = CreateConnection();

            await connection.QueryAsync<Costumer>(query, Costumer);
        }

        public async Task DeleteAsync(Guid id)
        {
            var query = "DELETE FROM Costumer " +
                "WHERE ID = @Id";
            var connection = CreateConnection();

            await connection.QueryAsync<Costumer>(query, new { Id = id });
        }

        public async Task<Costumer?> GetByIdAsync(Guid id)
        {
            var query = "SELECT * FROM Costumer " +
                "WHERE ID = @Id";
            var connection = CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<Costumer>(query, new { Id = id });
        }

        public async Task<IEnumerable<Costumer>> GetAsync(SearchfilterCostumer filter)
        {
            var query = @"SELECT * FROM Costumer WHERE 1=1";
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

            return await connection.QueryAsync<Costumer>(query, parameters);
        }
    }
}
