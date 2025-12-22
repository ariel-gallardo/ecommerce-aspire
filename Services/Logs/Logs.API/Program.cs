using Common.Api;
using Logs.Application.Profiles;
using Logs.Controllers;
using Logs.Infrastructure.Messaging.Consumer;
using Logs.Infrastructure.Persistence;

var app = WebApplication.CreateBuilder(args)
    .AddAutoMapperAssemblies(typeof(LogProfile).Assembly)
    .AddControllerAssemblies(typeof(InfoController).Assembly)
    .AddValidatorAssemblies()
    .AddServiceAssemblies()
    .AddSeederAssemblies()
    .AddMessageAssemblies(typeof(LogErrorRequestConsumer).Assembly)
    .BuildApi<LogsDbContext>();

app.Run();