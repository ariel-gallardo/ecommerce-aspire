using Common.Api;
using Security.Application;
using Security.Application.gRPC;
using Security.Application.Profiles;
using Security.Application.Rules;
using Security.Controllers;
using Security.Infrastructure.Persistence;
using Security.Infrastructure.Seeders;
using Security.Infrastructure.gRPC;

var app = WebApplication.CreateBuilder(args)
    .AddAutoMapperAssemblies(typeof(UserProfile).Assembly)
    .AddControllerAssemblies(typeof(UsersController).Assembly)
    .AddValidatorAssemblies(typeof(UserLoginDTOValidator).Assembly)
    .AddServiceAssemblies(typeof(UserServices).Assembly)
    .AddSeederAssemblies(typeof(UserSeeder).Assembly)
    .AddMessageAssemblies()
    .AddGrpcAssemblies(typeof(PermissionGrpcService).Assembly)
    .AddGrpcSecurityClients()
    .BuildApi<SecurityDbContext>();

app.Run();