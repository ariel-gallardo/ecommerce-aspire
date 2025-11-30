using Common.Extensions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Common.Infrastructure.Converters
{
    public class EnumValueToStringConverter<TEnum> : ValueConverter<TEnum, string>
            where TEnum : struct, Enum
    {
        public EnumValueToStringConverter()
            : base(
                enumValue => enumValue.AsStringUsingMemberValue(),
                stringValue => stringValue.AsEnumUsingMemberValue<TEnum>()
            )
        {
        }

    }
}
