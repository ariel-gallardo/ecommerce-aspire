using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

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
            builder.Services.AddSwaggerForOcelot(builder.Configuration, swaggerSetup: x =>
            {
                x.DocInclusionPredicate((docName, apiDesc) =>
                    {
                        return new[] {
                        "GET", "POST", "PUT", "PATCH", "DELETE", "OPTIONS", "HEAD"
                    }.Contains(apiDesc.HttpMethod);
                });
            });
            var app = builder.Build();
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
