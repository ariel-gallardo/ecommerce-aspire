using Common.Api;
using Order.Infrastructure.Persistence;

namespace Order.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = WebApplication.CreateBuilder(args)
            .AddAutoMapperAssemblies()
            .AddControllerAssemblies()
            .AddSeederAssemblies()
            .AddValidatorAssemblies()
            .AddServiceAssemblies()
            .BuildApi<OrderDbContext>();
            app.Run();
        }
    }
}
