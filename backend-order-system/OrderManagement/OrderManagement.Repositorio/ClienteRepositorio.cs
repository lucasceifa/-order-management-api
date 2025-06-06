using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Interfaces;

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

            await connection.QueryFirstAsync<Cliente>(query, new { Id = id });
        }

        public async Task<Cliente> ObterPorIdAsync(Guid id)
        {
            var query = "SELECT * FROM Cliente " +
                "WHERE ID = @Id";
            var connection = CreateConnection();

            return await connection.QueryFirstAsync<Cliente>(query, new { Id = id });
        }

        public async Task<IEnumerable<Cliente>> ObterAsync()
        {
            var query = "SELECT * FROM Cliente";
            var connection = CreateConnection();

            return await connection.QueryAsync<Cliente>(query);
        }
    }
}
