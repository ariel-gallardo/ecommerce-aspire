using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Product.Infrastructure.gRPC.Protos;


namespace Product.Infrastructure.gRPC.Extensions
{
    public static class AddGrpcClientsExtension
    {
        public static WebApplicationBuilder AddGrpcProductClients(this WebApplicationBuilder builder)
        {
            builder.Services.AddGrpcClient<ProductService.ProductServiceClient>(c =>
            {
                c.Address = new Uri("https://product");
            });
            return builder;
        }
    }
}
