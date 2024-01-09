namespace EssayCheek.Api.Services.TelegramBotAiAssistant;

public interface ITelegramAiAssistantService
{
    public ValueTask<string> UserMessageAnalysis(string message);
}