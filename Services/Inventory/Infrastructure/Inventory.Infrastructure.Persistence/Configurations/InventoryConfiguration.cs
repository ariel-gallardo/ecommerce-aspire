using Common.Infrastructure.Entities.Enums;
using Common.Infrastructure.Persistence;
using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Inventory.Infrastructure.Persistence.Configurations
{
    public class InventoryConfiguration : ConfigurationBase<InventoryItem>
    {
        public override void Configure(EntityTypeBuilder<InventoryItem> builder)
        {
            ConfigureAuditableGuid(builder);
            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.Unit).IsRequired().HasConversion(new EnumToStringConverter<Unit>()).HasMaxLength(30);
            builder.ComplexProperty(x => x.Quantity, x =>
            {
                x.Property(x => x.Unit).HasConversion(new EnumToStringConverter<Unit>()).HasMaxLength(30);
                x.Property(x => x.Value).HasPrecision(18, 2);
            });
            builder.ComplexProperty(x => x.QuantityAlert, x =>
            {
                x.Property(x => x.Unit).HasConversion(new EnumToStringConverter<Unit>()).HasMaxLength(30);
                x.Property(x => x.Value).HasPrecision(18, 2);
            });
            builder.HasIndex(x => x.Unit);
        }
    }
}
