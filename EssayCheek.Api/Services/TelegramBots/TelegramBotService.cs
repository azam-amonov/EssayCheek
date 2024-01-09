using EssayCheek.Api.Brokers.Logging;
using EssayCheek.Api.Brokers.Telegram;
using EssayCheek.Api.Services.EssayAnalysis;
using EssayCheek.Api.Services.TelegramBotAiAssistant;

namespace EssayCheek.Api.Services.TelegramBots;

public partial class TelegramBotService : ITelegramBotService
{
    private readonly ITelegramBroker telegramBroker;
    private readonly ILoggingBroker loggingBroker;
    
    public TelegramBotService(ITelegramBroker telegramBroker, ILoggingBroker loggingBroker, 
        IEssayAnalysisService essayAnalysisService, ITelegramAiAssistantService telegramAiAssistant)
    {
        this.telegramBroker = telegramBroker;
        this.loggingBroker = loggingBroker;
        this.telegramAiAssistant = telegramAiAssistant;
        this.essayAnalysisService = essayAnalysisService;
    }


    public Task BotStartAsync()
    {
        this.telegramBroker.BotStartAsync("BotStart");
        // this.telegramBroker.GteMessageAsync("Fuck You!");
        return Task.CompletedTask;
    }
}