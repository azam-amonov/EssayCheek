using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace EssayCheek.Api.Brokers.OpenAis;

internal partial interface IOpenAiBroker
{
    ValueTask<ChatCompletion> AnalyzeEssayAsync(ChatCompletion chatCompletion);
}