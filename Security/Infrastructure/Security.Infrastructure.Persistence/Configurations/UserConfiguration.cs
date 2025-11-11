using Common.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Security.Domain.Entities;
using Security.Domain.Enums;

namespace Security.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : ConfigurationBase<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            ConfigureIdentifiable(builder);
            builder.Property(x => x.Rol)
                .HasConversion(new EnumToStringConverter<RoleEnum>())
                .HasMaxLength(30)
                .IsRequired(true);
            builder.Property(x => x.Username).IsRequired(true);
            builder.Property(x => x.Email).IsRequired(true);
            builder.Property(x => x.Password).IsRequired(true);

            builder.HasIndex(x => x.Username).IsUnique();
            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Password);
            builder.Property(x => x.PersonaId).IsRequired(false);
            builder.HasOne(x => x.Persona).WithOne().IsRequired(false).HasForeignKey<User>(x => x.PersonaId);
        }
    }
}
