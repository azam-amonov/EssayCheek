using EssayCheek.Api.Model.Foundation.Essays;
using EssayCheek.Api.Model.Foundation.Essays.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Essays;

public partial class EssayServiceTest
{
    [Fact]
    public async Task ShouldThrowDependencyValidationExceptionOnRemoveByIdIfDbUpdateConcurrencyErrorOccursAndLogItAsync()
    {
        //given
        Guid someEssayId = Guid.NewGuid();

        var databaseUpdateConcurrencyException =
                        new DbUpdateConcurrencyException();

        var lockedEssayException =
                        new LockedEssayException(databaseUpdateConcurrencyException);

        var expectedEssayDependencyValidationException =
                        new EssayDependencyValidationException(lockedEssayException);

        _storageBrokerMock.Setup(broker =>
                        broker.SelectEssayByIdAsync(It.IsAny<Guid>()))
                            .ThrowsAsync(databaseUpdateConcurrencyException);
        
        //when
        ValueTask<Essay> removeEssayByIdTask =
                        _essayService.RemoveEssayByIdAsync(someEssayId);

        EssayDependencyValidationException actualEssayDependencyValidationException =
                        await Assert.ThrowsAsync<EssayDependencyValidationException>(
                                        removeEssayByIdTask.AsTask);
        
        //then
        actualEssayDependencyValidationException.Should().
                        BeEquivalentTo(expectedEssayDependencyValidationException);
        
        _storageBrokerMock.Verify(broker =>
                        broker.SelectEssayByIdAsync(It.IsAny<Guid>()),
                        Times.Once);
        
        _loggingBrokerMock.Verify(broker =>
                        broker.LogError(It.Is(SameExceptionAs(
                                        expectedEssayDependencyValidationException))),
                        Times.Once);
        
        _storageBrokerMock.Verify(broker =>
                        broker.DeleteEssayAsync(It.IsAny<Essay>()),
                        Times.Never);
        
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _dateTimeBrokerMock.VerifyNoOtherCalls();
    }
}