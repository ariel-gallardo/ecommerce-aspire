using Common.Api;
using Impecable.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args)
    .AddAutoMapperAssemblies(typeof(Impecable.Application.Profiles.TurnoProfile).Assembly)
    .AddValidatorAssemblies()
    .AddServiceAssemblies();
var app = builder.BuildApi<ApplicationDbContext>();
app.Run();