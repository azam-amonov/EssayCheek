using EssayCheek.Api.Services.TelegramBotAiAssistant.Exceptions;

namespace EssayCheek.Api.Services.TelegramBotAiAssistant;

public partial class TelegramAiAssistantService
{
    private static void TelegramMessageAnalysisIsNotNull(string analysis)
    {
        if (analysis is null)
        {
            throw new NullMessageAnalysisException();
        }
    }
}