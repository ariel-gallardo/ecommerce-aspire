
using AutoMapper;
using Common.Application.DTO.Entities.Base;
using Common.Domain.ValueObjects;

namespace Common.Application.Profiles
{
    public class CoordinatesProfile : Profile
    {
        public CoordinatesProfile()
        {
            CreateMap<Coordinates, CoordinatesDTO>().ReverseMap();
        }
    }
}
