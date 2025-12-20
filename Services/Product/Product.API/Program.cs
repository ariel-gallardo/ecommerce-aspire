using Common.Api;
using Product.Application.Pipelines;
using Product.Application.Profiles;
using Product.Controllers;
using Product.Domain.Modifier;
using Product.Infrastructure.Persistence;
using Product.Infrastructure.Seeders;

namespace Product.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = WebApplication.CreateBuilder(args)
            .AddAutoMapperAssemblies(typeof(ProductProfile).Assembly)
            .AddControllerAssemblies(typeof(ProductController).Assembly)
            .AddSeederAssemblies(typeof(ProductSeeder).Assembly)
            .AddValidatorAssemblies()
            .AddServiceAssemblies()
            .AddQuerieModifierAssemblies(typeof(CategoryQueryModifier).Assembly)
            .AddPipelinesAssemblies(typeof(CategorySearchByFiltersPipeline).Assembly)
            .BuildApi<ProductDbContext>();
            app.Run();
        }
    }
}
