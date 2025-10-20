
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Common.Domain.Converters
{
    public class EnumToStringConverter<TEnum> : ValueConverter<TEnum, string> where TEnum : struct, Enum
    {
        public EnumToStringConverter()
            : base(
                v => v.ToString(),           
                v => Enum.Parse<TEnum>(v)    
            )
        {
        }
    }
    public static class EnumStringConverter
    {
        public static string ToString<TEnum>(TEnum value) where TEnum : struct, Enum
            => value.ToString();

        public static TEnum FromString<TEnum>(string value) where TEnum : struct, Enum
            => Enum.Parse<TEnum>(value);
    }
}
