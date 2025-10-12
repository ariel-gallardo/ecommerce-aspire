using Common.Domain.DTOS.Base.Entities;
using System.Text.Json.Serialization;

namespace Common.Application.DTOS.Entities.User
{
    public class UserDTO : AuditableDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public Guid? PersonaId { get; set; }
    }
}
