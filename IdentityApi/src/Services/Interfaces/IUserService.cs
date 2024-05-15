using IdentityApi.src.Data.Entities;
using ResponsePackage;

namespace IdentityApi.src.Services.Interfaces;

public interface IUserService
{
    public Task<UserResponse> GetUserByUsername(string Name);
    public Task<UserResponse> GetUserById(long Id);
    public Task<User> UpdateUsername(string username, string new_username);
}
