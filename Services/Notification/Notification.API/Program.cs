using Common.Api;
using Notification.Infrastructure.Persistence;

namespace Notification.API
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
            .BuildApi<NotificationDbContext>();
            app.Run();
        }
    }
}
