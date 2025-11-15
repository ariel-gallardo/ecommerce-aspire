using AutoMapper;
using Shipping.Application.DTO;
using Shipping.Domain.Entities;

namespace Shipping.Application.Profiles
{
    public class ShipmentProfile : Profile
    {
        public ShipmentProfile()
        {
            CreateMap<ShipmentDTO, Shipment>().ReverseMap();
        }
    }
}
