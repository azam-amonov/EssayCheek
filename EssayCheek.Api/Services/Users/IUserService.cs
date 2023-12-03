using EssayCheek.Api.Model.Foundation.Users;

namespace EssayCheek.Api.Services.Users;

public interface IUserService
{
    ValueTask<User> AddUserAsync(User user);
    ValueTask<User?> GetUserByIdAsync(Guid id);
    IQueryable<User> GetAllUsersAsync();
    ValueTask<User> ModifyUserAsync(User user);
    ValueTask<User> RemoveUserAsync(User id);   
}