using Common.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Product.Infrastructure.Persistence.Configurations
{
    public class CategoryConfiguration : ConfigurationBase<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            ConfigureAuditableGuid(builder);
            builder.Property(x => x.Name).IsRequired(true);
            builder.HasIndex(x => x.Name).IsUnique();
            builder.Property(x => x.Description).IsRequired(false);
            builder.Property(x => x.ParentId).HasDefaultValue(null);
            builder
                .HasOne(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}
