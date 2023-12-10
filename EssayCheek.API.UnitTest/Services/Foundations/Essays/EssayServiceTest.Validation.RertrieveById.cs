using EssayCheek.Api.Model.Foundation.Essays;
using EssayCheek.Api.Model.Foundation.Essays.Exceptions;
using FluentAssertions;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Essays;

public partial class EssayServiceTest
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnRetrieveEssayByIdAndLogItAsync()
    {
        //given
        Guid invalidEssayId = Guid.Empty;
        var invalidEssayException = new InvalidEssayException();
        
        invalidEssayException.AddData(
                        key:nameof(Essay.Id),
                        values: "Id is required");

        var expectedEssayValidationException =
                        new EssayValidationException(invalidEssayException);
        
        //when
        ValueTask<Essay> retrieveEssayByIdTask =
                        _essayService.RetrieveEssayByIdAsync(invalidEssayId);

        EssayValidationException actualEssayValidationException =
                        await Assert.ThrowsAsync<EssayValidationException>(retrieveEssayByIdTask.AsTask);
        
        //then
        actualEssayValidationException.Should().BeEquivalentTo(expectedEssayValidationException);
        
        _loggingBrokerMock.Verify(broker => 
                        broker.LogError(It.Is(SameExceptionAs(
                                        expectedEssayValidationException))), Times.Once);

        _storageBrokerMock.Verify(broker =>
                        broker.SelectEssayByIdAsync(It.IsAny<Guid>()), Times.Never);
        
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
        _dateTimeBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdEssayNotFoundAndLogItAsync()
    {
        //given
        Guid someEssayId = Guid.NewGuid();
        Essay? noEssay = null;

        var notNotFoundEssayException = 
                        new NotFoundEssayException(someEssayId);
        
        var expectedEssayValidationException = 
                        new EssayValidationException(notNotFoundEssayException);

        _storageBrokerMock.Setup(broker =>
                        broker.SelectEssayByIdAsync(It.IsAny<Guid>()))
                        .ReturnsAsync(noEssay);
        
        //when
        ValueTask<Essay> retrieveEssayByIdTask =
                        _essayService.RetrieveEssayByIdAsync(someEssayId);

        EssayValidationException actualEssayValidationException =
                        await Assert.ThrowsAsync<EssayValidationException>(
                                        retrieveEssayByIdTask.AsTask);
        
        //then
        actualEssayValidationException.Should().BeEquivalentTo(expectedEssayValidationException);
        
        _storageBrokerMock.Verify(broker => 
                        broker.SelectEssayByIdAsync(It.IsAny<Guid>()), Times.Once);
        
        _loggingBrokerMock.Verify(broker => 
                        broker.LogError(It.Is(SameExceptionAs(
                                        expectedEssayValidationException))), Times.Once);
        
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _dateTimeBrokerMock.VerifyNoOtherCalls();
    }
}