using Common.Domain.Entities;
using Common.Extensions;
using Security.Domain.Const;
using System.Security.Claims;

namespace Security.Application.DTO
{
    public sealed class UserClaimsDTO
    {
        private readonly Claim[] _claims;

        public ulong Id { get; }
        public string Rol { get; }
        public string Username { get; }
        public string Email { get; }
        public string? Name { get; }
        public string? Lastname { get; }
        public string? Street { get; }
        public int? Number { get; }
        public string? Neighborhood { get; }
        public string? Description { get; }
        public decimal? Latitude { get; }
        public decimal? Longitude { get; }

        public UserClaimsDTO(User user)
        {
            Id = user.Id;
            Rol = user.Rol.AsStringUsingMemberValue();
            Username = user.Username;
            Email = user.Email;
            Name = user.Persona?.Name;
            Lastname = user.Persona?.Lastname;
            Street = user.Persona?.Address?.Street;
            Number = user.Persona?.Address?.Number;
            Neighborhood = user.Persona?.Address?.Neighborhood;
            Description = user.Persona?.Address?.Description;
            Latitude = user.Persona?.Address?.Coordinates?.Latitude;
            Longitude = user.Persona?.Address?.Coordinates?.Longitude;

            var list = new List<Claim>
            {
                new Claim(Claims.NameIdentifier, Id.ToString()),
                new Claim(Claims.Role, Rol),
                new Claim(Claims.Name, Username),
                new Claim(Claims.Email, Email)
            };

            if (!string.IsNullOrEmpty(Name))
                list.Add(new Claim(Claims.GivenName, Name));

            if (!string.IsNullOrEmpty(Lastname))
                list.Add(new Claim(Claims.Surname, Lastname));

            if (!string.IsNullOrEmpty(Street))
                list.Add(new Claim(Claims.Street, Street));

            if (Number.HasValue)
                list.Add(new Claim(Claims.Number, Number.Value.ToString()));

            if (!string.IsNullOrEmpty(Neighborhood))
                list.Add(new Claim(Claims.Neighborhood, Neighborhood));

            if (!string.IsNullOrEmpty(Description))
                list.Add(new Claim(Claims.Description, Description));

            if (Latitude.HasValue)
                list.Add(new Claim(Claims.Latitude, Latitude.Value.ToString()));

            if (Longitude.HasValue)
                list.Add(new Claim(Claims.Longitude, Longitude.Value.ToString()));

            _claims = list.ToArray();
        }

        public Claim? this[string claimType] => _claims.FirstOrDefault(c => c.Type == claimType);
        public Claim[] All => _claims;
    }
}
