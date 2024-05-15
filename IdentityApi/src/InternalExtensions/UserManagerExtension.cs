using IdentityApi.src.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityApi.src.InternalExtensions;

public static class UserManagerExtension
{
    public static async Task<User?> FindUserByCardId(this UserManager<User> userManager, int cardId) => await userManager.Users.Where(x => x.CardId == cardId).SingleOrDefaultAsync();
}
