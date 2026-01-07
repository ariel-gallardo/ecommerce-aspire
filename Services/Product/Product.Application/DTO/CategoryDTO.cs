using Common.Application.DTO.Base.Entities;
using Common.Infrastructure.Contracts;

namespace Product.Application.DTO
{
    public class CategoryDTO : AuditableGuidDTO, IAddDTO, IUpdateDTO, IReadDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ParentId { get; set; }
        public List<CategoryDTO> Children { get; set; }
    }
}
