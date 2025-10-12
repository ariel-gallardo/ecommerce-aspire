using Microsoft.EntityFrameworkCore;

namespace Common.Infrastructure.Persistence.Seeds.Base
{
    public interface ISeeder
    {
        Task SeedAsync(DbContext context);
    }
}
