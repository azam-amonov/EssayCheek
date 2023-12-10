using EssayCheek.Api.Model.Foundation.Essays;
using EssayCheek.Api.Model.Foundation.Essays.Exceptions;
using FluentAssertions;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Essays;

public partial class EssayServiceTest
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnRemoveByIdIfInvalidAsync()
    {
        //given
        Guid invalidEssayId = Guid.Empty;

        var invalidEssayException = new InvalidEssayException();
        
        invalidEssayException.AddData(
                        key: nameof(Essay.Id),
                        values: "Id is required");

        var exceptedEssayValidationException =
                        new EssayValidationException(invalidEssayException);

        //when
        ValueTask<Essay> removeEssayByIdTask = _essayService.RemoveEssayByIdAsync(invalidEssayId);

        EssayValidationException actualEssayValidationException =
                        await Assert.ThrowsAsync<EssayValidationException>(removeEssayByIdTask.AsTask);
        
        //then
        actualEssayValidationException.Should().BeEquivalentTo(exceptedEssayValidationException);
        
        _loggingBrokerMock.Verify(broker =>
                        broker.LogError(It.Is(SameExceptionAs(exceptedEssayValidationException))),
                        Times.Once);
        
        _storageBrokerMock.Verify(broker =>
                        broker.DeleteEssayAsync(It.IsAny<Essay>()), 
                        Times.Never);
        
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
        _dateTimeBrokerMock.VerifyNoOtherCalls();
    }
}