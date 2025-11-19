using YapaChallenge.AppHost;
using Microsoft.Extensions.Hosting;

namespace YapaChallenge
{
    public class AppStartup
    {
        private readonly string[] _args;
        public AppStartup(string[] args)
        {
            _args = args;
        }

        public void Run()
        {
            var builder = DistributedApplication.CreateBuilder(_args);
            Configure(builder);
            builder.Build().Run();
        }

        private void Configure(IDistributedApplicationBuilder builder)
        {
            var cfg = builder.Configuration;
            var env = builder.Environment;

            var redisPass = builder.AddParameterFromConfiguration("RedisPassword", "Parameters:AppSettings:Redis:Password");
            var rabbitUser = builder.AddParameterFromConfiguration("RabbitUser", "Parameters:AppSettings:RabbitMQ:Username");
            var rabbitPass = builder.AddParameterFromConfiguration("RabbitPassword", "Parameters:AppSettings:RabbitMQ:Password");

            var clientDbPassword = builder.AddParameterFromConfiguration("ClientDbPassword", "Parameters:AppSettings:ClientDb:Password");
            var securityDbPassword = builder.AddParameterFromConfiguration("SecurityDbPassword", "Parameters:AppSettings:SecurityDb:Password");

            var redisDataMount = cfg["Parameters:AppSettings:Redis:DataMount"];
            var rabbitDataMount = cfg["Parameters:AppSettings:RabbitMQ:DataMount"];
            var clientDbDataMount = cfg["Parameters:AppSettings:ClientDb:DataMount"];
            var clientDbPort = int.Parse(cfg["Parameters:AppSettings:ClientDb:Port"]);
            var securityDbDataMount = cfg["Parameters:AppSettings:SecurityDb:DataMount"];
            var securityDbPort = int.Parse(cfg["Parameters:AppSettings:SecurityDb:Port"]);

            var cache = builder.AddRedis("cache", 1000, redisPass);
            if (!string.IsNullOrWhiteSpace(redisDataMount))
                cache = cache.WithDataBindMount(redisDataMount);

            var rabbitMQ = builder.AddRabbitMQ("rabbit", rabbitUser, rabbitPass, 1001);
            if (!string.IsNullOrWhiteSpace(rabbitDataMount))
                rabbitMQ = rabbitMQ.WithDataBindMount(rabbitDataMount);


            var basePathDbTesting = cfg["Parameters:AppSettings:DatabaseTestingPath"];
            var isTesting = env.IsEnvironment("Testing");

            var securityDbFile = "Security.sqlite";
            var dbSecurityTesting = isTesting ? builder
                .AddSqlite("SecurityDb", basePathDbTesting, securityDbFile).WithSqliteWeb() : null;

            var dbSecurity = builder.AddMySql("SecurityDb", securityDbPassword, securityDbPort)
                            .WithDataBindMount(securityDbDataMount)
                            .WithPhpMyAdmin();

            var clientDbFile = "Client.sqlite";
            var dbClientTesting = isTesting ? builder
                .AddSqlite("ClientDb", basePathDbTesting, clientDbFile)
            .WithSqliteWeb() : null;

            var dbClient = builder.AddMySql("ClientDb", clientDbPassword, clientDbPort)
                                      .WithDataBindMount(clientDbDataMount)
                                      .WithPhpMyAdmin();


            var securityApi = builder.AddProject<Projects.Security_API>("security")
                                         .WithOpenApi()
                                         .WithAppSettingsEnvironments(cfg)
                                         .WithExternalHttpEndpoints()
                                         .WithHttpHealthCheck("/health")
                                         .WithReference(rabbitMQ)
                                         .WithReference(cache)
                                         .WithReference(isTesting?dbSecurityTesting:dbSecurity)
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
                                       .WithReference(isTesting?dbClientTesting:dbClient)
                                       .WaitFor(rabbitMQ)
                                       .WaitFor(cache)
                                       .WaitFor(dbClient);

                var apiGateway = builder.AddProject<Projects.ApiGateway>("api-gateway")
                                        .WithExternalHttpEndpoints()
                                        .WithReference(securityApi)
                                        .WithReference(clientApi);
            
        }
    }
}
