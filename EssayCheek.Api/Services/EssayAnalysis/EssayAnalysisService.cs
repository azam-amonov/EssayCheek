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
			ChatCompletion response = await _openAiBroker.MessageAnalyzeAsync(request);

			return response.Response.Choices.FirstOrDefault()!.Message.Content;
		});

	private static ChatCompletion CreateRequest(string message)
	{
		var request = new ChatCompletion
		{
			Request = new ChatCompletionRequest
			{
				Model = "gpt-4-1106-preview",
				MaxTokens = 60,
				Messages = new ChatCompletionMessage[]
				{
					new ChatCompletionMessage
					{
						Role = "assistant",
						Content = "send one interesting fact!"	
							// "You are an assistant, the user asked you to help him write an essay, so help him. " +
							// "If the question is not related to the essay, tell him that the message is incorrect."
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