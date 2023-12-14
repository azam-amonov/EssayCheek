using System.Linq.Expressions;
using EssayCheek.Api.Brokers.Logging;
using EssayCheek.Api.Brokers.OpenAis;
using EssayCheek.Api.Services.EssayAnalysis;
using Moq;
using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;
using Tynamix.ObjectFiller;
using Xeptions;

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
	
	private static DateTimeOffset GetRandomDateTimeOffset() =>
		new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

	private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption exceptedException) => 
		actualException => actualException.SameExceptionAs(exceptedException);

	private static ChatCompletion CreateOutputChatCompletion(string analysis)
	{
		return CreateOutputChatCompletionFiller(analysis).Create();
	}

	private static Filler<ChatCompletion> CreateOutputChatCompletionFiller(string analysis)
	{
		var filler = new Filler<ChatCompletion>();
		
		filler.Setup().OnProperty(chatCompletion => 
			chatCompletion.Response.Choices.FirstOrDefault()!.Message.Content).Use(analysis);
		
		filler.Setup().OnType<DateTimeOffset>().Use(GetRandomDateTimeOffset);
		
		return filler;
	}
}