using Telegram.Bot;

namespace EssayCheek.Api.Brokers.Telegram;

public partial interface ITelegramBroker
{
    internal ITelegramBotClient TelegramBotClient { get; }
}