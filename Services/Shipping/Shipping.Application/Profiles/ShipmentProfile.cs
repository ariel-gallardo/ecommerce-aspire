using AutoMapper;
using Common.Application.DTO.Base.Entities;
using Common.Domain.Entities.Base;
using Shipping.Application.DTO;
using Shipping.Domain.Entities;

namespace Shipping.Application.Profiles
{
    public class ShipmentProfile : Profile
    {
        public ShipmentProfile()
        {
            CreateMap<ShipmentDTO, Shipment>()
                .IncludeBase<AuditableGuidDTO, AuditableGuidEntity>()
                .ReverseMap();
        }
    }
}
