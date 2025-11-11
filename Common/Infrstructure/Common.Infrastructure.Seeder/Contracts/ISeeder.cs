using Common.Contracts;

namespace Common.Infrastructure.Persistence.Seeds.Base
{
    public interface ISeeder : IScoped
    {
        Task SeedAsync(CancellationToken cancellationToken = default);
    }
}
