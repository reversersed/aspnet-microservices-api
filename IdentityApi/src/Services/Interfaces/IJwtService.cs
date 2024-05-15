using IdentityApi.src.Data.Entities;
using MediatR;
using System.Security.Claims;

namespace IdentityApi.src.Services.Interfaces;

public interface IJwtService
{
    public Task<string> GenerateToken(User user, bool longer_expiration = false);
    public Task<string> GenerateRefreshToken(User user, bool longer_expiration = false);
    public Task RevokeToken(User user);
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
