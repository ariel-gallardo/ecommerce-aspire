using Aspire.Hosting;
using Ecommerce.AppHost;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);
var cfg = builder.Configuration;

var redisDataMount = cfg["Parameters:AppSettings:Redis:DataMount"];
var rabbitDataMount = cfg["Parameters:AppSettings:RabbitMQ:DataMount"];

var redisPass = builder.AddParameterFromConfiguration("RedisPassword", "Parameters:AppSettings:Redis:Password");
var rabbitUser = builder.AddParameterFromConfiguration("RabbitUser", "Parameters:AppSettings:RabbitMQ:Username");
var rabbitPass = builder.AddParameterFromConfiguration("RabbitPassword", "Parameters:AppSettings:RabbitMQ:Password");

var cache = builder.AddRedis("cache", 1000, redisPass);
var rabbitMQ = builder.AddRabbitMQ("rabbit", rabbitUser, rabbitPass, 1001);

if (!string.IsNullOrWhiteSpace(redisDataMount))
{
    builder.AddParameterFromConfiguration("RedisDataMount", "Parameters:AppSettings:Redis:DataMount");
    cache = cache.WithDataBindMount(redisDataMount);
}
if (!string.IsNullOrWhiteSpace(rabbitDataMount))
{
    builder.AddParameterFromConfiguration("RabbitDataMount", "Parameters:AppSettings:RabbitMQ:DataMount");
    rabbitMQ = rabbitMQ.WithDataBindMount(rabbitDataMount);
}

var cartApi = builder.AddProject<Projects.Cart_API>("cart")
    .WithAppSettingsEnvironments(cfg)
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(rabbitMQ)
    .WithReference(cache)
    .WaitFor(rabbitMQ)
    .WaitFor(cache);
var inventoryApi = builder.AddProject<Projects.Inventory_API>("inventory")
    .WithAppSettingsEnvironments(cfg)
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(rabbitMQ)
    .WithReference(cache)
    .WaitFor(rabbitMQ)
    .WaitFor(cache);
var productApi = builder.AddProject<Projects.Product_API>("product")
    .WithAppSettingsEnvironments(cfg)
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(rabbitMQ)
    .WithReference(cache)
    .WaitFor(rabbitMQ)
    .WaitFor(cache);
var securityApi = builder.AddProject<Projects.Security_API>("security")
    .WithAppSettingsEnvironments(cfg)
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(rabbitMQ)
    .WithReference(cache)
    .WaitFor(rabbitMQ)
    .WaitFor(cache);

if (builder.Environment.IsDevelopment())
{
    var basePath = @"D:\Db\Dev";
    var cartDbFile = "Cart.db";
    var inventoryDbFile = "Inventory.db";
    var productDbFile = "Product.db";
    var securityDbFile = "Security.db";

    if (File.Exists(Path.Join(basePath, cartDbFile))) File.Delete(Path.Join(basePath, cartDbFile));
    if (File.Exists(Path.Join(basePath, inventoryDbFile))) File.Delete(Path.Join(basePath, inventoryDbFile));
    if (File.Exists(Path.Join(basePath, productDbFile))) File.Delete(Path.Join(basePath, productDbFile));
    if (File.Exists(Path.Join(basePath, securityDbFile))) File.Delete(Path.Join(basePath, securityDbFile));

    var cartDb = builder.AddSqlite("CartDb",basePath, cartDbFile)
        .WithSqliteWeb();
    var inventoryDb = builder.AddSqlite("InventoryDb", basePath, inventoryDbFile)
        .WithSqliteWeb();
    var productDb = builder.AddSqlite("ProductDb", basePath, productDbFile)
        .WithSqliteWeb();
    var securityDb = builder.AddSqlite("SecurityDb", basePath, securityDbFile)
        .WithSqliteWeb();
    cartApi.WithReference(cartDb);
    inventoryApi.WithReference(inventoryDb);
    productApi.WithReference(productDb);
    securityApi.WithReference(securityDb);
}

builder.Build().Run();
