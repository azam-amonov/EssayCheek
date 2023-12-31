using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace EssayCheek.Api.Brokers.OpenAis;

internal partial class OpenAiBroker
{
    public async ValueTask<ChatCompletion> MessageAnalyzeAsync(ChatCompletion chatCompletion)
    {
	    return await _openAiClient.ChatCompletions.SendChatCompletionAsync(chatCompletion);
    }
}