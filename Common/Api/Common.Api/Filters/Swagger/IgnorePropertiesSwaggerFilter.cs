using Common.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Common.Api.Filters.Swagger
{
    public class IgnorePropertiesSwaggerFilter<I,A> : ISchemaFilter where A : Attribute
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;

            var ignoreAttribute = type.GetCustomAttribute<A>();
            if (ignoreAttribute == null)
                return;

            if (!typeof(I).IsAssignableFrom(type))
                return;

            var auditableProps = typeof(I)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => p.Name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var propName in auditableProps)
            {
                var camelCase = propName.ToCamelCase();
                if (schema.Properties.ContainsKey(camelCase))
                    schema.Properties.Remove(camelCase);
            }
        }
    }
}
