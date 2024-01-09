using Telegram.Bot.Types;

namespace EssayCheek.Api.Services.TelegramBots;

public partial class TelegramBotService
{
    private static string ValidateMessage(Message message)
    {
        return "";
    }

    private static string WelcomeMessage(Message message) => 
        message.Text!.Equals("/start") ? $"Welcome to EssayCheek {message.Chat.FirstName}" 
            : message.Text;

    private static string HelpMessage(Message message) =>
        message.Text!.Equals("/help") ? "Here is list of commands": message.Text;
    
    // private static static 
}
