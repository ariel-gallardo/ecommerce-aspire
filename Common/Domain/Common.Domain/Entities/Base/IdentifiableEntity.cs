using Common.Contracts.Entities;

namespace Common.Domain.Entities.Base
{
    public class IdentifiableEntity : EntityBase, IIdentifiable
    {
        public ulong Id { get; set; }
    }
}
