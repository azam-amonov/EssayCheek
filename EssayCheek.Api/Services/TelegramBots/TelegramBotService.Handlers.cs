using EssayCheek.Api.Services.EssayAnalysis;
using EssayCheek.Api.Services.TelegramBotAiAssistant;

namespace EssayCheek.Api.Services.TelegramBots;

public partial class TelegramBotService
{
    private readonly IEssayAnalysisService essayAnalysisService;
    private readonly ITelegramAiAssistantService telegramAiAssistant;
}