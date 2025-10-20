using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Common.Api.CustomAttributes;
using Common.Contracts.DTO.Base;

namespace Common.Api.Converters.JSON
{
    public class IdentifiableIgnoringConverter<T> : JsonConverter<T>
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<T>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var type = value.GetType();
            var ignoreAttribute = type.GetCustomAttribute<IgnoreAuditableAttribute>();

            if (ignoreAttribute == null)
            {
                JsonSerializer.Serialize(writer, value, options);
                return;
            }

            var identifiableProperties = typeof(IIdentifiableDTO)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => p.Name)
                .ToHashSet();

            writer.WriteStartObject();

            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!identifiableProperties.Contains(prop.Name))
                {
                    writer.WritePropertyName(prop.Name);
                    var propertyValue = prop.GetValue(value);
                    JsonSerializer.Serialize(writer, propertyValue, options);
                }
            }

            writer.WriteEndObject();
        }
    }
}
