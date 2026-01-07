using Common.Api;
using Product.Application.gRPC;
using Product.Application.Profiles;
using Product.Controllers;
using Product.Domain.Modifier;
using Product.Infrastructure.Persistence;
using Product.Infrastructure.Seeders;
using ProductGRPCProfile = Product.Application.gRPC.Profiles.ProductProfile;

namespace Product.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = WebApplication.CreateBuilder(args)
            .AddAutoMapperAssemblies(typeof(ProductProfile).Assembly, typeof(ProductGRPCProfile).Assembly)
            .AddControllerAssemblies(typeof(ProductController).Assembly)
            .AddSeederAssemblies(typeof(ProductSeeder).Assembly)
            .AddValidatorAssemblies()
            .AddServiceAssemblies()
            .AddQuerieModifierAssemblies(typeof(CategoryQueryModifier).Assembly)
            .AddPipelinesAssemblies()
            .AddGrpcAssemblies(typeof(ProductGrpcService).Assembly)
            .BuildApi<ProductDbContext>();
            app.Run();
        }
    }
}
