using Common.Infrastructure.Contracts;

namespace Common.Infrastructure.Persistence.Seeds.Base
{
    public interface ISeeder : IScoped
    {
        Task<IEnumerable<object>> SeedAsync(CancellationToken cancellationToken = default);
    }
}
