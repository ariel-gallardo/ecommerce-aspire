using Common.Api;
using Cart.Infrastructure.Persistence;
using Cart.Application.Profiles;
using Cart.Controllers;
using Cart.Infrastructure.Seeders;

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
            .BuildApi<CartDbContext>();
            app.Run();
        }
    }
}
