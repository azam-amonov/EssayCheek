using Telegram.Bot;

namespace EssayCheek.Api.Brokers.Telegram;

public partial interface ITelegramBroker
{
    Task BotStartAsync(string message);
    // string? GteMessageAsync (string? message = default);
}