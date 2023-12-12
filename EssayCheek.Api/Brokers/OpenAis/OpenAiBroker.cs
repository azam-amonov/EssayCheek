using Standard.AI.OpenAI.Clients.OpenAIs;
using Standard.AI.OpenAI.Models.Configurations;

namespace EssayCheek.Api.Brokers.OpenAis;

internal partial class OpenAiBroker : IOpenAiBroker
{
    private readonly IOpenAIClient _openAiClient;
    private readonly IConfiguration _configuration;

    public OpenAiBroker(IConfiguration configuration)
    {
        _configuration = configuration;
        _openAiClient = ConfigureOpenAiClient();
    }

    private IOpenAIClient ConfigureOpenAiClient()
    {
        var apiKey = _configuration.GetValue<string>(key:"OpenAiKey");
        var openAiConfiguration = new OpenAIConfigurations()
        {
            ApiKey = apiKey
        }; 
        
        return new OpenAIClient(openAiConfiguration);
    }
}

