using Common.Api;
using Payment.Infrastructure.Persistence;

namespace Payment.API
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
            .BuildApi<PaymentDbContext>();
            app.Run();
        }
    }
}
