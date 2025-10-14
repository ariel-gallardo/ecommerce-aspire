using Common.Domain.Entities;
using Common.Domain.ValueObjects;
using Common.Infrastructure.Configurations;
using Common.Infrastructure.Persistence.Seeds.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Common.Infrastructure.Persistence.Seeds.Entities
{
    public class PersonaSeeder : IDevelopmentSeeder
    {
        private readonly Random _random;
        private readonly int _quantity;

        public List<Persona> People { get; private set; }

        public PersonaSeeder(IOptions<AppSettings> options)
        {
            _random = new Random();
            _quantity = options.Value.Development.QuantityToGenerate;
            People = new List<Persona>();
        }

        public async Task SeedAsync(DbContext context)
        {
            People = Enumerable.Range(1, _quantity).Select(x =>
            {
                return new Persona
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified),
                    Name = $"Persona Name {x}",
                    Lastname = $"Persona LastName {x}",
                    Address = new Address
                    {
                        Coordinates = new Coordinates
                        {
                            Latitude = _random.NextDouble() * 180 - 90,
                            Longitude = _random.NextDouble() * 360 - 180
                        },
                        Description = $"Address Description {x}",
                        Neighborhood = $"Address Neighborhood {x}",
                        Number = _random.Next(1, 100),
                        Street = $"Address Street {x}"
                    }
                };
            }).ToList();

            for (int i = 0; i < People.Count; i++)
            {
                People[i].CreatedById = People[_random.Next(0, Math.Min(2, People.Count))].Id;
                People[i].UpdatedById = i % 3 == 0 ? People[_random.Next(0, Math.Min(2, People.Count))].Id : null;
                People[i].DeletedById = i % 7 == 0 ? People[_random.Next(0, Math.Min(2, People.Count))].Id : null;
            }

            context.AddRange(People);
        }
    }

}
