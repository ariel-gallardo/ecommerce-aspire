using Client.Domain.Entities;
using Common.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Client.Infrastructure.Persistence.Configurations
{
    public class ClienteConfiguration : ConfigurationBase<Cliente>
    {
        public override void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("clientes");
            ConfigureAuditable(builder);
            builder.Property(x => x.Nombre);
            builder.Property(x => x.Apellido);
            builder.Property(x => x.RazonSocial);
            builder.Property(x => x.Cuit);
            builder.Property(x => x.FechaNacimiento).HasColumnType("DATE");
            builder.Property(x => x.TelefonoCelular);   
            builder.Property(x => x.Email);
            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Cuit).IsUnique();
        }
    }
}
