using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Product.Infrastructure.gRPC.Protos;
using System.Reflection;


namespace Product.Infrastructure.gRPC.Extensions
{
    public static class AddGrpcClientsExtension
    {
        public static WebApplicationBuilder AddGrpcProductClients(this WebApplicationBuilder builder)
        {
            var executingAssembly = Assembly.GetExecutingAssembly().GetName().Name.Split('.', StringSplitOptions.RemoveEmptyEntries).First();
            var entryAssembly = Assembly.GetEntryAssembly().GetName().Name.Split('.', StringSplitOptions.RemoveEmptyEntries).First();
            builder.Services.AddGrpcClient<ProductService.ProductServiceClient>(c =>
            {
                if(executingAssembly != entryAssembly)
                c.Address = new Uri($"https://product");
                else
                c.Address = new Uri($"https://localhost:5008");
            });
            return builder;
        }
    }
}
