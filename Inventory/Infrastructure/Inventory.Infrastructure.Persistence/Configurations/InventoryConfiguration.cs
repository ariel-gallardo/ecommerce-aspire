using Common.Infrastructure;
using Inventory.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Configurations
{
    public class InventoryConfiguration : ConfigurationBase<InventoryItem>
    {
        public override void Configure(EntityTypeBuilder<InventoryItem> builder)
        {
            ConfigureAuditable(builder);
            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.Unit).IsRequired();
            builder.Navigation(x => x.Quantity).IsRequired(false);
            builder.Navigation(x => x.QuantityAlert).IsRequired(false);
            builder.OwnsOne(x => x.Quantity, x =>
            {
                x.Property(x => x.Unit).IsRequired(true);
                x.Property(x => x.Value).HasPrecision(18, 2).IsRequired(true);
                x.HasIndex(x => x.Unit);
                x.HasIndex(x => x.Value);
            });
            builder.OwnsOne(x => x.QuantityAlert, x =>
            {
                x.Property(x => x.Unit).IsRequired(true);
                x.Property(x => x.Value).HasPrecision(18, 2).IsRequired(true);
                x.HasIndex(x => x.Unit);
                x.HasIndex(x => x.Value);
            });
        }
    }
}
