using Common.Application.DTO.Base.Entities;
using Common.Infrastructure.Contracts;
using Security.Infrastructure.Entities;
using System.Text.Json.Serialization;

namespace Security.Application.DTO
{
    public class UserDTO : IdentifiableDTO, IReadDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public Guid? PersonaId { get; set; }
        public Role Rol { get; internal set; }
    }
}
