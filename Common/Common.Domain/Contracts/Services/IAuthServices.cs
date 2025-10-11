using System.Security.Claims;

namespace Common.Domain.Contracts.Services
{
    public interface IAuthServices
    {
        Guid Id { get; }
        string? ClaimValue(string claimType);
        string GenerateToken(Claim[] claims);

        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
