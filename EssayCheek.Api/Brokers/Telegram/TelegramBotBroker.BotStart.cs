namespace EssayCheek.Api.Brokers.Telegram;

public partial class TelegramBotBroker
{
    public Task BotStartAsync(Task updateHandler, Task exceptionHandler)
    {
        return Task.CompletedTask;
    }
}