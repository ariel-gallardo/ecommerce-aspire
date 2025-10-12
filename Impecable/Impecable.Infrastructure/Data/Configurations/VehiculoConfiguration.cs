using Common.Infrastructure;
using Impecable.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Impecable.Infrastructure.Data.Configurations
{
    public class VehiculoConfiguration : ConfigurationBase<Vehiculo>
    {
        public override void Configure(EntityTypeBuilder<Vehiculo> builder)
        {
            ConfigureAuditable(builder);
            builder.HasOne(x => x.Cliente).WithMany().HasForeignKey(x => x.ClienteId);
            builder.Property(x => x.ClienteId).IsRequired();
            builder.Property(x => x.Dominio).IsRequired(true);
            builder.HasIndex(x => x.Dominio).IsUnique(true);
        }
    }
}
