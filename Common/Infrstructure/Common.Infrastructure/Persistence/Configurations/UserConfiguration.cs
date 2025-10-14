using Common.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : ConfigurationBase<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            ConfigureIdentifiable(builder);

            builder.Property(x => x.Username).IsRequired(true);
            builder.Property(x => x.Email).IsRequired(true);
            builder.Property(x => x.Password).IsRequired(true);

            builder.HasIndex(x => x.Username).IsUnique();
            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Password);

            builder.HasOne(x => x.Persona).WithOne().IsRequired(false).HasForeignKey<User>(x => x.PersonaId);
        }
    }
}
