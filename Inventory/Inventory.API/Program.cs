using Common.Api;
using Inventory.Application.Profiles;
using Inventory.Controllers;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Seeders;

namespace Inventory.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = WebApplication.CreateBuilder(args)
            .AddAutoMapperAssemblies(typeof(InventoryItemProfile).Assembly)
            .AddControllerAssemblies(typeof(InventoryItemController).Assembly)
            .AddSeederAssemblies(typeof(InventoryItemSeeder).Assembly)
            .AddValidatorAssemblies()
            .AddServiceAssemblies()
            .BuildApi<InventoryDbContext>();
            app.Run();
        }
    }
}
