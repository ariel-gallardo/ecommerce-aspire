using Common.Domain.Configurations;
using Common.Domain.Contracts.Repositories;
using Common.Domain.Contracts.Services;
using Common.Domain.DTOS.Entities.User;
using Common.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Common.Infrastructure
{
    public class UserServices : IUserServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppSettings _appSettings;
        private readonly IUnitOfWork _unitOfWork;

        private ClaimsPrincipal User => _httpContextAccessor.HttpContext.User;


        public UserServices(IHttpContextAccessor httpContext, IOptions<AppSettings> appSettings, IUnitOfWork unitOfWork)
        {
            _httpContextAccessor = httpContext;
            _appSettings = appSettings.Value;
            _unitOfWork = unitOfWork;
        }

        public string? ClaimValue(string claimType)
        => User?.FindFirst(claimType)?.Value;

        public Guid Id
        {
            get
            {
                if (Guid.TryParse(ClaimValue(ClaimTypes.NameIdentifier), out Guid result))
                    return result;
                throw new PermissionDeniedException();
            }
        }

        public string GenerateToken(Claim[] claims)
        {
            var secretKey = _appSettings.Jwt.Secret;
            var issuer = _appSettings.Jwt.Issuer;
            var audience = _appSettings.Jwt.Audience;
            var expiresInMinutes = _appSettings.Jwt.ExpiresInMinutes;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string HashPassword(string password)
        => BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());

        public bool VerifyPassword(string password, string hashedPassword)
        => BCrypt.Net.BCrypt.Verify(password, hashedPassword);

        public string AuthUser(UserLoginDTO dto)
        {
            _unitOfWork.FirstOrDefaultByIdAsync
        }
    }
}
