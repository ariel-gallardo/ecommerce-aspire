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


var securityApi = builder.AddProject<Projects.Security_API>("security")
    .WithOpenApi()
    .WithAppSettingsEnvironments(cfg)
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(rabbitMQ)
    .WithReference(cache)
    .WaitFor(rabbitMQ)
    .WaitFor(cache);

var apiGateway = builder.AddProject<Projects.ApiGateway>("api-gateway")
    .WithExternalHttpEndpoints()
    .WithReference(securityApi);


if (builder.Environment.IsDevelopment())
{
    var basePath = cfg["Parameters:AppSettings:DatabaseDevPath"];
    var securityDbFile = "Security.sqlite";

    var securityDb = builder.AddSqlite("SecurityDb", basePath, securityDbFile)
        .WithSqliteWeb();
    securityApi.WithReference(securityDb);
}

builder.Build().Run();
