using Xeptions;

namespace EssayCheek.Api.Services.TelegramBotAiAssistant.Exceptions;

public class FailedMessageAnalysisServiceException : Xeption
{
    public FailedMessageAnalysisServiceException(System.Exception innerException): 
        base(message:"Failed Message Analysis Service Exception", innerException) {}
}