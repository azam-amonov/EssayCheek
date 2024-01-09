using Telegram.Bot;

namespace EssayCheek.Api.Brokers.Telegram;

public partial class TelegramBroker : ITelegramBroker
{
    private readonly IConfiguration configuration;
    private readonly CancellationTokenSource cancellationTokenSource;

    public TelegramBroker(IConfiguration configuration)
    {
        this.configuration = configuration;
        this.TelegramBotClient = ConfigureTelegramBotClient();
        this.cancellationTokenSource = new CancellationTokenSource();
    }

    private ITelegramBotClient ConfigureTelegramBotClient()
    {
        var botToken = this.configuration.GetValue<string>(key: "TelegramBotToken");
        return new TelegramBotClient(token: botToken);
    }

    public ITelegramBotClient TelegramBotClient { get; }
}