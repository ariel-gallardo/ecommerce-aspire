using Common.Domain.DTOS.Base.Entities;

namespace Application.DTOS.Entities
{
    public class PersonaDTO : AuditableDTO
    {
        public string Name { get; set; }
        public string Lastname { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PersonaDTO dTO &&
                   base.Equals(obj) &&
                   Name == dTO.Name &&
                   Lastname == dTO.Lastname;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Name, Lastname);
        }
    }
}
