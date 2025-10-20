using Common.Api;
using Impecable.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args)
    .AddDefaultAssemblies()
    .AddAutoMapperAssemblies(typeof(Impecable.Application.Profiles.TurnoProfile).Assembly)
    .AddValidatorAssemblies()
    .AddServiceAssemblies()
    .AddSeederDevelopmentAssemblies()
    .AddControllerAssemblies(typeof(Impecable.Controllers.TurnosController).Assembly);
var app = builder.BuildApi<ApplicationDbContext>();
app.Run();