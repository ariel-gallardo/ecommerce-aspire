using Common.Application.DTO.Base.Entities;
using Common.Infrastructure.Contracts;

namespace Cart.Application.DTO
{
    public class CartDTO : AuditableGuidDTO, IAddDTO, IUpdateDTO, IResultDTO
    {
        public List<CartItemResultDTO> Items { get; set; } = new();
    }
}
