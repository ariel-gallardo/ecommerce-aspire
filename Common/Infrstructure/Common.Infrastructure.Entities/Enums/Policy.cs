using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Common.Infrastructure.Entities.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Policy
    {
        [EnumMember(Value = "Access.Unknown")]
        Unknown,
        [EnumMember(Value = "Access.Administrator")]
        Administrator,
        [EnumMember(Value = "Access.Support")]
        Support,
        [EnumMember(Value = "Access.Client")]
        Client,
        [EnumMember(Value = "Access.Public")]
        Public
    }
}
