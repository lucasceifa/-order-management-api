using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Interfaces;
using OrderManagement.Dominio.Utils;

namespace OrderManagement.Repositorio
{
    public class ClienteRepositorio : IClienteRepositorio
    {
        private readonly string _connectionString;
        public ClienteRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            var query = "SELECT * FROM Cliente " +
                "WHERE Email = @Email";
            var connection = CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<Cliente>(query, new { Email = email }) != null;
        }

        public async Task AtualizarAsync(Cliente cliente)
        {
            var query = @"
                UPDATE Cliente SET
                    DataDeCadastro = @DataDeCadastro,
                    Nome = @Nome,
                    Email = @Email,
                    Telefone = @Telefone
                WHERE ID = @ID";
            var connection = CreateConnection();

            await connection.QueryAsync<Cliente>(query, cliente);
        }

        public async Task CriarAsync(Cliente cliente)
        {
            var query = "INSERT INTO Cliente (ID, DataDeCadastro, Nome, Email, Telefone)" +
                "VALUES (@ID, @DataDeCadastro, @Nome, @Email, @Telefone)";
            var connection = CreateConnection();

            await connection.QueryAsync<Cliente>(query, cliente);
        }

        public async Task DeletarAsync(Guid id)
        {
            var query = "DELETE FROM Cliente " +
                "WHERE ID = @Id";
            var connection = CreateConnection();

            await connection.QueryAsync<Cliente>(query, new { Id = id });
        }

        public async Task<Cliente?> ObterPorIdAsync(Guid id)
        {
            var query = "SELECT * FROM Cliente " +
                "WHERE ID = @Id";
            var connection = CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<Cliente>(query, new { Id = id });
        }

        public async Task<IEnumerable<Cliente>> ObterAsync(ParametrosBuscaCliente filtro)
        {
            var query = @"SELECT * FROM Cliente WHERE 1=1";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(filtro.Nome))
            {
                query += " AND LOWER(Nome) LIKE @Nome";
                parameters.Add("Nome", $"%{filtro.Nome.ToLower()}%");
            }

            if (!string.IsNullOrWhiteSpace(filtro.Email))
            {
                query += " AND LOWER(Email) LIKE @Email";
                parameters.Add("Email", $"%{filtro.Email.ToLower()}%");
            }

            if (filtro.DataDeCadastro.HasValue)
            {
                query += " AND CAST(DataDeCadastro AS DATE) = @DataDeCadastro";
                parameters.Add("DataDeCadastro", filtro.DataDeCadastro.Value.Date);
            }

            var connection = CreateConnection();

            return await connection.QueryAsync<Cliente>(query, parameters);
        }
    }
}
