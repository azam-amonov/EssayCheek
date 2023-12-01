using EssayCheek.Api.Brokers.Logging;
using EssayCheek.Api.Brokers.StorageBroker;
using EssayCheek.Api.Model.Users;

namespace EssayCheek.Api.Services.Users;

public class UserService : IUserService
{
    private readonly IStorageBroker _storageBroker;
    private readonly ILoggingBroker _loggingBroker;

    public UserService(IStorageBroker storageBroker, ILoggingBroker loggingBroker)
    {
        _storageBroker = storageBroker;
        _loggingBroker = loggingBroker;
    }

    public async ValueTask<User> AddUserAsync(User user) => await _storageBroker.InsertUserAsync(user);

    public async ValueTask<User?> GetUserByIdAsync(Guid id) => await _storageBroker.SelectUserByIdAsync(id);

    public IQueryable<User> GetAllUsersAsync() => _storageBroker.SelectAllUsers();

    public async ValueTask<User> ModifyUserAsync(User user) => await _storageBroker.UpdateUserAsync(user);

    public async ValueTask<User> RemoveUserAsync(User id) => await _storageBroker.DeleteUserAsync(id);
}