using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OrderManagement.Domain;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Interfaces;

namespace OrderManagement.Repositorio
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {
        private readonly string _connectionString;
        public ProdutoRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

        public async Task AtualizarAsync(Produto Produto)
        {
            var query = @"
                UPDATE Produto SET
                    DataDeCadastro = @DataDeCadastro,
                    Nome = @Nome,
                    Email = @Email,
                    Telefone = @Telefone
                WHERE ID = @ID";
            var connection = CreateConnection();

            await connection.QueryAsync<Produto>(query, Produto);
        }

        public async Task CriarAsync(Produto Produto)
        {
            var query = @"
                INSERT INTO Produto (ID, DataDeCadastro, Nome, Descricao, Preco, QuantidadeDisponivel)
                VALUES (@ID, @DataDeCadastro, @Nome, @Descricao, @Preco, @QuantidadeDisponivel)";

            var connection = CreateConnection();

            await connection.QueryAsync<Produto>(query, Produto);
        }

        public async Task DeletarAsync(Guid id)
        {
            var query = "DELETE FROM Produto " +
                "WHERE ID = @Id";
            var connection = CreateConnection();

            await connection.QueryAsync<Produto>(query, new { Id = id });
        }

        public async Task<Produto> ObterPorIdAsync(Guid id)
        {
            var query = "SELECT * FROM Produto " +
                "WHERE ID = @Id";
            var connection = CreateConnection();

            return await connection.QueryFirstAsync<Produto>(query, new { Id = id });
        }

        public async Task<IEnumerable<Produto>> ObterAsync()
        {
            var query = "SELECT * FROM Produto";
            var connection = CreateConnection();

            return await connection.QueryAsync<Produto>(query);
        }
    }
}
