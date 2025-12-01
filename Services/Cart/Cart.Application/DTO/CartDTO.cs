using Common.Application.DTO.Base.Entities;
using Common.Contracts.DTO.ABM;

namespace Cart.Application.DTO
{
    public class CartDTO : AuditableGuidDTO, IAddDTO, IUpdateDTO, IResultDTO
    {
        public List<CartItemDTO> Items { get; set; } = new();

        public override bool Equals(object? obj)
        {
            return obj is CartDTO dTO &&
                   base.Equals(obj) &&
                   EqualityComparer<List<CartItemDTO>>.Default.Equals(Items, dTO.Items);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Items);
        }
    }
}
