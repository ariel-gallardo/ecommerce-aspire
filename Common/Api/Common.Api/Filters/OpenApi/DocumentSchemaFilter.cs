using Common.Infrastructure.Entities;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Data;

namespace Common.Api.Filters.OpenApi
{
    public class DocumentSchemaFilter : IOpenApiDocumentTransformer
    {
        private readonly ISchemaGenerator _schemaGenerator;

        public DocumentSchemaFilter(ISchemaGenerator schemaGenerator)
        {
            _schemaGenerator = schemaGenerator;
        }
        public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            if (document == null)
            {
                return;
            }

            if (document.Components == null)
            {
                document.Components = new OpenApiComponents();
            }

            if (document.Components.Schemas == null)
            {
                document.Components.Schemas = new Dictionary<string, OpenApiSchema>();
            }

            var customSchemaTypes = new[] { typeof(ValidationError) };
            foreach (var type in customSchemaTypes)
            {
                if (!document.Components.Schemas.ContainsKey(type.Name))
                {
                    var validationErrorSchema = _schemaGenerator.GenerateSchema(type,new SchemaRepository());
                    document.Components.Schemas[type.Name] = validationErrorSchema;
                }
            }
        }
    }
}
