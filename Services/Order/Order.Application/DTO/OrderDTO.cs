using Common.Application.DTO.Base.Entities;
using Common.Application.DTO.ValueObjects;
using Common.Contracts.DTO.ABM;

namespace Order.Application.DTO
{
    public class OrderDTO : AuditableDTO, IAddDTO, IUpdateDTO
    {
        public List<OrderItemDTO> Items { get; set; } = new();
        public AddressDTO Address { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is OrderDTO dTO &&
                   base.Equals(obj) &&
                   EqualityComparer<List<OrderItemDTO>>.Default.Equals(Items, dTO.Items) &&
                   EqualityComparer<AddressDTO>.Default.Equals(Address, dTO.Address);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Items, Address);
        }
    }
}
