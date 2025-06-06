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

var app = builder.Build();

#region Iniciando tabelas da Database em SQL
using (var scope = app.Services.CreateScope())
{
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    var scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "Scripts", "tables.sql");

    if (File.Exists(scriptPath))
    {
        var script = File.ReadAllText(scriptPath);

        using var connection = new SqlConnection(connectionString);
        await connection.ExecuteAsync(script);
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
