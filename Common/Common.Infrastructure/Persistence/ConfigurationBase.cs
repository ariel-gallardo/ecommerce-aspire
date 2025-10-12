using Common.Contracts.Entities;
using Common.Domain.Contracts.Entities;
using Common.Domain.Entities.Base;
using Common.Infrastructure.Factory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.Infrastructure
{
    public abstract class ConfigurationBase<T> : IEntityTypeConfiguration<T> where T : EntityBase
    {
        public abstract void Configure(EntityTypeBuilder<T> builder);

        protected void ConfigureIdentifiable<U>(EntityTypeBuilder<U> builder) where U : class, IIdentifiable
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd().HasValueGeneratorFactory<GuidValueGeneratorFactory>();
        }

        protected void ConfigureAuditable<U>(EntityTypeBuilder<U> builder) where U : class, IAuditable
        {
            ConfigureIdentifiable(builder);
            builder.HasOne(x => x.CreatedBy).WithMany().HasForeignKey(x => x.CreatedById).IsRequired(true);
            builder.HasOne(x => x.UpdatedBy).WithMany().HasForeignKey(x => x.UpdatedById).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(x => x.DeletedBy).WithMany().HasForeignKey(x => x.DeletedById).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
