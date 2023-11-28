using EssayCheek.Api.Model.Users;
using Microsoft.EntityFrameworkCore;

namespace EssayCheek.Api.Brokers.StorageBroker;

public partial class StorageBroker : IStorageBroker
{
    private DbSet<User> Users => Set<User>();


    public IQueryable<User> SelectAllUsers() => SelectAll<User>();
    
    public async ValueTask<User> InsertUserAsync(User user) => await InsertAsync<User>(user);

    public async ValueTask<User?> SelectUserByIdAsync(Guid id) => await SelectAsync<User>(id);

    public async ValueTask<User> UpdateUserAsync(User user) => await UpdateAsync<User>(user);

    public async ValueTask<User> DeleteUserAsync(User user) => await DeleteAsync<User>(user);

}