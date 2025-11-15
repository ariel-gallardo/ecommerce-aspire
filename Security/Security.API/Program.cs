using Common.Api;
using Security.Application;
using Security.Application.Profiles;
using Security.Application.Rules;
using Security.Controllers;
using Security.Infrastructure.Messaging.Consumer;
using Security.Infrastructure.Persistence;
using Security.Infrastructure.Seeders;

var app = WebApplication.CreateBuilder(args)
    .AddAutoMapperAssemblies(typeof(UserProfile).Assembly)
    .AddControllerAssemblies(typeof(UsersController).Assembly)
    .AddValidatorAssemblies(typeof(UserLoginDTOValidator).Assembly)
    .AddServiceAssemblies(typeof(UserServices).Assembly)
    .AddSeederAssemblies(typeof(UserSeeder).Assembly)
    .AddMessageAssemblies(typeof(LoadPermissionRequestConsumer).Assembly)
    .BuildApi<SecurityDbContext>();

app.Run();