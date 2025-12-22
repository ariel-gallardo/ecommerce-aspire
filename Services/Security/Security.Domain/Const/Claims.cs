using Common.Infrastructure.Contracts;
using System.Security.Claims;

namespace Security.Domain.Const
{
    public class Claims : IClaim
    {
        public const string Role = ClaimTypes.Role;
        public const string Street = "AddressStreet";
        public const string Number = "AddressNumber";
        public const string Neighborhood = "AddressNeighborhood";
        public const string Description = "AddressDescription";
        public const string Latitude = "AddressLatitude";
        public const string Longitude = "AddressLongitude";
        public const string NameIdentifier = ClaimTypes.NameIdentifier;
        public const string Name = ClaimTypes.Name;
        public const string Email = ClaimTypes.Email;
        public const string GivenName = ClaimTypes.GivenName;
        public const string Surname = ClaimTypes.Surname;
    }
}
