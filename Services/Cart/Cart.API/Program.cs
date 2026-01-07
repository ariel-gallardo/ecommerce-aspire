using Cart.Application.Pipelines.AssociatePriceAndName.Cart;
using Cart.Application.Profiles;
using Cart.Controllers;
using Cart.Infrastructure.Persistence;
using Cart.Infrastructure.Seeders;
using Common.Api;
using Product.Infrastructure.gRPC.Extensions;

namespace Cart.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = WebApplication.CreateBuilder(args)
            .AddAutoMapperAssemblies(typeof(CartProfile).Assembly)
            .AddControllerAssemblies(typeof(CartController).Assembly)
            .AddSeederAssemblies(typeof(CartSeeder).Assembly)
            .AddValidatorAssemblies()
            .AddServiceAssemblies()
            .AddPipelinesAssemblies(typeof(SearchQuerieSinglePipeline).Assembly)
            .AddGrpcProductClients()
            .BuildApi<CartDbContext>();
            
            app.Run();
        }
    }
}
