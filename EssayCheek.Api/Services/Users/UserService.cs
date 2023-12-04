using EssayCheek.Api.Brokers.DateTimes;
using EssayCheek.Api.Brokers.Logging;
using EssayCheek.Api.Brokers.StorageBroker;
using EssayCheek.Api.Model.Foundation.Users;

namespace EssayCheek.Api.Services.Users;

public partial class UserService : IUserService
{
    private readonly IStorageBroker _storageBroker;
    private readonly IDateTimeBroker _dateTimeBroker;
    private readonly ILoggingBroker _loggingBroker;

    public UserService(IStorageBroker storageBroker, IDateTimeBroker dateTimeBroker, ILoggingBroker loggingBroker)
    {
        _storageBroker = storageBroker;
        _dateTimeBroker = dateTimeBroker;
        _loggingBroker = loggingBroker;
    }

    public ValueTask<User> AddUserAsync(User user) => 
    TryCatch(async () =>
    {
        ValidateUser(user);
        return await _storageBroker.InsertUserAsync(user);
    });


    public ValueTask<User> RetrieveUserByIdAsync(Guid userId) =>
    TryCatch(async () =>
    {
        ValidateUserId(userId);
        User? maybeUser = await _storageBroker.SelectUserByIdAsync(userId);
        
        ValidateStorageUser(maybeUser!, userId);
        
        return maybeUser!;
    });

    public IQueryable<User> RetrieveAllUsers() => 
                TryCatch(() => _storageBroker.SelectAllUsers());

    public async ValueTask<User> ModifyUserAsync(User user) => await _storageBroker.UpdateUserAsync(user);

    public async ValueTask<User> RemoveUserAsync(User id) => await _storageBroker.DeleteUserAsync(id);
}