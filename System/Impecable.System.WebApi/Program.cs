using Common.Domain.Configurations;
using Common.Domain.Contracts.Repositories;
using Common.Domain.Contracts.Services;
using Common.Infrastructure;
using Common.Infrastructure.Persistence.Seeds;
using Impecable.System.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        if (env.IsDevelopment())
        {
            options.UseInMemoryDatabase("InMemoryDB");
        }
    }
);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(
    typeof(Base.Application.Profiles.UserProfile).Assembly,
    typeof(Impecable.System.Application.Profiles.TurnoProfile).Assembly
);
builder.Services.AddScoped<DbContext, ApplicationDbContext>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userService = services.GetRequiredService<IUserServices>();
        var appSettings = services.GetRequiredService<IOptions<AppSettings>>();
        Seeders.Seed(context, userService, appSettings);
    }
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();