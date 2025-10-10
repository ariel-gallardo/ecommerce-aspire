using Common.Domain.Entities;
using Common.Domain.Entities.Base;

namespace Impecable.System.Domain.Entities
{
    public class Vehiculo : AuditableEntity
    {
        public string Dominio { get; set; }
        public virtual Persona Cliente { get; set; }
        public Guid ClienteId { get; set; }
    }
}
