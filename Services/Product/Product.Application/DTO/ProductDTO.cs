using Common.Application.DTO.Base.Entities;
using Common.Application.DTO.ValueObjects;
using Common.Infrastructure.Contracts;

namespace Product.Application.DTO
{
    public class ProductDTO : AuditableGuidDTO, IAddDTO, IUpdateDTO, IReadDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public PriceDTO Price { get; set; }
    }
}
