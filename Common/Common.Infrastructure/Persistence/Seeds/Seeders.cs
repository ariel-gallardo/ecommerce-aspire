using Common.Domain.Contracts.Services;
using Common.Domain.Entities;
using Common.Domain.ValueObjects;
using Common.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Common.Infrastructure.Persistence.Seeds
{
    public static class Seeders
    {
        private static Random _random;
        private static int _quantity;

        #region User
        private static List<User> _users = new List<User>();

        private static void GenerateUsers(IAuthServices userServices)
        {
            _users.AddRange(Enumerable.Range(1, _quantity).Select(x =>
            {
                var guid = Guid.NewGuid();
                return new User
                {
                    Id = Guid.NewGuid(),
                    Email = $"user_email_{x}@mail.com",
                    Username = $"user_name_{x}",
                    Password = userServices.HashPassword("123456aA$"),
                };
            }));
        }

        private static void AssociatePeopletoUsers()
        {
            foreach (var i in Enumerable.Range(0, _quantity)) _users[i].PersonaId = i % 4 == 0 ? _people.ElementAt(i).Id : null;
        }

        #endregion

        #region People
        private static IEnumerable<Persona> _people;
        private static void GeneratePeopleWithAddressAndCoordinates()
        {
            var peopleList = Enumerable.Range(1, _quantity).Select(x => new Persona
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified),
                Name = $"Persona Name {x}",
                Lastname = $"Persona Name {x}",
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
            }).ToList();

            for (int i = 0; i < peopleList.Count; i++)
            {
                var person = peopleList[i];
                person.CreatedById = peopleList[_random.Next(0, Math.Min(2, peopleList.Count))].Id;
                person.UpdatedById = i % 3 == 0 ? peopleList[_random.Next(0, Math.Min(2, peopleList.Count))].Id : null;
                person.DeletedById = i % 7 == 0 ? peopleList[_random.Next(0, Math.Min(2, peopleList.Count))].Id : null;
            }

            _people = peopleList;
        }

        #endregion

        private static void Generators(DbContext context, IAuthServices userServices)
        {
            GenerateUsers(userServices);
            GeneratePeopleWithAddressAndCoordinates();
            AssociatePeopletoUsers();

            context.AddRange(_people);
            context.AddRange(_users);
        }

        public static void Seed(DbContext context, IAuthServices userService, IOptions<AppSettings> appSettings)
        {
            _random = new Random();
            _quantity = appSettings.Value.Development.QuantityToGenerate;
            Generators(context,userService);
            context.SaveChanges();
        }
    }
}
