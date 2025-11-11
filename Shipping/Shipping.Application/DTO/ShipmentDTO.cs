using Common.Application.DTO.Base.Entities;
using Common.Application.DTO.ValueObjects;
using Common.Contracts.DTO.ABM;

namespace Shipping.Application.DTO
{
    public class ShipmentDTO : AuditableDTO, IAddDTO, IUpdateDTO
    {
        public string TrackingNumber { get; set; }
        public string Carrier { get; set; }
        public string Status { get; set; }
        public AddressDTO ShippingAddress { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ShipmentDTO dTO &&
                   base.Equals(obj) &&
                   TrackingNumber == dTO.TrackingNumber &&
                   Carrier == dTO.Carrier &&
                   Status == dTO.Status &&
                   EqualityComparer<AddressDTO>.Default.Equals(ShippingAddress, dTO.ShippingAddress);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), TrackingNumber, Carrier, Status, ShippingAddress);
        }
    }
}
