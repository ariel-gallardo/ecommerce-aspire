using AutoMapper;
using Client.Domain.Entities;
using Common.Infrastructure.Cache;
using Common.Infrastructure.Configurations;
using Common.Infrastructure.Persistence.Seeds.Base;
using Common.Infrastructure.Seeder.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace Client.Infrastructure.Seeders
{
    public class ClientSeeder : Seeder, IDevelopmentSeeder
    {
        public ClientSeeder(IOptions<AppSettings> options, ICacheManagerServices cache, IMapper mapper, IServiceProvider sp) : base(options, cache, mapper, sp)
        {
        }

        public async Task<IEnumerable<object>> SeedAsync(CancellationToken cancellationToken)
        {
            _cache.SetCancellationToken(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if(await Set<Cliente>().AnyAsync(cancellationToken))
            {
                return Array.Empty<object>();
            }
            return Enumerable.Range(1, _quantity).Select(i => new Cliente
            {
                Nombre = $"Nombre {i}",
                Apellido = $"Apellido {i}",
                Cuit = $"00-{i.ToString().PadLeft(8, '0')}-0",
                CreatedAt = DateTime.UtcNow,
                Email = $"email_{i}@mail.com",
                FechaNacimiento = DateTime.ParseExact("01/01/2000", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                RazonSocial = $"Razon Social {i}",
                TelefonoCelular = $"{i.ToString().PadLeft(10, '0')}"
            });
        }
    }
}
