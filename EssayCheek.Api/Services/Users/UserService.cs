using EssayCheek.Api.Brokers.StorageBroker;
using EssayCheek.Api.Model.Users;

namespace EssayCheek.Api.Services.Users;

public class UserService : IUserService
{
    private readonly IStorageBroker _storageBroker;

    public UserService(IStorageBroker storageBroker)
    {
        _storageBroker = storageBroker;
    }

    public ValueTask<User> AddUserAsync(User user) => throw new NotImplementedException();

    public async ValueTask<User?> GetUserByIdAsync(Guid id) => await _storageBroker.SelectUserByIdAsync(id);

    public IQueryable<User> GetAllUsersAsync() =>  _storageBroker.SelectAllUsers();

    public async ValueTask<User> ModifyUserAsync(User user) => await _storageBroker.UpdateUserAsync(user);

    public async ValueTask<User> RemoveUserAsync(User id) => await _storageBroker.DeleteUserAsync(id);
}