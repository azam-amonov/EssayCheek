using EssayCheek.Api.Model.Foundation.EssayResults;
using FluentAssertions;
using Force.DeepCloner;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.EssayResults;

public partial class EssayResultServiceTest
{
    [Fact]
    public async Task ShouldAddEssayResultAsync()
    {
        //given
        EssayResult randomEssayResult = CreateRandomEssayResult();
        EssayResult inputEssayResult = randomEssayResult;
        EssayResult persistedEssayResult = inputEssayResult;
        EssayResult expectedEssayResult = persistedEssayResult.DeepClone();

        _storageBrokerMock.Setup(broker => 
                        broker.InsertEssayResultAsync(inputEssayResult)).
                        ReturnsAsync(persistedEssayResult);
        
        //when
        EssayResult actualEssayResult = await _essayResultService.AddEssayResultsAsync(inputEssayResult);
        
        //then
        actualEssayResult.Should().BeEquivalentTo(expectedEssayResult);
        
        _storageBrokerMock.Verify(broker =>
                        broker.InsertEssayResultAsync(actualEssayResult), Times.Once);
        
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();

    }
}