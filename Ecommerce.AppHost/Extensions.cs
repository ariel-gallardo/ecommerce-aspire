
using Microsoft.Extensions.Configuration;

namespace Ecommerce.AppHost
{
    public static class Extensions
    {
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
