using Common.Domain.Entities;
using Common.Domain.Entities.Base;

namespace Impecable.System.Domain.Entities
{
    public class Turno : AuditableEntity
    {
        public virtual Persona Cliente { get; set; }
        public Guid ClienteId { get; set; }
        public virtual Vehiculo Vehiculo { get; set; }
        public Guid VehiculoId { get; set; }
    }
}
