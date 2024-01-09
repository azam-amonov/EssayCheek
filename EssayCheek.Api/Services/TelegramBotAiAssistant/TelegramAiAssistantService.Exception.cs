using EssayCheek.Api.Services.TelegramBotAiAssistant.Exceptions;
using Xeptions;

namespace EssayCheek.Api.Services.TelegramBotAiAssistant;

public partial class TelegramAiAssistantService
{
    private delegate ValueTask<string> ReturningMessageAnalysisFunction();

    private async ValueTask<string> TryCatch(ReturningMessageAnalysisFunction returningEssayAnalysisFunction)
    {
        try
        {
            return await returningEssayAnalysisFunction();
        }
        catch (NullMessageAnalysisException nullEssayAnalysisException)
        {
            throw CreateAndLogValidationException(nullEssayAnalysisException);
        }catch (Exception exception)
        {
            var failedMessageAnalysisException = new FailedMessageAnalysisServiceException(exception);
            throw CreateAndLogServiceException(failedMessageAnalysisException);
        }
    }
    
    private System.Exception CreateAndLogServiceException(Xeption exception)
    {
        var essayAnalysisServiceException = new MessageAnalysisValidationException(exception);
        this.loggingBroker.LogError(essayAnalysisServiceException);

        return essayAnalysisServiceException;
    }
    
    private MessageAnalysisValidationException CreateAndLogValidationException(Xeption exception)
    {
        var messageAnalysisException = new MessageAnalysisValidationException(exception);
        this.loggingBroker.LogError(messageAnalysisException);
        
        return messageAnalysisException;
    }
}