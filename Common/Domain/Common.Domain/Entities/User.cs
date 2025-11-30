using Common.Domain.Entities.Base;
using Security.Infrastructure.Entities;

namespace Common.Domain.Entities
{
    public class User : IdentifiableEntity
    {
        public Role Rol { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid? PersonaId { get; set; }
        public virtual Persona Persona { get; set; }
    }
}
