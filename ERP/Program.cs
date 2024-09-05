using Microsoft.EntityFrameworkCore;
using ERP.Data;
using Microsoft.Data.Sqlite;
using System.Text.Json.Serialization;
using ERP.Repositories;

var builder = WebApplication.CreateBuilder(args);

// DB       ----------------------------------------------------------------------------
var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "Erp.db" };
var connectionString = connectionStringBuilder.ToString();
builder.Services.AddDbContext<ErpContext>(opt => opt.UseSqlite(connectionString));
// DB       ----------------------------------------------------------------------------

// Services ----------------------------------------------------------------------------

builder.Services.
    AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });



// REPOSITORIES
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IShipmentRepository, ShipmentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services ----------------------------------------------------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
