using EssayCheek.Api.Model.Essays;
using EssayCheek.Api.Model.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EssayCheek.Api.Brokers.StorageBroker;

public sealed partial class StorageBroker
{
    public void ConfigureUser(EntityTypeBuilder<User> builder)
    {
        builder.HasMany<Essay>(user => user.Essays)
                        .WithOne(essays => essays.User)
                        .HasForeignKey(essay => essay.UserId);
    }
}