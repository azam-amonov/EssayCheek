using EssayCheek.Api.Brokers.Logging;
using EssayCheek.Api.Brokers.OpenAis;
using EssayCheek.Api.Services.EssayAnalysis;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;
using Tynamix.ObjectFiller;

namespace EssayCheek.API.UnitTest.Services.Foundations.EssayAnalysis;

public partial class EssayAnalysisServiceTest
{
	private readonly Mock<ILoggingBroker> _loggerBrokerMock;
	private readonly Mock<IOpenAiBroker> _openAiBrokerMock;
	private readonly IEssayAnalysisService _essayAnalysisService;

	public EssayAnalysisServiceTest()
	{
		_openAiBrokerMock = new Mock<IOpenAiBroker>();
		_loggerBrokerMock = new Mock<ILoggingBroker>();

		_essayAnalysisService = new EssayAnalysisService(
			openAiBroker: _openAiBrokerMock.Object, 
			loggingBroker: _loggerBrokerMock.Object);
	}

	private static string GetRandomString() => 
			new MnemonicString(wordCount:GetRandomNumber()).GetValue();

	private static int GetRandomNumber() => 
			new IntRange(min: 9, max: 99).GetValue();

	private static ChatCompletion CreateOutputChatCompletion(string analysis)
	{
		var inputFiller = CreateInputChatCompletionFiller();
		var partialChatCompletion = inputFiller.Create();
		
		partialChatCompletion.Response.Choices = new ChatCompletionChoice[]
		{
			new ChatCompletionChoice
			{
				Message = new ChatCompletionMessage
				{
					Content = analysis
				}
			}
		};

		return partialChatCompletion;
	}

	// {
	// 	return new ChatCompletion
	// 	{
	// 		Response = new ChatCompletionResponse
	// 		{
	// 			Choices = new ChatCompletionChoice[]
	// 			{
	// 				new ChatCompletionChoice
	// 				{
	// 					Message = new ChatCompletionMessage
	// 					{
	// 						Content = analysis
	// 					}
	// 				}
	// 			}
	// 		}
	// 	};
	// }



	private static Filler<ChatCompletion> CreateInputChatCompletionFiller()
	{
		var filler = new Filler<ChatCompletion>();
		filler.Setup().OnProperty(chatCompletion => chatCompletion.Response).IgnoreIt();
		
		return filler;
	}
}