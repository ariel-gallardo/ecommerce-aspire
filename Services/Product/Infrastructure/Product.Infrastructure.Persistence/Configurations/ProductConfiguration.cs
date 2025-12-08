using Common.Domain.Enums;
using Common.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Product.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : ConfigurationBase<Domain.Entities.Product>
    {
        public override void Configure(EntityTypeBuilder<Domain.Entities.Product> builder)
        {
            ConfigureAuditableGuid(builder);
            builder.Property(x => x.Name).IsRequired(true);
            builder.HasIndex(x => x.Name).IsUnique();
            builder.Property(x => x.Description).IsRequired(false);
            builder.HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.OwnsOne(x => x.Price, x =>
            {
                x.Property(y => y.Unit).HasConversion(new EnumToStringConverter<Unit>()).IsRequired(true);
                x.Property(y => y.Value).IsRequired(true).HasPrecision(18, 2);
            });
            builder.Navigation(x => x.Price).IsRequired(false);
        }
    }
}
