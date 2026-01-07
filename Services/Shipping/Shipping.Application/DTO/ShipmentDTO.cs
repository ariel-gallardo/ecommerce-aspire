using Common.Application.DTO.Base.Entities;
using Common.Application.DTO.ValueObjects;
using Common.Infrastructure.Contracts;

namespace Shipping.Application.DTO
{
    public class ShipmentDTO : AuditableGuidDTO, IAddDTO, IUpdateDTO, IReadDTO
    {
        public string TrackingNumber { get; set; }
        public string Carrier { get; set; }
        public string Status { get; set; }
        public AddressDTO ShippingAddress { get; set; }
    }
}
