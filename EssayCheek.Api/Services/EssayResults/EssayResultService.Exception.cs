using EssayCheek.Api.Model.Foundation.EssayResults;
using EssayCheek.Api.Model.Foundation.EssayResults.Exceptions;
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
        catch (Exception exception)
        {
            var failedEssayResultException = new FailedEssayResultServiceException(exception);
            throw CreateAndLogServiceException(failedEssayResultException);
        }
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