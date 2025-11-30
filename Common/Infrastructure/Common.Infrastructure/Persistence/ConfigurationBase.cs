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
            builder.Property(x => x.CreatedAt).HasColumnName("fecha_creacion").IsRequired(true);
            builder.Property(x => x.UpdatedAt).HasColumnName("fecha_modificacion").IsRequired(false);
            builder.Property(x => x.DeletedAt).HasColumnName("fecha_eliminacion").IsRequired(false);
            builder.HasOne(x => x.CreatedBy).WithMany().HasForeignKey(x => x.CreatedById);
            builder.HasOne(x => x.UpdatedBy).WithMany().HasForeignKey(x => x.UpdatedById).IsRequired(false);
            builder.HasOne(x => x.DeletedBy).WithMany().HasForeignKey(x => x.DeletedById).IsRequired(false);
            builder.HasQueryFilter(x => x.DeletedAt == null);
        }

        protected void ConfigureAuditableGuid<U>(EntityTypeBuilder<U> builder) where U : class, IAuditableGuid
        {
            ConfigureIdentifiableGuid(builder);
            builder.Property(x => x.CreatedAt).HasColumnName("fecha_creacion").IsRequired(true);
            builder.Property(x => x.UpdatedAt).HasColumnName("fecha_modificacion").IsRequired(false);
            builder.Property(x => x.DeletedAt).HasColumnName("fecha_eliminacion").IsRequired(false);
            builder.HasOne(x => x.CreatedBy).WithMany().HasForeignKey(x => x.CreatedById);
            builder.HasOne(x => x.UpdatedBy).WithMany().HasForeignKey(x => x.UpdatedById).IsRequired(false);
            builder.HasOne(x => x.DeletedBy).WithMany().HasForeignKey(x => x.DeletedById).IsRequired(false);
            builder.HasQueryFilter(x => x.DeletedAt == null);
        }
    }
}
