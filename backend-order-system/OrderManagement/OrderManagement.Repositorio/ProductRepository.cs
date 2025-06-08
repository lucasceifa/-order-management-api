using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OrderManagement.Domain;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Interfaces;
using OrderManagement.Dominio.Utils;

namespace OrderManagement.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;
        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

        public async Task UpdateAsync(Product Product)
        {
            var query = @"
                UPDATE Product SET
                    CreationDate = @CreationDate,
                    Name = @Name,
                    Email = @Email,
                    Cellphone = @Cellphone
                WHERE ID = @ID";
            var connection = CreateConnection();

            await connection.QueryAsync<Product>(query, Product);
        }

        public async Task CreateAsync(Product Product)
        {
            var query = @"
                INSERT INTO Product (ID, CreationDate, Name, Description, Price, QuantityAvailable)
                VALUES (@ID, @CreationDate, @Name, @Description, @Price, @QuantityAvailable)";

            var connection = CreateConnection();

            await connection.QueryAsync<Product>(query, Product);
        }

        public async Task DeleteAsync(Guid id)
        {
            var query = "DELETE FROM Product " +
                "WHERE ID = @Id";
            var connection = CreateConnection();

            await connection.QueryAsync<Product>(query, new { Id = id });
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            var query = "SELECT * FROM Product " +
                "WHERE ID = @Id";
            var connection = CreateConnection();

            return await connection.QueryFirstAsync<Product>(query, new { Id = id });
        }

        public async Task<IEnumerable<Product>> GetAsync(SearchfilterProduct filter)
        {
            var query = @"SELECT * FROM Product WHERE 1=1";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query += " AND LOWER(Name) LIKE @Name";
                parameters.Add("Name", $"%{filter.Name.ToLower()}%");
            }

            if (filter.PriceMin.HasValue)
            {
                query += " AND Price >= @PriceMin";
                parameters.Add("PriceMin", filter.PriceMin.Value);
            }

            if (filter.PriceMax.HasValue)
            {
                query += " AND Price <= @PriceMax";
                parameters.Add("PriceMax", filter.PriceMax.Value);
            }

            if (filter.QuantityAvailable.HasValue)
            {
                query += " AND QuantityAvailable = @QuantityAvailable";
                parameters.Add("QuantityAvailable", filter.QuantityAvailable.Value);
            }

            var connection = CreateConnection();
            return await connection.QueryAsync<Product>(query, parameters);
        }
    }
}
