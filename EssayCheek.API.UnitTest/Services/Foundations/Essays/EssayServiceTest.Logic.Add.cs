using EssayCheek.Api.Model.Foundation.Essays;
using EssayCheek.Api.Model.Foundation.Essays.Exceptions;
using FluentAssertions;
using Force.DeepCloner;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Essays;

public partial class EssayServiceTest
{
    [Fact]
    public async Task ShouldAddEssayAsync()
    {
        // given
        Essay randomEssay = CreateRandomEssay();
        Essay inputEssay = randomEssay;
        Essay persistedEssay = inputEssay;
        Essay expectedEssay = persistedEssay.DeepClone();

        _storageBrokerMock.Setup(broker =>
                        broker.InsertEssayAsync(inputEssay)).ReturnsAsync(persistedEssay);
        
        // when
        Essay actualEssay = await _essayService.AddEssayAsync(inputEssay);
        
        // then
        actualEssay.Should().BeEquivalentTo(expectedEssay);

        _storageBrokerMock.Verify(broker => 
                        broker.InsertEssayAsync(actualEssay),Times.Once);
        
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task ShouldThrowValidationExceptionOnAddIfEssayIsInvalidAndLogItAsync(string invalidText)
    {
       //given
       var invalidEssay = new Essay()
       {
                       Title = invalidText,
                       Content = invalidText
       };

       var expectedInvalidEssayException = new InvalidEssayException();
       
       expectedInvalidEssayException.AddData(
                       key: nameof(Essay.Id),
                       values:"Id is required");
       
       expectedInvalidEssayException.AddData(
                       key: nameof(Essay.UserId),
                       values: "Id is required");
       
       expectedInvalidEssayException.AddData(
                       key: nameof(Essay.Title),
                       values:"Text is required");
       
       expectedInvalidEssayException.AddData(
                       key: nameof(Essay.Content),
                       values:"Text is required");
       
       expectedInvalidEssayException.AddData(
                       key: nameof(Essay.SubmittedDate),
                       values:"Date is required");

       var expectedEssayValidationException =
                       new EssayValidationException(expectedInvalidEssayException);
       
       //when
       ValueTask<Essay> addEssayTask = _essayService.AddEssayAsync(invalidEssay);

       EssayValidationException actualEssayValidationException =
                       await Assert.ThrowsAsync<EssayValidationException>(addEssayTask.AsTask);
       
       //then
       actualEssayValidationException.Should().BeEquivalentTo(expectedEssayValidationException);
       
       _loggingBrokerMock.Verify(broker => 
                       broker.LogError(It.Is(SameExceptionAs(
                                       expectedEssayValidationException))),Times.Once);
       
       _storageBrokerMock.Verify(broker =>
                       broker.InsertEssayAsync(invalidEssay), Times.Never);
       
       _storageBrokerMock.VerifyNoOtherCalls();
       _loggingBrokerMock.VerifyNoOtherCalls();
    }
}















