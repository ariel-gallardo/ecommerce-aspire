using Common.Contracts.Entities;
using Common.Domain.Entities;

namespace Common.Domain.Contracts.Entities
{
    public interface IAuditable : IIdentifiable
    {   
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}
