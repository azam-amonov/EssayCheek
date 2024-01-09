using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace EssayCheek.Api.Brokers.Telegram;

public partial class TelegramBroker
{
    public Task BotStartAsync(string message)
    {
        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        TelegramBotClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cancellationTokenSource.Token
        );
        
        return Task.CompletedTask;
    }

    // public string? GteMessageAsync(string? message = default) => message;

}