using Api;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// var connectionString =
//     builder.Configuration.GetConnectionString("DefaultConnection")
//     ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//
// builder.Services.AddDbContext<ProductDbContext>(options =>
//     options.UseSqlServer(connectionString));

builder.AddSqlServerDbContext<ProductDbContext>(connectionName: "products");
builder.Services.AddHttpClient<WeatherHttpClient>(x => x.BaseAddress = new Uri("https+http://weather"));
builder.Services.AddHttpClient<WarehouseHttpClient>(x => x.BaseAddress = new Uri("https+http://warehouse"));

builder.Services.AddSingleton(TracerProvider.Default.GetTracer(builder.Environment.ApplicationName));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapDefaultEndpoints();
app.Run();