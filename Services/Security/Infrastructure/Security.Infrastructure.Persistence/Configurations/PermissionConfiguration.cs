using Common.Infrastructure;
using Common.Infrastructure.Converters;
using Common.Infrastructure.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Security.Domain.Entities;

namespace Security.Infrastructure.Persistence.Configurations
{
    public class PermissionConfiguration : ConfigurationBase<Permission>
    {
        public override void Configure(EntityTypeBuilder<Permission> builder)
        {
            ConfigureAuditableGuid(builder);
            builder.Property(x => x.Controller).IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.Action).IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.Url).IsRequired(false).HasMaxLength(75);
            builder.Property(x => x.Policy)
                .HasConversion(new EnumValueToStringConverter<Policy>())
                .HasMaxLength(50)
                .IsRequired(true);

            builder.HasIndex(x => x.Url)
                   .IsUnique()
                   .HasFilter("url IS NOT NULL");

            builder.HasIndex(x => new { x.Controller, x.Action })
                   .IsUnique()
                   .HasFilter("url IS NULL");

            builder.HasIndex(x => x.Policy);
        }
    }
}
