using EssayCheek.Api.Model.Foundation.EssayResults;
using EssayCheek.Api.Model.Foundation.EssayResults.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace EssayCheek.Api.Services.EssayResults;

public partial class EssayResultService
{
    private delegate IQueryable<EssayResult> ReturningEssayResultsFunctions();
    private delegate ValueTask<EssayResult> ReturningResultEssayFunctions();

    private async ValueTask<EssayResult> TryCatch(ReturningResultEssayFunctions returningEssayFunction)
    {
        try
        {
            return await returningEssayFunction();
        }
        catch (EssayResultNullException essayResultNullException)
        {
            throw CreateAndLogValidationException(essayResultNullException);
        }
        catch (InvalidEssayResultException invalidEssayResultException)
        {
            throw CreateAndLogValidationException(invalidEssayResultException);
        }
        catch (SqlException exception)
        {
            var essayResultStorageException = new FailedEssayResultStorageException(exception);
            throw CreateAndLogCriticalDependencyException(essayResultStorageException);
        }
        catch (Exception exception)
        {
            var failedEssayResultException = new FailedEssayResultServiceException(exception);
            throw CreateAndLogServiceException(failedEssayResultException);
        }
    }

    private EssayResultDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
    {
        var essayResultDependencyException = new EssayResultDependencyException(exception);
        _loggingBroker.LogCritical(essayResultDependencyException);

        return essayResultDependencyException;
    }

    private Exception CreateAndLogServiceException(Xeption exception)
    {
        var essayResultServiceException = new EssayResultServiceException(exception);
        _loggingBroker.LogError(essayResultServiceException);

        return essayResultServiceException;
    }

    private EssayResultValidationException CreateAndLogValidationException(Xeption exception)
    {
        var essayValidationException = new EssayResultValidationException(exception);
        _loggingBroker.LogError(essayValidationException);

        return essayValidationException;
    }
}