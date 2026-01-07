using Common.Infrastructure.Persistence;
using Logs.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logs.Infrastructure.Persistence.Configurations
{
    public class LogErrorConfiguration : ConfigurationBase<LogError>
    {
        public override void Configure(EntityTypeBuilder<LogError> builder)
        {
            ConfigureAuditableGuid(builder);
            builder.Property(x => x.StackTrace);
            builder.Property(x => x.Exception);
            builder.Property(x => x.ExceptionType);
            builder.HasIndex(x => x.ExceptionType);
        }
    }
}
