using EssayCheek.Api.Services.EssayAnalysis.Exceptions;
using Xeptions;

namespace EssayCheek.Api.Services.EssayAnalysis;

public partial class EssayAnalysisService
{
    private delegate ValueTask<string> ReturningEssayAnalysisFunction();

    private async ValueTask<string> TryCatch(ReturningEssayAnalysisFunction returningEssayAnalysisFunction)
    {
        try
        {
            return await returningEssayAnalysisFunction();
        }
        catch (NullEssayAnalysisException nullEssayAnalysisException)
        {
            throw CreateAndLogValidationException(nullEssayAnalysisException);
        }
        catch (Exception exception)
        {
            var failedEssayAnalysisException = new FailedEssayAnalysisServiceException(exception);
            throw CreateAndLogServiceException(failedEssayAnalysisException);
        }
    }

    private Exception CreateAndLogServiceException(Xeption exception)
    {
        var essayAnalysisServiceException = new EssayAnalysisValidationException(exception);
        _loggingBroker.LogError(essayAnalysisServiceException);

        return essayAnalysisServiceException;
    }

    private EssayAnalysisValidationException CreateAndLogValidationException(Xeption exception)
    {
        var essayAnalysisException = new EssayAnalysisValidationException(exception);
        _loggingBroker.LogError(essayAnalysisException);

        return essayAnalysisException;
    }
}