using Common.Api;
using Invoice.Infrastructure.Persistence;

namespace Invoice.API
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
            .BuildApi<InvoiceDbContext>();
            app.Run();
        }
    }
}
