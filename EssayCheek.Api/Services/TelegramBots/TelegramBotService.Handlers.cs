using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EssayCheek.Api.Services.TelegramBots;

public partial class TelegramBotService
{
    private async Task HandleUpdateAsync(ITelegramBotClient telegramBotClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update.Message is not { } message)
            return;

        if (message.Text is not { } messageText)
            return;

        var textToAi = await this.essayAnalysisService.EssayAnalysisAsync(messageText);
        
        Message sentMessage = await telegramBotClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            parseMode: ParseMode.Markdown,
            text: textToAi,
            cancellationToken: cancellationToken);
    }
    
    private static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, 
        CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        return Task.CompletedTask;
    }
}