using Common.Domain.Exceptions;
using Common.Infrastructure.Configurations;
using Common.Infrastructure.Entities.Const;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Security.Infrastructure.Contracts;
using Security.Infrastructure.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace Security.Infrastructure
{
    public class AuthServices : IAuthServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppSettings _appSettings;
        private readonly IAuthorizationService _authorizationService;
        private ClaimsPrincipal _user;
        public ClaimsPrincipal User { get => _httpContextAccessor?.HttpContext?.User ?? _user; private set { _user = value; } }

        public AuthServices(IHttpContextAccessor httpContext, IOptions<AppSettings> appSettings, IAuthorizationService authorizationService)
        {
            _httpContextAccessor = httpContext;
            _appSettings = appSettings.Value;
            _authorizationService = authorizationService;
        }

        public bool IsAuthenticated
        {
            get => User?.Identity?.IsAuthenticated ?? false;
        }

        public async Task AuthAsAdmin()
        {
            if (!IsAuthenticated)
            {
                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Role, nameof(Role.Administrator)),
                    new Claim(ClaimTypes.NameIdentifier, SecurityConst.InternalAdminId.ToString())
                };
                var identity = new ClaimsIdentity(claims, SecurityConst.AuthenticationInternal);
                var principal = new ClaimsPrincipal(identity);
                if(_httpContextAccessor?.HttpContext != null) _httpContextAccessor.HttpContext.User = principal;
                else User = principal;
            }

        }

        public async Task<bool?> CanAccess(string policyName)
        {
            var user = _httpContextAccessor?.HttpContext?.User;
            if (!user.Identity.IsAuthenticated) return null;
            var result = await _authorizationService.AuthorizeAsync(user, policyName);
            return result.Succeeded;
        }

        public string? ClaimValue(string claimType)
        => User?.FindFirst(claimType)?.Value;

        public long Id
        {
            get
            {
                if (long.TryParse(ClaimValue(ClaimTypes.NameIdentifier), out long result))
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

            return $"Bearer {new JwtSecurityTokenHandler().WriteToken(token)}";
        }

        public string HashPassword(string password)
        => BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());

        public bool VerifyPassword(string password, string hashedPassword)
        => BCrypt.Net.BCrypt.Verify(password, hashedPassword);

    }
}
