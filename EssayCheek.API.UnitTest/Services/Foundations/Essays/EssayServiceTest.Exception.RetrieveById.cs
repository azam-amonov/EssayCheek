using EssayCheek.Api.Model.Foundation.Essays;
using EssayCheek.Api.Model.Foundation.Essays.Exceptions;
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
                        broker.LogCritical(It.Is(SameExceptionAs(
                            expectedEssayDependencyException))),
                        Times.Once);
        
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _dateTimeBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnRetrieveEssayByIdIfDatabaseUpdateErrorOccursAndLogItAsync()
    {
        //given
        Guid someId = Guid.NewGuid();
        var serviceException = new Exception();

        var failedEssayServiceException =
                        new FailedEssayServiceException(serviceException);

        var expectedEssayServiceException = 
                        new EssayServiceException(failedEssayServiceException);

        _storageBrokerMock.Setup(broker =>
                    broker.SelectEssayByIdAsync(It.IsAny<Guid>()))
                        .ThrowsAsync(serviceException);

        //when
        ValueTask<Essay?> retrievedEssayByIdTask = _essayService.RetrieveEssayByIdAsync(someId);

        EssayServiceException actualEssayServiceException =
                        await Assert.ThrowsAsync<EssayServiceException>(retrievedEssayByIdTask.AsTask);
        
        //then
        actualEssayServiceException.Should().BeEquivalentTo(expectedEssayServiceException);
        
        _storageBrokerMock.Verify(broker =>
                        broker.SelectEssayByIdAsync(It.IsAny<Guid>()),Times.Once);
        
        _loggingBrokerMock.Verify(broker =>
                        broker.LogError(It.Is(SameExceptionAs(
                                        expectedEssayServiceException))),
                        Times.Once);
        
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _dateTimeBrokerMock.VerifyNoOtherCalls();
    }
}