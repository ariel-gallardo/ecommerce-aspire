using Common.Application.DTO.Base.Entities;
using Common.Contracts.DTO.ABM;

namespace Security.Application.DTO
{
    public class PermissionDTO : AuditableGuidDTO, IAddDTO, IUpdateDTO, IResultDTO
    {
        public string Url { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Policy { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PermissionDTO dTO &&
                   base.Equals(obj) &&
                   Url == dTO.Url &&
                   Controller == dTO.Controller &&
                   Action == dTO.Action &&
                   Policy == dTO.Policy;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Url, Controller, Action, Policy);
        }
    }
}
