using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace EssayCheek.Api.Brokers.OpenAis;

public partial interface IOpenAiBroker
{
    ValueTask<ChatCompletion> MessageAnalyzeAsync(ChatCompletion chatCompletion);
}