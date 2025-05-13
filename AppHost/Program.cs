using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var password = builder.AddParameter("password", secret: true);
var exampleParam = builder.AddParameter("example-parameter-name");
var key = $"Parameters:example-parameter-name";
var value = builder.Configuration[key]; // value = "local-value"

var randomConnectionString = builder.AddConnectionString("random-connection-string");

var server = builder.AddSqlServer("server", password, 1433)
    .WithLifetime(ContainerLifetime.Persistent);
var db = server.AddDatabase("products");

var migration = builder.AddProject<MigrationService>("migration")
    .WithReference(db)
    .WaitFor(db)
    .WithParentRelationship(server);

var cache = builder.AddRedis("cache")
    .WithRedisCommander()
    .WithLifetime(ContainerLifetime.Persistent);

var warehouse = builder.AddProject<WarehouseApi>("warehouse")
    .WithReference(cache)
    .WaitFor(cache);

builder.AddProject<Api>("api") // unique name
    // .WithHttpsEndpoint(port: 7890)
    .WithReference(db)
    .WaitFor(db)
    .WaitFor(migration)
    .WithReference(warehouse)
    .WithReference(randomConnectionString)
    .WithEnvironment("EXAMPLE_PARAM" ,exampleParam);

// .WithReplicas(1) // not load balanced, replicas in aspire require lots more work to be useful https://github.com/dotnet/aspire/issues/6640
// .WithExplicitStart();
// var rabbitmq = builder.AddRabbitMQ("rabbitmq")
//     .WithManagementPlugin();

builder.Eventing.Subscribe<BeforeStartEvent>(
    static (@event, cancellationToken) =>
    {
        var logger = @event.Services.GetRequiredService<ILogger<Program>>();

        logger.LogInformation("1. BeforeStartEvent");

        return Task.CompletedTask;
    });

builder.Eventing.Subscribe<AfterEndpointsAllocatedEvent>(
    static (@event, cancellationToken) =>
    {
        var logger = @event.Services.GetRequiredService<ILogger<Program>>();

        logger.LogInformation("2. AfterEndpointsAllocatedEvent");

        return Task.CompletedTask;
    });

builder.Eventing.Subscribe<AfterResourcesCreatedEvent>(
    static (@event, cancellationToken) =>
    {
        var logger = @event.Services.GetRequiredService<ILogger<Program>>();

        logger.LogInformation("3. AfterResourcesCreatedEvent");

        return Task.CompletedTask;
    });

builder.Eventing.Subscribe<ResourceReadyEvent>(
    cache.Resource,
    static (@event, cancellationToken) =>
    {
        var logger = @event.Services.GetRequiredService<ILogger<Program>>();

        logger.LogInformation("100. ResourceReadyEvent");

        return Task.CompletedTask;
    });

builder.Eventing.Subscribe<BeforeResourceStartedEvent>(
    cache.Resource,
    static (@event, cancellationToken) =>
    {
        var logger = @event.Services.GetRequiredService<ILogger<Program>>();

        logger.LogInformation("99. BeforeResourceStartedEvent");

        return Task.CompletedTask;
    });

builder.Eventing.Subscribe<ConnectionStringAvailableEvent>(
    cache.Resource,
    static (@event, cancellationToken) =>
    {
        var logger = @event.Services.GetRequiredService<ILogger<Program>>();

        logger.LogInformation("98. ConnectionStringAvailableEvent");

        return Task.CompletedTask;
    });

builder.AddDockerComposePublisher();

builder.Build().Run();