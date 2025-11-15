using Common.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Security.Domain.Entities;

namespace Security.Infrastructure.Persistence.Configurations
{
    public class PermissionConfiguration : ConfigurationBase<Permission>
    {
        public override void Configure(EntityTypeBuilder<Permission> builder)
        {
            ConfigureAuditable(builder);
            builder.Property(x => x.Controller).IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.Action).IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.Url).IsRequired(false).HasMaxLength(75);
            builder.Property(x => x.Policy).HasMaxLength(50);
            builder.HasIndex(x => x.Controller);
            builder.HasIndex(x => x.Action);
            builder.HasIndex(x => x.Url);
        }
    }
}
