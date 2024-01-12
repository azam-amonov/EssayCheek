using EssayCheek.Api.Services.EssayAnalysis;
using EssayCheek.Api.Services.TelegramBotAiAssistant;
using Telegram.Bot;
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
        var botClient = this.telegramBroker.TelegramBotClient;
        var message = update.Message;

        
        await MessageAction(botClient: botClient, update: update, cancellationToken: cancellationToken);
        var response = await telegramAiAssistant.UserMessageAnalysis(message.Text);
        Console.WriteLine(response);
        
        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: message.Text,
            replyToMessageId: message.MessageId,
            cancellationToken: cancellationToken);
    }

    private static async Task MessageAction(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        var message = update.Message;
        
        await botClient.SendChatActionAsync(
            chatId: message.Chat.Id,
            chatAction: ChatAction.Typing,
            cancellationToken: cancellationToken);
    }
}