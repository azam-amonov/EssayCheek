using EssayCheek.Api.Brokers.Logging;
using EssayCheek.Api.Brokers.Telegram;
using EssayCheek.Api.Services.EssayAnalysis;
using EssayCheek.Api.Services.TelegramBotAiAssistant;
using EssayCheek.Api.Settings;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace EssayCheek.Api.Services.TelegramBots;

public  partial class TelegramBotService : ITelegramBotService
{
    private readonly ITelegramBotBroker telegramBotBroker;
    private readonly ILoggingBroker loggingBroker;
    
    public TelegramBotService(ITelegramBotBroker telegramBotBroker, ILoggingBroker loggingBroker, 
        IEssayAnalysisService essayAnalysisService, ITelegramAiAssistantService telegramAiAssistant)
    {
        this.telegramBotBroker = telegramBotBroker;
        this.loggingBroker = loggingBroker;
        this.telegramAiAssistant = telegramAiAssistant;
        this.essayAnalysisService = essayAnalysisService;
    }

    public Task BotStartAsync(){

    var botClient = this.telegramBotBroker.TelegramBotClient;

        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: this.telegramBotBroker.CancellationToken
        );
        
        return Task.CompletedTask;
    }
}