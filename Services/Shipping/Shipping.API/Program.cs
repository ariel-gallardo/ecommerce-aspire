using Common.Api;
using Shipping.Application.Profiles;
using Shipping.Controllers;
using Shipping.Infrastructure.Persistence;
using Shipping.Infrastructure.Seeders;

namespace Shipping.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = WebApplication.CreateBuilder(args)
            .AddAutoMapperAssemblies(typeof(ShipmentProfile).Assembly)
            .AddControllerAssemblies(typeof(ShipmentController).Assembly)
            .AddSeederAssemblies(typeof(ShipmentSeeder).Assembly)
            .AddValidatorAssemblies()
            .AddServiceAssemblies()
            .BuildApi<ShippingDbContext>();
            app.Run();
        }
    }
}
