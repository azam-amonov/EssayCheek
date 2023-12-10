using EssayCheek.Api.Model.Foundation.Essays;
using EssayCheek.Api.Model.Foundation.Essays.Exceptions;
using EssayCheek.Api.Model.Foundation.Users.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Essays;

public partial class EssayServiceTest
{
    [Fact]
    public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
    {
        //given
        Guid someId = Guid.NewGuid();
        SqlException sqlException = CreateSqlException();

        var failedEssayStorageException =
                        new FailedEssayStorageException(sqlException);

        var expectedEssayDependencyException =
                        new EssayDependencyException(failedEssayStorageException);

        _storageBrokerMock.Setup(broker =>
                        broker.SelectEssayByIdAsync(It.IsAny<Guid>())).ThrowsAsync(sqlException);
        
        //when

        ValueTask<Essay?> retrieveEssayByIdTask = _essayService.RetrieveEssayByIdAsync(someId);

        EssayDependencyException actualEssayDependencyException =
                        await Assert.ThrowsAsync<EssayDependencyException>(retrieveEssayByIdTask.AsTask);
        
        //then
        actualEssayDependencyException.Should().BeEquivalentTo(expectedEssayDependencyException);

        _storageBrokerMock.Verify(broker =>
                        broker.SelectEssayByIdAsync(It.IsAny<Guid>()), Times.Once);

        _loggingBrokerMock.Verify(broker =>
                        broker.LogError(It.Is(SameExceptionAs(
                            expectedEssayDependencyException))),
                        Times.Once);
        
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _dateTimeBrokerMock.VerifyNoOtherCalls();
    }
}