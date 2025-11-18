using Client.Infrastructure.Persistence;
using Common.Api;

var app = WebApplication.CreateBuilder(args)
    .AddAutoMapperAssemblies()
    .AddControllerAssemblies()
    .AddValidatorAssemblies()
    .AddServiceAssemblies()
    .AddSeederAssemblies()
    .AddMessageAssemblies()
    .BuildApi<ClientDbContext>();

app.Run();