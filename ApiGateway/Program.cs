using Microsoft.Extensions.Options;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;

namespace ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddServiceDefaults();
            builder.Services.AddAuthorization();
            builder.Services.AddOcelot(builder.Configuration);
            builder.Services.AddSwaggerForOcelot(builder.Configuration);
            builder.Services.AddCors(o =>
            {
                o.AddPolicy("AllowMySite", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("X-Current-Page", "X-Total-Pages", "X-Page-Size", "X-Total-Count");
                });
            });
            var app = builder.Build();
            app.UseCors("AllowMySite");
            app.UseSwaggerForOcelotUI(opt =>
            {
                opt.PathToSwaggerGenerator = "/swagger/docs";
            }).UseOcelot()
            .Wait();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.Run();
        }
    }
}
