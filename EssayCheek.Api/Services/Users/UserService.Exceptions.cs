using EFxceptions.Models.Exceptions;
using EssayCheek.Api.Model.Foundation.Users;
using EssayCheek.Api.Model.Foundation.Users.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace EssayCheek.Api.Services.Users;

public partial class UserService
{
    private delegate IQueryable<User> ReturningUsersFunction();
    private delegate ValueTask<User> ReturningUserFunction();
    
    private async ValueTask<User> TryCatch(ReturningUserFunction returningUserFunction){
        try
        {
            return await returningUserFunction();
        }
        catch (UserNullException nullUserException)
        {
            throw CreateAndLogValidationException(nullUserException);
        }
        catch (InvalidUserException invalidUserException)
        {
            throw CreateAndLogValidationException(invalidUserException);
        }
        catch (SqlException sqlException)
        {
            var userStorageException =
                            new FailedUserStorageException(sqlException);
         
            throw CreateAndLogCriticalDependencyException(userStorageException);
        }
        catch (NotFoundUserException notFoundUserException)
        {
            throw CreateAndLogValidationException(notFoundUserException);
        }
        catch (DuplicateKeyException duplicateKeyException)
        {
            var alreadyExistsUserException = new
                            AlreadyExistsUserException(duplicateKeyException);
        
            throw CreateAndLogDependencyValidationException(alreadyExistsUserException);
        }
        catch (Exception exception)
        {
            var failedUserException = new FailedUserServiceException(exception);
            throw CreateAndLogServiceException(failedUserException);
        }
    }

    private IQueryable<User> TryCatch(ReturningUsersFunction returningUsersFunction)
    {
        try
        {
            return returningUsersFunction();
        }
        catch (SqlException sqlException)
        {
            var failedUserStorageException = new FailedUserStorageException(sqlException);
            
            throw CreateAndLogCriticalDependencyException(failedUserStorageException);
        }
        catch (Exception exception)
        {
            var failedUserServiceException = new FailedUserServiceException(exception);
            
            throw CreateAndLogServiceException(failedUserServiceException);
        }
    }

    private Exception CreateAndLogServiceException(Xeption exception)
    {
        var userServiceException = new UserServiceException(exception);
        
        _loggingBroker.LogError(userServiceException);
        return userServiceException;
    }

    private Exception CreateAndLogDependencyValidationException(Xeption exception)
    {
        var userValidationException = 
                        new UserDependencyValidationException(exception);
        
        _loggingBroker.LogError(userValidationException);
        return userValidationException;
    }

    private UserValidationException CreateAndLogValidationException(Xeption exception)
    {
        var userValidationException = 
            new UserValidationException(exception);
        
        _loggingBroker.LogError(userValidationException);
        return userValidationException;
    }
    private UserDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
    {
        var userDependencyException = 
                new UserDependencyException(exception);
        
        _loggingBroker.LogCritical(userDependencyException);
        return userDependencyException;
    }
    
}

