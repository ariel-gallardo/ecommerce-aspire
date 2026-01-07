using Common.Application.DTO.ValueObjects;

namespace Cart.Application.DTO
{
    public class CartItemResultDTO : CartItemDTO
    {
        public PriceDTO Price { get; set; }
        public string Name { get; set; }
    }
}
