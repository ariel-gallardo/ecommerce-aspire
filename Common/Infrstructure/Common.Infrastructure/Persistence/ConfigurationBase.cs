using Common.Contracts.Entities;
using Common.Domain.Contracts.Entities;
using Common.Domain.Entities.Base;
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
            builder.Property(x => x.Id).HasColumnType("SERIAL").ValueGeneratedOnAdd();
        }

        protected void ConfigureAuditable<U>(EntityTypeBuilder<U> builder) where U : class, IAuditable
        {
            ConfigureIdentifiable(builder);
            builder.Property(x => x.CreatedAt).HasColumnName("fecha_creacion").IsRequired(true);
            builder.Property(x => x.UpdatedAt).HasColumnName("fecha_modificacion").IsRequired(true);
            builder.Property(x => x.DeletedAt).HasColumnName("fecha_eliminacion").IsRequired(false);
        }
    }
}
