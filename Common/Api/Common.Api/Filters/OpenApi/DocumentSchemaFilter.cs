using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Common.Api.Filters.OpenApi
{
    public class DocumentSchemaFilter : IOpenApiDocumentTransformer
    {
        private readonly ISchemaGenerator _schemaGenerator;

        public DocumentSchemaFilter(ISchemaGenerator schemaGenerator)
        {
            _schemaGenerator = schemaGenerator;
        }

        private void AddValidationErrorSchema(OpenApiDocument document)
        {
            var schemaName = "ValidationError";
            if (document.Components.Schemas.ContainsKey(schemaName))
            {
                return;
            }
            var validationErrorSchema = new OpenApiSchema
            {
                Type = "object",
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    ["property"] = new OpenApiSchema { Type = "string" },
                    ["message"] = new OpenApiSchema { Type = "string" }
                }
            };
            document.Components.Schemas[schemaName] = validationErrorSchema;
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

            AddValidationErrorSchema(document);
        }
    }
}
