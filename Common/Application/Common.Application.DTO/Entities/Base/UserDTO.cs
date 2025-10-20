using Common.Application.DTO.Base.Entities;
using Common.Contracts.DTO.ABM;
using System.Text.Json.Serialization;

namespace Common.Application.DTO.Entities.Base
{
    public class UserDTO : AuditableDTO, IResultDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public Guid? PersonaId { get; set; }
        public string Rol { get; internal set; }
    }
}
