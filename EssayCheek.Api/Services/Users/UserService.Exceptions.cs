using EssayCheek.Api.Model.Foundation.Users;
using EssayCheek.Api.Model.Foundation.Users.Exceptions;

namespace EssayCheek.Api.Services.Users;

public partial class UserService
{
    private delegate ValueTask<User> ReturningUserFunction();
    
    private async ValueTask<User> TryCatch(ReturningUserFunction returningUserFunction){
        try
        {
            return await returningUserFunction();
        }
        catch (NullUserException nullUserException)
        {
            var userValidationException = new UserValidationException(nullUserException);
            
            _loggingBroker.LogError(userValidationException);
            throw userValidationException;
        }
    }
}