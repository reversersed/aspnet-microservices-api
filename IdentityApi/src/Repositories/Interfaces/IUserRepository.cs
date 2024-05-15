using IdentityApi.src.Data.Entities;

namespace IdentityApi.src.Repositories.Interfaces;

public interface IUserRepository
{
    public Task RevokeToken(User user);
    public Task UpdateRefreshToken(User user, string token, bool longer_expiration = false);
}
