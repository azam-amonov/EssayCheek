using EssayCheek.Api.Model.Foundation.Users;
using EssayCheek.Api.Model.Foundation.Users.Exceptions;

namespace EssayCheek.Api.Services.Users;

public partial class UserService
{
    private void ValidateUserNotNull(User user)
    {
        if (user is null)
            throw new NullUserException();
    }
}