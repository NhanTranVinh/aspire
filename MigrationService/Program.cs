using MigrationService;
using Persistence;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.AddServiceDefaults();

builder.AddSqlServerDbContext<ProductDbContext>("products");
var host = builder.Build();
host.Run();