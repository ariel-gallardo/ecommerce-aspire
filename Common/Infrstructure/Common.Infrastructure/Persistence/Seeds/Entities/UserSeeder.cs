using Common.Domain.Contracts.Services;
using Common.Domain.Entities;
using Common.Infrastructure.Configurations;
using Common.Infrastructure.Persistence.Seeds.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Common.Infrastructure.Persistence.Seeds.Entities
{
    public class UserSeeder : IDevelopmentSeeder
    {
        private readonly IAuthServices _authServices;
        private readonly int _quantity;
        private readonly PersonaSeeder _personaSeeder;

        public List<User> Users { get; private set; }

        public UserSeeder(IAuthServices authServices, IOptions<AppSettings> options, PersonaSeeder personaSeeder)
        {
            _authServices = authServices;
            _quantity = options.Value.Development.QuantityToGenerate;
            _personaSeeder = personaSeeder;
            Users = new List<User>();
        }

        public Task SeedAsync(DbContext context)
        {
            Users = Enumerable.Range(1, _quantity).Select(x =>
            {
                return new User
                {
                    Id = Guid.NewGuid(),
                    Email = $"user_email_{x}@mail.com",
                    Username = $"user_name_{x}",
                    Password = _authServices.HashPassword("123456aA$")
                };
            }).ToList();

            // Asociar personas
            for (int i = 0; i < _quantity; i++)
            {
                Users[i].PersonaId = i % 4 == 0 ? _personaSeeder.People.ElementAt(i).Id : null;
            }

            context.AddRange(Users);
            return Task.CompletedTask;
        }
    }

}
