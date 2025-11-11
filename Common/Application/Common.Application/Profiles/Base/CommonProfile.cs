using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Common.Application.Profiles.Base
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap<Guid, string>().ConvertUsing(src => src.ToString());
            CreateMap<string, Guid>().ConvertUsing(src => Guid.Parse(src));
            CreateMap(typeof(Enum), typeof(string)).ConvertUsing(typeof(EnumToStringConverter<>));
            CreateMap(typeof(string), typeof(Enum)).ConvertUsing(typeof(StringToEnumConverter<>));
        }
    }
}
