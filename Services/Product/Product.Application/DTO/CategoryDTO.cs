using Common.Application.DTO.Base.Entities;
using Common.Contracts.DTO.ABM;

namespace Product.Application.DTO
{
    public class CategoryDTO : AuditableGuidDTO, IAddDTO, IUpdateDTO, IResultDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ParentId { get; set; }
        public List<CategoryDTO> Children { get; set; }
    }
}
