using Common.Contracts;
using System.Security.Claims;

namespace Security.Infrastructure.Contracts
{
    public interface IAuthServices : IScoped
    {
        Guid Id { get; }
        bool IsAuthenticated { get; }
        Task<bool?> CanAccess(string policyName);
        string? ClaimValue(string claimType);
        string GenerateToken(Claim[] claims);

        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
