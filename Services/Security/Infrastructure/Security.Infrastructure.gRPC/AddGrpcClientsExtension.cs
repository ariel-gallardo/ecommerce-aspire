using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Security.Infrastructure.gRPC.Protos;
using System.Reflection;

namespace Security.Infrastructure.gRPC
{
    public static class AddGrpcClientsExtension
    {
        public static WebApplicationBuilder AddGrpcSecurityClients(this WebApplicationBuilder builder)
        {
            var executingAssembly = Assembly.GetExecutingAssembly().GetName().Name.Split('.', StringSplitOptions.RemoveEmptyEntries).First();
            var entryAssembly = Assembly.GetEntryAssembly().GetName().Name.Split('.', StringSplitOptions.RemoveEmptyEntries).First();
            builder.Services.AddGrpcClient<PermissionService.PermissionServiceClient>(c =>
            {
                if(executingAssembly != entryAssembly)
                c.Address = new Uri($"https://security");
                else
                c.Address = new Uri($"https://localhost:5009");
            });
            return builder;
        }
    }
}
