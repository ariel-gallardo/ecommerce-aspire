using Cart.Domain.Entities;
using Common.Domain.Enums;
using Common.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cart.Infrastructure.Persistence.Configurations
{
    public class CartItemConfiguration : ConfigurationBase<CartItem>
    {
        public override void Configure(EntityTypeBuilder<CartItem> builder)
        {
            ConfigureAuditable(builder);
            builder.Property(x => x.ProductId).IsRequired();
            builder.Navigation(x => x.Quantity).IsRequired();
            builder.OwnsOne(x => x.Quantity, x =>
            {
                x.Property(x => x.Unit).HasConversion(new EnumToStringConverter<Unit>()).IsRequired(true);
                x.Property(x => x.Value).IsRequired(true).HasPrecision(18, 2);
                x.HasIndex(x => x.Unit);
                x.HasIndex(x => x.Value);
            });

            builder.HasIndex(x => x.ProductId);
        }
    }
}
