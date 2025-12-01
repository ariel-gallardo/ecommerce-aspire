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
        }
        protected void ConfigureIdentifiableGuid<U>(EntityTypeBuilder<U> builder) where U : class, IIdentifiableGuid
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd().HasValueGeneratorFactory<GuidValueGeneratorFactory>();
        }

        protected void ConfigureAuditable<U>(EntityTypeBuilder<U> builder) where U : class, IAuditable
        {
            ConfigureIdentifiable(builder);
            builder.Property(x => x.CreatedAt).IsRequired(true);
            builder.Property(x => x.UpdatedAt).IsRequired(false);
            builder.Property(x => x.DeletedAt).IsRequired(false);
            builder.Property(x => x.CreatedById);
            builder.Property(x => x.UpdatedById).IsRequired(false);
            builder.Property(x => x.DeletedById).IsRequired(false);

            builder.HasIndex(x => x.CreatedAt);
            builder.HasIndex(x => x.UpdatedAt);
            builder.HasIndex(x => x.DeletedAt);

            builder.HasIndex(x => x.CreatedById);
            builder.HasIndex(x => x.UpdatedById);
            builder.HasIndex(x => x.DeletedById);
            builder.HasQueryFilter(x => x.DeletedAt == null && x.DeletedById == null);
        }

        protected void ConfigureAuditableGuid<U>(EntityTypeBuilder<U> builder) where U : class, IAuditableGuid
        {
            ConfigureIdentifiableGuid(builder);
            builder.Property(x => x.CreatedAt).IsRequired(true);
            builder.Property(x => x.UpdatedAt).IsRequired(false);
            builder.Property(x => x.DeletedAt).IsRequired(false);
            builder.Property(x => x.CreatedById);
            builder.Property(x => x.UpdatedById).IsRequired(false);
            builder.Property(x => x.DeletedById).IsRequired(false);

            builder.HasIndex(x => x.CreatedAt);
            builder.HasIndex(x => x.UpdatedAt);
            builder.HasIndex(x => x.DeletedAt);

            builder.HasIndex(x => x.CreatedById);
            builder.HasIndex(x => x.UpdatedById);
            builder.HasIndex(x => x.DeletedById);
            builder.HasQueryFilter(x => x.DeletedAt == null && x.DeletedById == null);
        }
    }
}
