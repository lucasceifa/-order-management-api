using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Interfaces;
using OrderManagement.Repositorio;
using OrderManagement.Servico;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MainDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MainDB")); 
});

#region Declarando dependências da classe Cliente
builder.Services.AddTransient<ClienteService, ClienteService>();
builder.Services.AddTransient<IClienteRepositorio, ClienteRepositorio>();
#endregion

#region Declarando dependências da classe Produto
builder.Services.AddTransient<ProdutoService, ProdutoService>();
builder.Services.AddTransient<IProdutoRepositorio, ProdutoRepositorio>();
#endregion

var app = builder.Build();

#region Iniciando tabelas da Database em SQL e injetando dados caso estejam vazios
using (var scope = app.Services.CreateScope())
{
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    var scriptFolder = Path.Combine(Directory.GetCurrentDirectory(), "Scripts");

    var tablesScriptPath = Path.Combine(scriptFolder, "tables.sql");
    var dataScriptPath = Path.Combine(scriptFolder, "tables_data.sql");

    using var connection = new SqlConnection(connectionString);

    if (File.Exists(tablesScriptPath))
    {
        var tablesScript = File.ReadAllText(tablesScriptPath);
        await connection.ExecuteAsync(tablesScript);
    }

    if (File.Exists(dataScriptPath))
    {
        var dataScript = File.ReadAllText(dataScriptPath);
        await connection.ExecuteAsync(dataScript);
    }
}
#endregion

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
