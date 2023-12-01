using EssayCheek.Api.Model.Foundation.Users;

namespace EssayCheek.Api.Brokers.StorageBroker;

public partial interface IStorageBroker
{
    ValueTask<User> InsertUserAsync(User user);
    IQueryable<User> SelectAllUsers();
    ValueTask<User?> SelectUserByIdAsync(Guid id);
    ValueTask<User> UpdateUserAsync(User user);
    ValueTask<User> DeleteUserAsync(User user);
}