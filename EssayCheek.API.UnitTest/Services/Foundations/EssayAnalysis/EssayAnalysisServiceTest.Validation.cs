using EssayCheek.Api.Services.EssayAnalysis.Exceptions;
using FluentAssertions;
using Moq;
using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions.Exceptions;
using Xunit.Sdk;

namespace EssayCheek.API.UnitTest.Services.Foundations.EssayAnalysis;

public partial class EssayAnalysisServiceTest
{
    [Fact] public async Task ShouldValidateExceptionOnAnalyzeEssayIfContentIsNullAsync()
    {
        //given
        string? nullContent = null;
        var nullEssayAnalysisServiceException = new NullEssayAnalysisException();
        
        var expectedEssayAnalysisValidationException = 
            new EssayAnalysisValidationException(nullEssayAnalysisServiceException);
        
        //when
        ValueTask<string> responseEssayAnalysisTask = 
            _essayAnalysisService.EssayAnalysisAsync(nullContent);

        EssayAnalysisValidationException actualEssayAnalysisValidationException =
            await Assert.ThrowsAsync<EssayAnalysisValidationException>(
                responseEssayAnalysisTask.AsTask);
        
        //then 
        actualEssayAnalysisValidationException.Should()
            .BeEquivalentTo(expectedEssayAnalysisValidationException);
        
        _loggerBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(
                expectedEssayAnalysisValidationException))),
            Times.Once);
        
        _loggerBrokerMock.VerifyNoOtherCalls();
        _openAiBrokerMock.VerifyNoOtherCalls();
    }
}