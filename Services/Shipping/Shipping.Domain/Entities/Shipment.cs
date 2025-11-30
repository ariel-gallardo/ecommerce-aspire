using Common.Domain.Entities.Base;
using Common.Domain.ValueObjects;
using Shipping.Domain.Enums;

namespace Shipping.Domain.Entities
{
    public class Shipment : AuditableEntity
    {
        public string TrackingNumber { get; set; }
        public string Carrier { get; set; }
        public ShipmentStatus Status { get; set; }
        public Address ShippingAddress { get; set; }
    }
}
