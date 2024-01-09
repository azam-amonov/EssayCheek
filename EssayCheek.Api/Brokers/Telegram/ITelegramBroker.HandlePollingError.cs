using Telegram.Bot;
using Telegram.Bot.Types;

namespace EssayCheek.Api.Brokers.Telegram;

public partial interface ITelegramBroker
{
    Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken);

}