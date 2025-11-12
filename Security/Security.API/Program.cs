using Common.Api;
using Security.Application;
using Security.Application.Profiles;
using Security.Application.Rules;
using Security.Infrastructure.Persistence;
using Security.Infrastructure.Seeders;
using Security.Presentation.Contracts;

var app = WebApplication.CreateBuilder(args)
    .AddAutoMapperAssemblies(typeof(UserProfile).Assembly)
    .AddControllerAssemblies(typeof(IUsersController).Assembly)
    .AddValidatorAssemblies(typeof(UserLoginDTOValidator).Assembly)
    .AddServiceAssemblies(typeof(UserServices).Assembly)
    .AddSeederAssemblies(typeof(UserSeeder).Assembly)
    .BuildApi<SecurityDbContext>();

app.Run();