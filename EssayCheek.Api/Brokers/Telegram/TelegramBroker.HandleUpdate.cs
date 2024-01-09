using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EssayCheek.Api.Brokers.Telegram;

public partial class TelegramBroker
{
    public async Task HandleUpdateAsync(ITelegramBotClient telegramBotClient, Update update, 
        CancellationToken cancellationToken)
    {
        var message = update.Message;
        // message.Text = GteMessageAsync();

        await TelegramBotClient.SendChatActionAsync(chatId: message.Chat.Id, chatAction: ChatAction.Typing,
            cancellationToken: cancellationToken );
        
        await TelegramBotClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: message.Text,
            replyToMessageId: message.MessageId,
            cancellationToken: cancellationToken);
    }
}