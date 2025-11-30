using Common.Infrastructure;
using Common.Infrastructure.Converters;
using Common.Infrastructure.Entities.Enums;
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
            builder.Property(x => x.Policy)
                .HasConversion(new EnumValueToStringConverter<Policy>())
                .HasMaxLength(50)
                .IsRequired(true);
            builder.HasIndex(x => x.Controller);
            builder.HasIndex(x => x.Action);
            builder.HasIndex(x => x.Url);
            builder.HasIndex(x => x.Policy);
        }
    }
}
