using EssayCheek.Api.Brokers.Logging;
using EssayCheek.Api.Brokers.OpenAis;
using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace EssayCheek.Api.Services.EssayAnalysis;

public partial class EssayAnalysisService : IEssayAnalysisService
{
	private readonly IOpenAiBroker _openAiBroker;
	private readonly ILoggingBroker _loggingBroker;

	public EssayAnalysisService(IOpenAiBroker openAiBroker, ILoggingBroker loggingBroker)
	{
		_openAiBroker = openAiBroker;
		_loggingBroker = loggingBroker;
	}

	public ValueTask<string> EssayAnalysisAsync(string essay) =>
		TryCatch(async () =>
		{
			ValidateEssayAnalysisIsNotNull(essay);
			
			ChatCompletion request = CreateRequest(essay);
			ChatCompletion response = await _openAiBroker.AnalyzeEssayAsync(request);

			return response.Response.Choices.FirstOrDefault()!.Message.Content;
		});

	private static ChatCompletion CreateRequest(string essay)
	{
		var request = new ChatCompletion
		{
			Request = new ChatCompletionRequest
			{
				Model = "gpt-4-1106-preview",
				MaxTokens = 1500,
				Messages = new ChatCompletionMessage[]
				{
					new ChatCompletionMessage
					{
						Role = "system",
						Content =
							"You are IELTS Writing examiner. Give detailed IELTS feedback based on marking criteria of IELTS."
					},
					new ChatCompletionMessage
					{
						Role = "user",
						Content = essay
					}
				}
			}
		};
		return request;
	}
}