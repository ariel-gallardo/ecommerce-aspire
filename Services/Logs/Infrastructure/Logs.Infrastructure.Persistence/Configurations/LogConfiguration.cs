using Common.Infrastructure.Persistence;
using Logs.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logs.Infrastructure.Persistence.Configurations
{
    public class LogConfiguration : ConfigurationBase<Log>
    {
        public override void Configure(EntityTypeBuilder<Log> builder)
        {
            ConfigureAuditableGuid(builder);
            builder.Property(x => x.ServiceName);
            builder.Property(x => x.Message);
            builder.Property(x => x.TraceId);
            builder.HasOne(x => x.Error).WithOne(x => x.Log)
                .HasForeignKey<Log>(x => x.ErrorId)
                .IsRequired(false);

            builder.HasIndex(x => x.ServiceName);
            builder.HasIndex(x => x.TraceId);
        }
    }
}
