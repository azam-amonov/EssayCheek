using Telegram.Bot;

namespace EssayCheek.Api.Brokers.Telegram;

public class TelegramBroker : ITelegramBroker
{
    private readonly IConfiguration configuration;

    public TelegramBroker(IConfiguration configuration)
    {
        this.configuration = configuration;
        this.TelegramBotClient = ConfigureTelegramBotClient();
        this.CancellationTokenSource = new CancellationTokenSource();
    }

    private ITelegramBotClient ConfigureTelegramBotClient()
    {
        var botToken = this.configuration.GetValue<string>(key: "TelegramBotToken");
        return new TelegramBotClient(token: botToken);
    }

    public ITelegramBotClient TelegramBotClient { get; }
    public CancellationTokenSource CancellationTokenSource { get; }
 
}