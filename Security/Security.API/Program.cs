using Common.Api;
using Security.Application;
using Security.Application.Profiles;
using Security.Application.Rules;
using Security.Controllers;
using Security.Infrastructure.Messaging.Messages.Request;
using Security.Infrastructure.Persistence;
using Security.Infrastructure.Seeders;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args)
            .AddAutoMapperAssemblies(typeof(UserProfile).Assembly)
            .AddControllerAssemblies(typeof(UsersController).Assembly)
            .AddValidatorAssemblies(typeof(UserLoginDTOValidator).Assembly)
            .AddServiceAssemblies(typeof(UserServices).Assembly)
            .AddSeederAssemblies(typeof(UserSeeder).Assembly)
            .AddMessageAssemblies(typeof(LoadPermissionRequest).Assembly);

        var app = builder.BuildApi<SecurityDbContext>();

        app.Run();
    }
}
