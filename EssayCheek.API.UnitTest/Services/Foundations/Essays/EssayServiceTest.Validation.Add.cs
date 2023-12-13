using EssayCheek.Api.Model.Foundation.Essays;
using EssayCheek.Api.Model.Foundation.Essays.Exceptions;
using FluentAssertions;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Essays;

public partial class EssayServiceTest
{
    [Fact]
    public async Task ShouldValidationExceptionOnAddIfEssayIsNullAsync()
    {
       //given
       Essay? nullEssay = null;
       var nullEssayException = new EssayNullException();

       var expectedEssayValidationException = new EssayValidationException(nullEssayException);

       //when
       ValueTask<Essay> addEssayTask = _essayService.AddEssayAsync(nullEssay);

       EssayValidationException actualEssayValidationException =
                       await Assert.ThrowsAsync<EssayValidationException>(addEssayTask.AsTask);
       
       //then
       actualEssayValidationException.Should().BeEquivalentTo(expectedEssayValidationException);

       _loggingBrokerMock.Verify(broker => 
               broker.LogError(It.Is(SameExceptionAs(
                   expectedEssayValidationException))), 
           Times.Once);
       
       _loggingBrokerMock.VerifyNoOtherCalls();
       _storageBrokerMock.VerifyNoOtherCalls();
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ShouldThrowValidationExceptionOnAddIfEssayIsInvalidAndLogItAsync(string invalidText)
    {
       //given
       var invalidEssay = new Essay()
       {
                       Title = invalidText,
                       Content = invalidText,
                       SubmittedDate = default(DateTimeOffset)
       };
    
       var expectedInvalidEssayException = new InvalidEssayException();
       
       expectedInvalidEssayException.AddData(
                       key: nameof(Essay.Id),
                       values:"Id is required");
       
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
                       broker.InsertEssayAsync(invalidEssay),Times.Never);
       
       _loggingBrokerMock.VerifyNoOtherCalls();
       _storageBrokerMock.VerifyNoOtherCalls();
       _dateTimeBrokerMock.VerifyNoOtherCalls();
    }
}