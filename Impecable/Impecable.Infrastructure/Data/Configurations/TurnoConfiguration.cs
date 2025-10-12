using Common.Infrastructure;
using Impecable.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Impecable.Infrastructure.Data.Configurations
{
    public class TurnoConfiguration : ConfigurationBase<Turno>
    {
        public override void Configure(EntityTypeBuilder<Turno> builder)
        {
            ConfigureAuditable(builder);
            builder.HasOne(x => x.Cliente).WithMany().HasForeignKey(x => x.ClienteId).IsRequired(true);
            builder.HasOne(x => x.Vehiculo).WithMany().HasForeignKey(x => x.VehiculoId).IsRequired(true);
        }
    }
}
