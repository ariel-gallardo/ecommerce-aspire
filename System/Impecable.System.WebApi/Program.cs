using Application.Contracts.Services;
using Common.Application;
using Common.Application.Rules;
using Common.Domain.Configurations;
using Common.Domain.Contracts.Repositories;
using Common.Domain.Contracts.Services;
using Common.Infrastructure;
using Common.Infrastructure.Persistence.Seeds;
using Common.SwaggerExamples.UserLogin;
using FluentValidation;
using FluentValidation.AspNetCore;
using Impecable.System.Infrastructure.Data;
using Impecable.System.WebApi.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;


builder.Services.AddValidatorsFromAssemblies(new[]{ typeof(UserLoginDTOValidator).Assembly });

builder.Services.AddControllers(o =>
{
    o.Filters.Add<FluentValidationFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition =
        System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
}).ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.ExampleFilters();
});

builder.Services.AddSwaggerExamplesFromAssemblies(typeof(UserLoginRequestExample).Assembly);

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
builder.Services.AddScoped<IAuthServices, AuthServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userService = services.GetRequiredService<IAuthServices>();
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