using Telegram.Bot;

namespace EssayCheek.Api.Brokers.Telegram;

public partial interface ITelegramBotBroker
{
    internal CancellationToken CancellationToken { get; }
    internal ITelegramBotClient TelegramBotClient { get; }
}