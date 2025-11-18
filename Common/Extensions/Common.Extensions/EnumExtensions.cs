using System.Reflection;
using System.Runtime.Serialization;

namespace Common.Extensions
{
    public static class EnumExtensions
    {
        public static string AsStringUsingMemberValue(this Enum @enum)
        {
            var member = @enum.GetType().GetMember(@enum.ToString()).FirstOrDefault();

            if (member != null)
            {
                var enumMemberAttribute = member.GetCustomAttribute<EnumMemberAttribute>(false);
                if (enumMemberAttribute != null && !string.IsNullOrWhiteSpace(enumMemberAttribute.Value))
                {
                    return enumMemberAttribute.Value;
                }
            }
            return @enum.ToString();
        }

        public static T AsEnumUsingMemberValue<T>(this string enumValueString) where T : struct, Enum
        {
            if (!typeof(T).IsEnum) throw new ArgumentException($"The generic type must be an Enum. The current type is: {typeof(T).FullName}");

            if (string.IsNullOrWhiteSpace(enumValueString)) throw new ArgumentNullException(nameof(enumValueString));

            var enumType = typeof(T);
            var members = enumType.GetMembers(BindingFlags.Public | BindingFlags.Static);

            foreach (var member in members)
            {
                if (member.MemberType == MemberTypes.Field)
                {
                    var enumMemberAttribute = member.GetCustomAttribute<EnumMemberAttribute>(false);

                    if (enumMemberAttribute != null && !string.IsNullOrWhiteSpace(enumMemberAttribute.Value) &&
                        enumMemberAttribute.Value.Equals(enumValueString, StringComparison.OrdinalIgnoreCase))
                    {
                        return (T)Enum.Parse(enumType, member.Name, true);
                    }
                }
            }

            if (Enum.TryParse(enumValueString, true, out T result)) return result;
            throw new ArgumentException($"Could not convert the string '{enumValueString}' to the Enum '{enumType.Name}'.");
        }
    }
}
