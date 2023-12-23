using System.Reflection;
using EssayCheek.Api.Brokers.DateTimes;
using EssayCheek.Api.Brokers.Logging;
using EssayCheek.Api.Brokers.OpenAis;
using EssayCheek.Api.Brokers.StorageBroker;
using EssayCheek.Api.Services.EssayAnalysis;
using EssayCheek.Api.Services.EssayResults;
using EssayCheek.Api.Services.Essays;
using EssayCheek.Api.Services.TelegramBots;
using EssayCheek.Api.Services.TextFormatterService;
using EssayCheek.Api.Services.Users;

namespace EssayCheek.Api.Configuration;

public static partial class HostConfiguration
{
    private static readonly ICollection<Assembly> Assemblies;
    public static IConfiguration Configuration { get;}

    static HostConfiguration()
    {
        Assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(Assembly.Load).ToList();
        Assemblies.Add(Assembly.GetExecutingAssembly());

        var configurationBuilder = new ConfigurationBuilder();
        
        configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        configurationBuilder.AddEnvironmentVariables();

        Configuration = configurationBuilder.Build();
    }
    
    private static WebApplicationBuilder AddBrokers(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddTransient<IStorageBroker, StorageBroker>()
            .AddTransient<ILoggingBroker, LoggingBroker>()
            .AddTransient<IOpenAiBroker, OpenAiBroker>()
            .AddScoped<IDateTimeBroker, DateTimeBroker>();
        
        return builder;
    }

    private static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddTransient<IUserService, UserService>()
            .AddTransient<IEssayService, EssayService>()
            .AddTransient<IEssayResultService, EssayResultService>()
            .AddTransient<IEssayAnalysisService, EssayAnalysisService>()
            .AddTransient<ITelegramBotService, TelegramBotService>();
        
        return builder;
    }
    

    private static WebApplicationBuilder AddTextInputFormatter(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(options =>
        {
            options.InputFormatters.Add(new TextInputFormatter());
        });

        return builder;
    }

    private static WebApplicationBuilder AddExposers(this WebApplicationBuilder builder)
    {
        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddControllers();
        
        return builder;
    }
    
    private static WebApplicationBuilder AddDevTools(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddDbContext<StorageBroker>();

        return builder;
    }

    private static WebApplication UseExposes(this WebApplication app)
    {
        app.MapControllers();
        return app;
    }

    private static WebApplication UseDevTools(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        
        return app;
    }
}