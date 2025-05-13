using OpenTelemetry.Trace;
using WarehouseApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.AddRedisClient(connectionName: "cache");
// builder.Services.AddSingleton(TracerProvider.Default.GetTracer(builder.Environment.ApplicationName));
builder.Services.AddSingleton<WarehouseStore>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();