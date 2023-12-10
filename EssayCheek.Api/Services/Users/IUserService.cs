using EssayCheek.Api.Model.Foundation.Users;

namespace EssayCheek.Api.Services.Users;

public interface IUserService
{
    ValueTask<User> AddUserAsync(User user);
    ValueTask<User?> RetrieveUserByIdAsync(Guid id);
    IQueryable<User> RetrieveAllUsers();
    ValueTask<User> ModifyUserAsync(User user);
    ValueTask<User> RemoveUserByIdAsync(Guid id);   
}