using EssayCheek.Api.Services.EssayAnalysis;
using EssayCheek.Api.Services.TelegramBotAiAssistant;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EssayCheek.Api.Services.TelegramBots;

public partial class TelegramBotService
{
    private readonly IEssayAnalysisService essayAnalysisService;
    private readonly ITelegramAiAssistantService telegramAiAssistant;

    private async Task HandleUpdateAsync(ITelegramBotClient telegramBotClient, Update update,
        CancellationToken cancellationToken)
    {
        var message = update.Message;
        string botMessage; 
        Console.WriteLine(message.Text);
        
        await telegramBotClient.SendChatActionAsync(chatId: message.Chat.Id, ChatAction.Typing,
            cancellationToken: cancellationToken );
        
            botMessage = await telegramAiAssistant.UserMessageAnalysis(message.Text);
            // botMessage = await essayAnalysisService.EssayAnalysisAsync(message.Text);
            // botMessage = message.Text;
        
        await telegramBotClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            parseMode: ParseMode.Markdown,
            text: botMessage,
            replyToMessageId: message.MessageId,
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