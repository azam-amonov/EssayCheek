using Xeptions;

namespace EssayCheek.Api.Services.TelegramBotAiAssistant.Exceptions;

public class NullMessageAnalysisException : Xeption
{
    public NullMessageAnalysisException() : base (message:"Null message Exception") { }
}