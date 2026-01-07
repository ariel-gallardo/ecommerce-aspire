using Common.Infrastructure.Entities.Enums;
using MassTransit.Transports;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Common.Api.Filters.OpenApi
{
    public class StandardNameSchemaFilter : IOpenApiSchemaTransformer
    {
        private static readonly Regex CleanRegex =
            new("(NullableOf|DTO|PagedList|Result|\\d+)", RegexOptions.Compiled);


        private static readonly IList<Type> _enumTypes = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsEnum)
            .ToList();

        private OpenApiSchema HandleSchema(OpenApiSchema schema)
        {



            if (!string.IsNullOrWhiteSpace(schema.Title))
            {
                schema.Title = CleanRegex.Replace(schema.Title, string.Empty);
            }

            if (schema.Annotations?.TryGetValue("x-schema-id", out var raw) == true &&
                raw is string value)
            {
                value = CleanRegex.Replace(value, string.Empty);
                schema.Annotations["x-schema-id"] = value;
                var enumType = _enumTypes.FirstOrDefault(x => x.Name == value);
                if (enumType != null)
                {
                    schema.Type = "string";
                    schema.Format = null;
                    schema.Enum = Enum.GetNames(enumType)
                        .Where(x => x != null)
                        .Select(name => (IOpenApiAny)new OpenApiString(name))
                        .ToList();
                    return schema;
                }
            }

            if (schema.Items != null)
            {
                HandleSchema(schema.Items);
            }
            return schema;
        }

        public async Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
        {
            if (schema == null) return;
            
            schema =  HandleSchema(schema);
            foreach (var key in schema.Properties.Keys)
            {
                schema.Properties[key] = HandleSchema(schema.Properties[key]);
            }
        }
    }
}
