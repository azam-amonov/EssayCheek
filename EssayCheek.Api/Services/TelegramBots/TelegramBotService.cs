using EssayCheek.Api.Brokers.Logging;
using EssayCheek.Api.Brokers.Telegram;
using EssayCheek.Api.Services.EssayAnalysis;
using EssayCheek.Api.Services.TelegramBotAiAssistant;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace EssayCheek.Api.Services.TelegramBots;

public partial class TelegramBotService : ITelegramBotService
{
    private readonly ITelegramBroker telegramBroker;
    private readonly ILoggingBroker loggingBroker;
    
    public TelegramBotService(ITelegramBroker telegramBroker, 
        ILoggingBroker loggingBroker, 
        IEssayAnalysisService essayAnalysisService, 
        ITelegramAiAssistantService telegramAiAssistant)
    {
        this.telegramBroker = telegramBroker;
        this.loggingBroker = loggingBroker;
        this.telegramAiAssistant = telegramAiAssistant;
        this.essayAnalysisService = essayAnalysisService;
    }


    public Task BotStartAsync()
    {
        var botClient = this.telegramBroker.TelegramBotClient;
        var cancellationToken = this.telegramBroker.CancellationTokenSource.Token;
        
        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cancellationToken
        );

        return Task.CompletedTask;
    }
}