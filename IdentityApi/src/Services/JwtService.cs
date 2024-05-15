using IdentityApi.src.Data.Entities;
using IdentityApi.src.Repositories.Interfaces;
using IdentityApi.src.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IdentityApi.src.Services;

public class JwtService(IUserRepository _userRepository, SignInManager<User> _signInManager, ILogger<JwtService> _logger) : IJwtService
{
    private readonly IUserRepository userRepository = _userRepository ?? throw new ArgumentNullException(nameof(_userRepository));
    private readonly SignInManager<User> signInManager = _signInManager ?? throw new ArgumentNullException(nameof(_signInManager));
    private readonly ILogger<JwtService> logger = _logger ?? throw new ArgumentNullException(nameof(_logger));

    public async Task<string> GenerateRefreshToken(User user, bool longer_expiration = false)
    {
        var rnd = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(rnd);
        var token = Convert.ToBase64String(rnd);

        await userRepository.UpdateRefreshToken(user, token, longer_expiration);

        return token;
    }

    public async Task<string> GenerateToken(User user, bool longer_expiration = false)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Extensions.JwtExtension.securityKey));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var roles = await signInManager.UserManager.GetRolesAsync(user);

        var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName ?? "")
            };
        roles.ToList().ForEach(i => claims.Add(new Claim("Role", i)));
        user.Scopes?.ToList().ForEach(i => claims.Add(new Claim("scope", i)));

        var tokeOptions = new JwtSecurityToken(
            issuer: "http://localhost:9001",
            claims: claims,
            expires: longer_expiration ? DateTime.UtcNow.AddDays(31) : DateTime.UtcNow.AddHours(24),
            signingCredentials: signinCredentials
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        logger.LogInformation("[Jwt] Created token for user {user} with claims:", user.UserName);
        claims.ForEach(i => logger.LogInformation("[Jwt] Claim {type}: {value}",i.Type, i.Value));
        return tokenString;
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Extensions.JwtExtension.securityKey)),
            ValidateLifetime = false 
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Невалидный токен");
        return principal;
    }

    public async Task RevokeToken(User user) => await userRepository.RevokeToken(user);
}
