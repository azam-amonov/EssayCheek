using Telegram.Bot;
using Telegram.Bot.Types;

namespace EssayCheek.Api.Brokers.Telegram;

public partial interface ITelegramBroker
{
    Task HandleUpdateAsync(ITelegramBotClient telegramBotClient, Update update,
        CancellationToken cancellationToken);
}