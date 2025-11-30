using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Common.Application.Profiles.Base
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap<long, string>().ConvertUsing(src => src.ToString());
            CreateMap<string, long>().ConvertUsing(src => long.Parse(src));
            CreateMap(typeof(Enum), typeof(string)).ConvertUsing(typeof(EnumToStringConverter<>));
            CreateMap(typeof(string), typeof(Enum)).ConvertUsing(typeof(StringToEnumConverter<>));
        }
    }
}
