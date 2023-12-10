using EssayCheek.Api.Model.Foundation.Essays;
using EssayCheek.Api.Model.Foundation.Essays.Exceptions;
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
        catch (NotFoundEssayException notFoundEssayException)
        {
            throw CreateAndLogValidationException(notFoundEssayException);
        }
        catch (Exception exception)
        {
            var failedEssayEssayException = new FailedEssayServiceException(exception);
            throw CreateAndLogServiceException(failedEssayEssayException);
        }
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