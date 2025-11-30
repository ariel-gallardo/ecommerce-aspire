using Common.Application.DTO.Base.Entities;
using Common.Contracts.DTO.ABM;

namespace Product.Application.DTO
{
    public class CategoryDTO : AuditableDTO, IAddDTO, IUpdateDTO, IResultDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ParentId { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CategoryDTO dTO &&
                   base.Equals(obj) &&
                   Name == dTO.Name &&
                   Description == dTO.Description &&
                   ParentId == dTO.ParentId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Name, Description, ParentId);
        }
    }
}
