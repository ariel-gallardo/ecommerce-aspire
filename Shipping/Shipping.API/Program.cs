namespace Shipping.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = WebApplication.CreateBuilder(args)
            .AddAutoMapperAssemblies()
            .AddControllerAssemblies()
            .AddSeederDevelopmentAssemblies()
            .AddValidatorAssemblies()
            .AddServiceAssemblies()
            .BuildApi<>();
            app.Run();
        }
    }
}
