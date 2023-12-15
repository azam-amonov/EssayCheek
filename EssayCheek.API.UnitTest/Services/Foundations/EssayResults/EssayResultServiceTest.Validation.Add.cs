using EssayCheek.Api.Model.Foundation.EssayResults;
using EssayCheek.Api.Model.Foundation.EssayResults.Exceptions;
using FluentAssertions;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.EssayResults;

public partial class EssayResultServiceTest
{
    [Fact]
    public async Task ShouldValidateExceptionOnAddIfEssayResultIsNullAsync()
    {
        //given
        EssayResult? nullEssayResult = null;
        var nullEssayResultException = new EssayResultNullException();
        var expectedEssayResultValidationException = new 
            EssayResultValidationException(nullEssayResultException);

        //when
        ValueTask<EssayResult> addEssayResultTask = 
            _essayResultService.AddEssayResultsAsync(nullEssayResult);

        EssayResultValidationException actualEssayResultValidationException = 
            await Assert.ThrowsAsync<EssayResultValidationException>(addEssayResultTask.AsTask);
        
        //then
        actualEssayResultValidationException.Should().BeEquivalentTo(
            expectedEssayResultValidationException);
        
        _loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(
                expectedEssayResultValidationException))),
            Times.Once);
        
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ShouldThrowValidationExceptionOnAddIfEssayResultIsInvalidAndLogItAsync(string invalidText)
    {
        //given
        var invalidEssayResult = new EssayResult()
        {
            Feedback = invalidText,
        };

        var expectedInvalidEssayResultException = new InvalidEssayResultException();
        
        expectedInvalidEssayResultException.AddData(
            key: nameof(EssayResult.Feedback),
            values: "Text is required");
        
        expectedInvalidEssayResultException.AddData(
            key: nameof(EssayResult.Id),
            values: "Id is required");

        var expectedEssayResultValidationException =
            new EssayResultValidationException(expectedInvalidEssayResultException);
        
        //when
        ValueTask<EssayResult> addEssayResultTask = 
            _essayResultService.AddEssayResultsAsync(invalidEssayResult);

        EssayResultValidationException actualEssayResultValidationException =
            await Assert.ThrowsAsync<EssayResultValidationException>(addEssayResultTask.AsTask);

        //then
        actualEssayResultValidationException.Should().
            BeEquivalentTo(expectedEssayResultValidationException);
        
        _loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(
                expectedEssayResultValidationException))),
            Times.Once);
        
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
}