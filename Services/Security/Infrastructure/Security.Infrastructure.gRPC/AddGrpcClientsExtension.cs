using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Security.Infrastructure.gRPC.Protos;

namespace Security.Infrastructure.gRPC
{
    public static class AddGrpcClientsExtension
    {
        public static WebApplicationBuilder AddGrpcSecurityClients(this WebApplicationBuilder builder)
        {
            builder.Services.AddGrpcClient<PermissionService.PermissionServiceClient>(c =>
            {
                c.Address = new Uri("https://security");
            });
            return builder;
        }
    }
}
