using FluentAssertions;
using Moq;
using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace EssayCheek.API.UnitTest.Services.Foundations.EssayAnalysis;

public partial class EssayAnalysisServiceTest
{
	[Fact]
	public async Task ShouldAnalyzeEssayServicesAsync()
	{
		//given
		string randomText = GetRandomString();
		string inputEssay = randomText;
		string anotherRandomText = GetRandomString();
		string expectedAnalysis = anotherRandomText;
		
		ChatCompletion analyzedChatCompletion = CreateOutputChatCompletion(expectedAnalysis);

		_openAiBrokerMock.Setup(broker => 
				broker.MessageAnalyzeAsync(It.IsAny<ChatCompletion>()))
					.ReturnsAsync(analyzedChatCompletion);
		
		//when
		string actualAnalysis = await _essayAnalysisService.EssayAnalysisAsync(inputEssay);
		
		//then
		actualAnalysis.Should().BeEquivalentTo(expectedAnalysis);

		_openAiBrokerMock.Verify(broker => 
			broker.MessageAnalyzeAsync(It.IsAny<ChatCompletion>()), 
				Times.Once);

		_openAiBrokerMock.VerifyNoOtherCalls();
		_loggerBrokerMock.VerifyNoOtherCalls();
	}
}