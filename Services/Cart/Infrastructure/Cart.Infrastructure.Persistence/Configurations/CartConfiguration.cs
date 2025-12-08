using Common.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CartEntity = Cart.Domain.Entities.Cart;
namespace Cart.Infrastructure.Persistence.Configurations
{
    public class CartConfiguration : ConfigurationBase<CartEntity>
    {
        public override void Configure(EntityTypeBuilder<CartEntity> builder)
        {
            ConfigureAuditableGuid(builder);
            builder.HasMany(x => x.Items).WithOne(x => x.Cart).HasForeignKey(x => x.CartId);
        }
    }
}
