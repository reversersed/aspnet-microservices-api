using IdentityApi.src.Data;
using IdentityApi.src.Data.Entities;
using IdentityApi.src.Repositories.Interfaces;
using IdentityApi.src.Services.Interfaces;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ResponsePackage;

namespace IdentityApi.src.Services;

public class UserService(SignInManager<Data.Entities.User> _signInManager) : IUserService
{
    private readonly SignInManager<Data.Entities.User> signInManager = _signInManager ?? throw new ArgumentNullException(nameof(_signInManager));

    public async Task<UserResponse> GetUserById(long Id)
    {
        var user = await signInManager.UserManager.FindByIdAsync(Id.ToString()) ?? throw new CustomExceptionResponse(ResponseCodes.UserNotFound, "Пользователь не найден");
        var response_user = user.Adapt<UserResponse>();
        response_user.Roles = await signInManager.UserManager.GetRolesAsync(user);
        return response_user;
    }

    public async Task<UserResponse> GetUserByUsername(string Name)
    {
        var user = await signInManager.UserManager.FindByNameAsync(Name) ?? throw new CustomExceptionResponse(ResponseCodes.UserNotFound, "Пользователь не найден");
        var response_user = user.Adapt<UserResponse>();
        response_user.Roles = await signInManager.UserManager.GetRolesAsync(user);
        return response_user;
    }

    public async Task<User> UpdateUsername(string username, string new_username)
    {
        var user_taken = await signInManager.UserManager.FindByNameAsync(new_username);
        if (user_taken is not null)
            throw new CustomExceptionResponse(ResponseCodes.NotUnique, $"Имя пользователя {new_username} уже занято");
        var user = await signInManager.UserManager.FindByNameAsync(username) ?? throw new CustomExceptionResponse(ResponseCodes.UserNotFound, "Пользователь не авторизован");
        if (user.LastNameChange != null && (DateTime.UtcNow - user.LastNameChange).Value.TotalDays < 31)
            throw new CustomExceptionResponse(ResponseCodes.Restricted, "Менять имя пользователя можно раз в месяц");

        user.LastNameChange = DateTime.UtcNow;
        user.UserName = new_username;
        var result = await signInManager.UserManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new CustomExceptionResponse(ResponseCodes.UndefinedServerException, "Во время обновления произошла ошибка", result.Errors.Select(i => i.Description).ToList());
        return user;
    }
}
