using Common.Infrastructure.Contracts;

namespace Common.Domain.Entities.Base
{
    public class IdentifiableGuidEntity : EntityBase, IIdentifiableGuid
    {
        public Guid Id { get; set; }
    }
}
