
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace YapaChallenge.AppHost
{
    public static class Extensions
    {
        public static IResourceBuilder<ProjectResource> WithOpenApi(this IResourceBuilder<ProjectResource> builder, bool IsHttps = true)
        {
            IResourceBuilder<ProjectResource> builder2 = builder;
            builder2.WithCommand("OpenApi", "OpenApi", (ExecuteCommandContext context) => OnLinkOpenerCommandAsync(builder2, context, "/swagger/docs/v1/swagger.json"), new CommandOptions
            {
                IconName = "Accessibility",
                IconVariant = IconVariant.Filled
            });
            return builder2;
        }
        private static Task<ExecuteCommandResult> OnLinkOpenerCommandAsync(IResourceBuilder<ProjectResource> builder, ExecuteCommandContext context, string? Route = null, string? CustomUrl = null, bool IsHttps = true)
        {
            string fileName = (string.IsNullOrEmpty(CustomUrl) ? (builder.GetEndpoint(IsHttps ? "https" : "http").Url + Route) : CustomUrl);
            Process.Start(new ProcessStartInfo(fileName)
            {
                UseShellExecute = true,
                Verb = "open"
            });
            return Task.FromResult(CommandResults.Success());
        }
        public static IResourceBuilder<T> WithAppSettingsEnvironments<T>(
               this IResourceBuilder<T> builder,
               IConfiguration configuration)
               where T : IResourceWithEnvironment
        {
            var section = configuration.GetSection("Parameters:AppSettings");
            foreach (var child in section.GetChildren())
            {
                AddEnvironmentRecursive(builder, child, $"AppSettings__{child.Key}");
            }
            return builder;
        }

        private static void AddEnvironmentRecursive<T>(
            IResourceBuilder<T> builder,
            IConfigurationSection section,
            string prefix)
            where T : IResourceWithEnvironment
        {
            if (!section.GetChildren().Any())
            {
                builder.WithEnvironment(prefix, section.Value ?? string.Empty);
            }
            else
            {
                foreach (var child in section.GetChildren())
                {
                    AddEnvironmentRecursive(builder, child, $"{prefix}__{child.Key}");
                }
            }
        }
    }
}
