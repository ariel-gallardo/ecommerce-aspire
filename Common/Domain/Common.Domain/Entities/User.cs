using Common.Domain.Entities.Base;
using Common.Domain.Enums;
namespace Common.Domain.Entities
{
    public class User : IdentifiableEntity
    {
        public RoleEnum Rol { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid? PersonaId { get; set; }
        public virtual Persona Persona { get; set; }
    }
}
