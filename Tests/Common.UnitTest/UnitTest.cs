using Common.Api;
using Common.Application;
using Common.Contracts;
using Common.Infrastructure.Configurations;
using Common.Infrastructure.Seeder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Common.UnitTest
{
    public abstract class UnitTest<IDBContext> where IDBContext : DbContext
    {

        private readonly Assembly[] _mapperAssemblies;
        private readonly Assembly[] _rabbitMqAssemblies;
        private readonly Assembly[] _seederAssemblies;
        private readonly Assembly[] _serviceAssemblies;
        private readonly Assembly[] _controllerAssemblies;
        protected readonly IServiceProvider _services;
        protected readonly AppSettings _appSettings;

        protected UnitTest(Assembly[] mapperAssemblies, 
            Assembly[] rabbitMqAssemblies,
            Assembly[] seederAssemblies,
            Assembly[] serviceAssemblies, 
            Assembly[] controllerAssemblies)
        {
            _mapperAssemblies = mapperAssemblies;
            _rabbitMqAssemblies = rabbitMqAssemblies;
            _seederAssemblies = seederAssemblies;
            _serviceAssemblies = serviceAssemblies;
            _controllerAssemblies = controllerAssemblies;
            var builder = Host.CreateDefaultBuilder();
            builder.UseEnvironment("Testing");
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true);
            });

            builder.ConfigureServices((context, services) =>
            {
                Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");

                services.Configure<AppSettings>(context.Configuration.GetSection("Parameters:AppSettings"))
                .AddApplicationAutoMapper(_mapperAssemblies)
                .ConfigureRedis(context.Configuration)
                .ConfigureRabbitMq(context.Configuration, _rabbitMqAssemblies)
                .ConfigureDatabase<IDBContext>(context.Configuration)
                .AddApplicationServices(_serviceAssemblies);
                
                services.AddApi(_controllerAssemblies);
                
                var appSettings = context.Configuration.GetValue<string>("Parameters:AppSettings");
                using (var sp = services.BuildServiceProvider())
                {
                    var dbContext = sp.GetService<DbContext>();
                    dbContext.Database.EnsureCreated();
                }

                services.AddSeeders(context.HostingEnvironment, _seederAssemblies);

                Environment.SetEnvironmentVariable("Parameters:AppSettings", appSettings);
                services.AddHostedService<SecurityMicroserviceHostedService>();
            });

            var host = builder.Build();
            _services = host.Services;
            _appSettings = _services.GetService<IOptions<AppSettings>>().Value;
        }

        
        protected abstract Task Should_Get_All_Async();

        
        protected abstract Task Should_Get_By_Id_Async();


        protected abstract Task Should_Search_Async();

        
        protected abstract Task Should_Create_Entity_Async();

        
        protected abstract Task Should_Update_Entity_Async();

        
        protected abstract Task Should_Delete_Entity_Async();

    }
}
