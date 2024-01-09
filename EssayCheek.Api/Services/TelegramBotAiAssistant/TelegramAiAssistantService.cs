using EssayCheek.Api.Brokers.Logging;
using EssayCheek.Api.Brokers.OpenAis;
using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace EssayCheek.Api.Services.TelegramBotAiAssistant;

public partial class TelegramAiAssistantService : ITelegramAiAssistantService
{
    private readonly IOpenAiBroker openAiBroker;
    private readonly ILoggingBroker loggingBroker;

    public TelegramAiAssistantService(IOpenAiBroker openAiBroker, ILoggingBroker loggingBroker)
    {
        this.openAiBroker = openAiBroker;
        this.loggingBroker = loggingBroker;
    }
    
    public ValueTask<string> UserMessageAnalysis(string message) =>
        TryCatch(async () =>
        {
            TelegramMessageAnalysisIsNotNull(message);
			
            ChatCompletion request = CreateRequest(message);
            ChatCompletion response = await this.openAiBroker.MessageAnalyzeAsync(request);

            return response.Response.Choices.FirstOrDefault()!.Message.Content;
        });
    
    private static ChatCompletion CreateRequest(string message)
    {
        var request = new ChatCompletion
        {
            Request = new ChatCompletionRequest
            {
                Model = "gpt-4-1106-preview",
                MaxTokens = 100,
                Messages = new ChatCompletionMessage[]
                {
                    new ChatCompletionMessage
                    {
                        Role = "assistant",
                        Content = 
                            "You are an assistant, and the user has sought your help in crafting an essay. " +
                            "Please provide assistance with the essay. If the question is unrelated to the essay, " +
                            "inform the user that the message is not applicable to the essay context."
                    },
                    new ChatCompletionMessage
                    {
                        Role = "user",
                        Content = message
                    }
                }
            }
        };
        
        return request;
    }
}