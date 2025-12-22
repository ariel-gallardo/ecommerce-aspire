using Cart.Domain.Entities;
using Common.Infrastructure.Entities.Enums;
using Common.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cart.Infrastructure.Persistence.Configurations
{
    public class CartItemConfiguration : ConfigurationBase<CartItem>
    {
        public override void Configure(EntityTypeBuilder<CartItem> builder)
        {
            ConfigureAuditableGuid(builder);
            builder.Property(x => x.ProductId).IsRequired();
            builder.OwnsOne(x => x.Quantity, x =>
            {
                x.Property(x => x.Unit).HasConversion(new EnumToStringConverter<Unit>());
                x.Property(x => x.Value).IsRequired(true).HasPrecision(18, 2);
                x.HasIndex(x => x.Unit);
                x.HasIndex(x => x.Value);
            });

            builder.HasIndex(x => x.ProductId);
        }
    }
}
