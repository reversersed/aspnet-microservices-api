using Extensions;
using IdentityApi.src.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityApi.src.Data.Seeds;

public class AdminCreationSeed
{
    public static async Task SeedAsync(SignInManager<User> userManager, RoleManager<IdentityRole<long>> roleManager, ILogger logger)
    {
        var roles = new string[] { ClaimRoles.User, ClaimRoles.Admin };
        foreach (var item in roles)
        {
            if(!await roleManager.RoleExistsAsync(item))
                await roleManager.CreateAsync(new IdentityRole<long> { Name = item });
        }

        var users = new List<User>
        {
            new()
            {
                UserName = "admin"
            }
        };
        foreach(var user in users)
        {
            if (await userManager.UserManager.FindByNameAsync(user.UserName ?? "admin") == null)
            {
                var result = await userManager.UserManager.CreateAsync(user, user.UserName ?? "admin");

                if (result.Succeeded)
                {
                    if (user.UserName != null && user.UserName.Equals("admin"))
                        await userManager.UserManager.AddToRoleAsync(user, ClaimRoles.Admin);
                    await userManager.UserManager.AddToRoleAsync(user, ClaimRoles.User);
                    logger.LogInformation("{username} has been created", user.UserName);
                }
                else
                { 
                    logger.LogError("Errror while creating user {username}:", user.UserName);
                    result.Errors.ToList().ForEach(e => logger.LogError("{Code}: {Description}", e.Code, e.Description));
                }
            }
        }
        if(await userManager.UserManager.FindByNameAsync(ClaimRoles.Business) == null)
        {
            var user = new User { UserName = ClaimRoles.Business };
            var result = await userManager.UserManager.CreateAsync(user, user.UserName);

            if (result.Succeeded)
            {
                await userManager.UserManager.AddToRoleAsync(user, ClaimRoles.User);
                await userManager.UserManager.AddToRoleAsync(user, ClaimRoles.Business);
                logger.LogInformation("{username} has been created", user.UserName);
            }
            else
            {
                logger.LogError("Errror while creating user {username}:", user.UserName);
                result.Errors.ToList().ForEach(e => logger.LogError("{Code}: {Description}", e.Code, e.Description));
            }
        }
        var businessUsers = new User[]
        {
            new() { UserName = "Neurosoft" },
            new() { UserName = "Involta" },
            new() { UserName = "Tele2" }
        };
        for(int i =  0; i < businessUsers.Length; i++)
        {
            if (await userManager.UserManager.FindByNameAsync(businessUsers[i].UserName) == null)
            {
                var result = await userManager.UserManager.CreateAsync(businessUsers[i], businessUsers[i].UserName);

                if (result.Succeeded)
                {
                    await userManager.UserManager.AddToRoleAsync(businessUsers[i], ClaimRoles.User);
                    await userManager.UserManager.AddToRoleAsync(businessUsers[i], ClaimRoles.Business);
                    logger.LogInformation("{username} has been created", businessUsers[i].UserName);
                }
                else
                {
                    logger.LogError("Errror while creating user {username}:", businessUsers[i].UserName);
                    result.Errors.ToList().ForEach(e => logger.LogError("{Code}: {Description}", e.Code, e.Description));
                }
            }
        }
    }
}
