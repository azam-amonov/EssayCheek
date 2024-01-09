using Xeptions;

namespace EssayCheek.Api.Services.TelegramBotAiAssistant.Exceptions;

public class MessageAnalysisValidationException : Xeption
{
    public MessageAnalysisValidationException(Xeption innerException) :
        base(message: "Message validation error occurred, fix the error and tyr again.",
            innerException) { }   
}