using Common.Application.DTO.Base.Entities;
using Common.Infrastructure.Contracts;

namespace Security.Application.DTO
{
    public class PermissionDTO : AuditableGuidDTO, IAddDTO, IUpdateDTO, IResultDTO
    {
        public string Url { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Policy { get; set; }
    }
}
