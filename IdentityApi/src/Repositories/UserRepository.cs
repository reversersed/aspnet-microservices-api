using IdentityApi.src.Data;
using IdentityApi.src.Data.Entities;
using IdentityApi.src.Repositories.Interfaces;

namespace IdentityApi.src.Repositories;

public class UserRepository(DataContext _context) : IUserRepository
{
    private readonly DataContext context = _context ?? throw new ArgumentNullException(nameof(context));

    public async Task RevokeToken(User user)
    {
        user.RefreshToken = null;
        user.RefreshExpirationDate = null;

        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    public async Task UpdateRefreshToken(User user, string token, bool longer_expiration = false)
    {
        user.RefreshToken = token;
        user.RefreshExpirationDate = DateTime.UtcNow.AddDays(longer_expiration ? 62 : 7);

        context.Users.Update(user);
        await context.SaveChangesAsync();
    }
}
