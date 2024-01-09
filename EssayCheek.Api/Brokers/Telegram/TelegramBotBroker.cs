using System.Runtime.CompilerServices;
using Telegram.Bot;

namespace EssayCheek.Api.Brokers.Telegram;

public partial class TelegramBotBroker : ITelegramBotBroker
{
    private readonly IConfiguration configuration;
    private readonly CancellationTokenSource cancellationTokenSource;

    public TelegramBotBroker(IConfiguration configuration)
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

    public CancellationToken CancellationToken => this.cancellationTokenSource.Token;
    public ITelegramBotClient TelegramBotClient { get; }
}