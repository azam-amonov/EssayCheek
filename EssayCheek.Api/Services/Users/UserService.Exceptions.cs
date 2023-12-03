using EssayCheek.Api.Model.Foundation.Users;
using EssayCheek.Api.Model.Foundation.Users.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

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
            throw CreateAndLogValidationException(nullUserException);
        }
        catch (InvalidUserException invalidUserException)
        {
            throw CreateAndLogValidationException(invalidUserException);
        }
        catch (SqlException sqlException)
        {
            var userStorageException = new FailedUserStorageException(sqlException);
            throw CreateAndLogCriticalDependencyException(userStorageException);
        }
    }

    private UserValidationException CreateAndLogValidationException(Xeption exception)
    {
        var userValidationException = new UserValidationException(exception);
        
        _loggingBroker.LogError(userValidationException);
        return userValidationException;
    }
    
    private UserDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
    {
        var userDependencyException = new UserDependencyException(exception);
        
        _loggingBroker.LogCritical(userDependencyException);
        return userDependencyException;
    }
}

