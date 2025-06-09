using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Interfaces;
using OrderManagement.Repository;
using OrderManagement.Service;
using OrderXProductManagement.Dominio.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MainDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MainDB")); 
});

#region Declaring dependencies from Customer class
builder.Services.AddTransient<CustomerService, CustomerService>();
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
#endregion

#region Declaring dependencies from Product class
builder.Services.AddTransient<ProductService, ProductService>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
#endregion

#region Declaring dependencies from Order and OrderXProduct class
builder.Services.AddTransient<OrderXProductService, OrderXProductService>();
builder.Services.AddTransient<IOrderXProductRepository, OrderXProductRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
#endregion

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

#region Starting database tables and inserting some data into it
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

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
