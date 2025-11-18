using Ecommerce.AppHost;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);
var cfg = builder.Configuration;

var redisDataMount = cfg["Parameters:AppSettings:Redis:DataMount"];
var rabbitDataMount = cfg["Parameters:AppSettings:RabbitMQ:DataMount"];
var clientDbDataMount = cfg["Parameters:AppSettings:ClientDb:DataMount"];
var clientDbPort = Int32.Parse(cfg["Parameters:AppSettings:ClientDb:Port"]);
var securityDbDataMount = cfg["Parameters:AppSettings:SecurityDb:DataMount"];
var securityDbPort = Int32.Parse(cfg["Parameters:AppSettings:SecurityDb:Port"]);

var redisPass = builder.AddParameterFromConfiguration("RedisPassword", "Parameters:AppSettings:Redis:Password");
var rabbitUser = builder.AddParameterFromConfiguration("RabbitUser", "Parameters:AppSettings:RabbitMQ:Username");
var rabbitPass = builder.AddParameterFromConfiguration("RabbitPassword", "Parameters:AppSettings:RabbitMQ:Password");

var clientDbPassword = builder.AddParameterFromConfiguration("ClientDbPassword", "Parameters:AppSettings:ClientDb:Password");
var securityDbPassword = builder.AddParameterFromConfiguration("SecurityDbPassword", "Parameters:AppSettings:SecurityDb:Password");


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

var dbSecurity = builder.AddMySql("SecurityDb", securityDbPassword, securityDbPort)
    .WithDataBindMount(securityDbDataMount)
    .WithPhpMyAdmin();

var dbClient = builder.AddMySql("ClientDb",clientDbPassword, clientDbPort)
    .WithDataBindMount(clientDbDataMount)
    .WithPhpMyAdmin();

var securityApi = builder.AddProject<Projects.Security_API>("security")
    .WithOpenApi()
    .WithAppSettingsEnvironments(cfg)
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(rabbitMQ)
    .WithReference(cache)
    .WithReference(dbSecurity)
    .WaitFor(rabbitMQ)
    .WaitFor(cache)
    .WaitFor(dbSecurity);

var clientApi = builder.AddProject<Projects.Client_API>("client")
    .WithOpenApi()
    .WithAppSettingsEnvironments(cfg)
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(rabbitMQ)
    .WithReference(cache)
    .WithReference(dbClient)
    .WaitFor(rabbitMQ)
    .WaitFor(cache)
    .WaitFor(dbClient);

var apiGateway = builder.AddProject<Projects.ApiGateway>("api-gateway")
    .WithExternalHttpEndpoints()
    .WithReference(securityApi)
    .WithReference(clientApi);

builder.Build().Run();
