using Common.Application.DTO.Base.Entities;
using Common.Infrastructure.Contracts;

namespace Cart.Application.DTO
{
    public class CartDTO : AuditableGuidDTO, IAddDTO, IUpdateDTO, IReadDTO
    {
        public List<CartItemReadDTO> Items { get; set; } = new();
    }
}
