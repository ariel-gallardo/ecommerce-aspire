using Common.Domain.Entities.Base;
using Common.Domain.ValueObjects;

namespace Common.Domain.Entities
{
    public class Persona : AuditableGuidEntity
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public virtual Address? Address { get; set; }
    }
}
