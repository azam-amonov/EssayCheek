using EssayCheek.Api.Brokers.Logging;
using EssayCheek.Api.Brokers.OpenAis;
using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace EssayCheek.Api.Services.EssayAnalysis;

public class EssayAnalysisService : IEssayAnalysisService
{
	private readonly IOpenAiBroker _openAiBroker;
	private readonly ILoggingBroker _loggingBroker;

	public EssayAnalysisService(IOpenAiBroker openAiBroker, ILoggingBroker loggingBroker)
	{
		_openAiBroker = openAiBroker;
		_loggingBroker = loggingBroker;
	}


	public async ValueTask<string> AnalyzeEssayAsync(string essay)
	{
		ChatCompletion request = CreateRequest(essay);
		ChatCompletion response = await _openAiBroker.AnalyzeEssayAsync(request);

		return response.Response.Choices.FirstOrDefault()!.Message.Content;
	}

	private static ChatCompletion CreateRequest(string essay)
	{
		var request = new ChatCompletion
		{
			Request = new ChatCompletionRequest
			{
				Model = "gpt-4-1106-preview",
				Messages = new ChatCompletionMessage[]
				{
					new ChatCompletionMessage
					{
						Role = "System",
						Content =
							"You are IELTS Writing examiner. Give detailed IELTS score based on marking criteria of IELTS."
					},
					new ChatCompletionMessage
					{
						Role = "User",
						Content = essay
					}
				}
			}
		};
		
		return request;
	}
}