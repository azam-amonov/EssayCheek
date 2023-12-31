using EFxceptions.Models.Exceptions;
using EssayCheek.Api.Model.Foundation.Essays;
using EssayCheek.Api.Model.Foundation.Essays.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace EssayCheek.Api.Services.Essays;

public partial class EssayService
{
    private delegate IQueryable<Essay> ReturningEssaysFunctions();
    private delegate ValueTask<Essay> ReturningEssayFunctions();

    private async ValueTask<Essay> TryCatch(ReturningEssayFunctions returningEssayFunction)
    {
        try
        {
            return await returningEssayFunction();
        }
        catch (EssayNullException essayNullException)
        {
            throw CreateAndLogValidationException(essayNullException);
        }
        catch (InvalidEssayException invalidEssayException)
        {
            throw CreateAndLogValidationException(invalidEssayException);
        }
        catch (SqlException sqlException)
        {
            var essayStorageException = new FailedEssayStorageException(sqlException);

            throw CreateAndLogCriticalDependencyException(essayStorageException);
        }
        catch (NotFoundEssayException notFoundEssayException)
        {
            throw CreateAndLogValidationException(notFoundEssayException);
        }
        catch (DuplicateKeyException duplicateKeyException)
        {
            var alreadyExistsEssayException = new AlreadyExistsEssayException(duplicateKeyException);
            throw CreateAndLogDependencyValidationException(alreadyExistsEssayException);
        }
        catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
        {
            var lockedEssayException = new LockedEssayException(dbUpdateConcurrencyException);
            throw CreateAndLogDependencyValidationException(lockedEssayException);
        }
        catch (Exception exception)
        {
            var failedEssayEssayException = new FailedEssayServiceException(exception);
            throw CreateAndLogServiceException(failedEssayEssayException);
        }
    }

    private IQueryable<Essay> TryCatch(ReturningEssaysFunctions returningEssaysFunctions)
    {
        try
        {
            return returningEssaysFunctions();
        }
        catch (SqlException sqlException)
        {
            var failedEssayStorageException = new FailedEssayStorageException(sqlException);
            throw CreateAndLogCriticalDependencyException(failedEssayStorageException);
        }
        catch (Exception exception)
        {
            var failedEssayServiceException = new FailedEssayServiceException(exception);
            throw CreateAndLogServiceException(failedEssayServiceException);
        }
    }

    private Exception CreateAndLogDependencyValidationException(Xeption exception)
    {
        var essayDependencyValidationException = new EssayDependencyValidationException(exception);
        _loggingBroker.LogError(essayDependencyValidationException);

        return essayDependencyValidationException;
    }

    private EssayDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
    {
        var essayDependencyException = new EssayDependencyException(exception);
        _loggingBroker.LogCritical(essayDependencyException);
        
        return essayDependencyException;
    }

    private Exception CreateAndLogServiceException(Xeption exception)
    {
        var essayServiceException = new EssayServiceException(exception);
        _loggingBroker.LogError(essayServiceException);

        return essayServiceException;
    }

    private EssayValidationException CreateAndLogValidationException(Xeption exception)
    {
        var essayValidationException = new EssayValidationException(exception);
        _loggingBroker.LogError(essayValidationException);

        return essayValidationException;
    }
}