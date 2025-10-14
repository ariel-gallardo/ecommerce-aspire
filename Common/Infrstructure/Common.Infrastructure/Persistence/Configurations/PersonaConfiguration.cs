using Common.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.Infrastructure.Persistence.Configurations
{
    public class PersonaConfiguration : ConfigurationBase<Persona>
    {
        public override void Configure(EntityTypeBuilder<Persona> builder)
        {
            ConfigureAuditable(builder);
            builder.Navigation(x => x.Address).IsRequired(false);
            builder.OwnsOne(x => x.Address, x =>
            {
                x.Property(y => y.Street);
                x.Property(y => y.Number);
                x.Property(y => y.Description);
                x.Property(y => y.Street);
                x.OwnsOne(y => y.Coordinates, y =>
                {
                    y.Property(y => y.Latitude);
                    y.Property(y => y.Longitude);
                });
                x.Navigation(y => y.Coordinates).IsRequired(false);
            });
        }
    }
}
