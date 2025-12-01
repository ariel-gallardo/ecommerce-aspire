using Common.Application.DTO.Base.Entities;
using Common.Contracts.DTO.ABM;

namespace Product.Application.DTO
{
    public class ProductDTO : AuditableGuidDTO, IAddDTO, IUpdateDTO, IResultDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ProductDTO dTO &&
                   base.Equals(obj) &&
                   Name == dTO.Name &&
                   Description == dTO.Description &&
                   CategoryId.Equals(dTO.CategoryId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Name, Description, CategoryId);
        }
    }
}
