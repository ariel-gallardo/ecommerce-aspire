using Common.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Common.Infrastructure.Persistence.Seeds.Base
{
    public interface ISeeder : IScoped
    {
        Task SeedAsync(DbContext context);
    }
}
