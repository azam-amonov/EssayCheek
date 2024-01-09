using Telegram.Bot;

namespace EssayCheek.Api.Brokers.Telegram;

public partial interface ITelegramBotBroker
{
    Task BotStartAsync(Task updateHandler, Task exceptionHandler);
}