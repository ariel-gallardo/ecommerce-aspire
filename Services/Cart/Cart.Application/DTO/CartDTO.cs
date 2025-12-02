using Common.Application.DTO.Base.Entities;
using Common.Contracts.DTO.ABM;

namespace Cart.Application.DTO
{
    public class CartDTO : AuditableGuidDTO, IAddDTO, IUpdateDTO, IResultDTO
    {
        public List<CartItemDTO> Items { get; set; } = new();
    }
}
