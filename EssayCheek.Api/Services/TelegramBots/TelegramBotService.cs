using EssayCheek.Api.Services.EssayAnalysis;
using EssayCheek.Api.Settings;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EssayCheek.Api.Services.TelegramBots;

public  partial class TelegramBotService : ITelegramBotService
{
    private readonly CancellationTokenSource cancellationTokenSource = new();
    private readonly IEssayAnalysisService essayAnalysisService;
    private readonly BotSettings botSettings;
    
    public TelegramBotService(IEssayAnalysisService essayAnalysisService,
        IOptions<BotSettings> botOptions)
    {
        this.essayAnalysisService = essayAnalysisService;
        this.botSettings = botOptions.Value;
    }

    public Task BotStartAsync()
    {
        var botClient = new TelegramBotClient(this.botSettings.Token);

        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cancellationTokenSource.Token
        );
        
        return Task.CompletedTask;
    }
}