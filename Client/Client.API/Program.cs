using Client.Application.Profiles;
using Client.Application.Rules;
using Client.Controllers;
using Client.Infrastructure.Persistence;
using Common.Api;

var app = WebApplication.CreateBuilder(args)
    .AddAutoMapperAssemblies(typeof(ClientProfile).Assembly)
    .AddControllerAssemblies(typeof(ClientesController).Assembly)
    .AddValidatorAssemblies(typeof(ClientDTOValidator).Assembly)
    .AddServiceAssemblies()
    .AddSeederAssemblies()
    .AddMessageAssemblies()
    .BuildApi<ClientDbContext>();

app.Run();