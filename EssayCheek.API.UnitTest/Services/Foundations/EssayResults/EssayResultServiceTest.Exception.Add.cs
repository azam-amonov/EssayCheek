using EssayCheek.Api.Model.Foundation.EssayResults;
using EssayCheek.Api.Model.Foundation.EssayResults.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.EssayResults;

public partial class EssayResultServiceTest
{
    [Fact]
    public async Task ShouldThrowCriticalDependencyExceptionOnAdIfSqlErrorOccursAndLogItAsync()
    {
    //given
    EssayResult randomEssayResult = CreateRandomEssayResult();
    SqlException sqlException = CreateSqlException();

    var failedEssayResultStorageException = 
        new FailedEssayResultStorageException(sqlException);
    
    var expectedEssayResultStorageException = 
        new EssayResultDependencyException(failedEssayResultStorageException);

    _storageBrokerMock.Setup(broker =>
        broker.InsertEssayResultAsync(randomEssayResult)).ThrowsAsync(sqlException);
    
    //when
    ValueTask<EssayResult> addEssayResultTask = _essayResultService.AddEssayResultsAsync(randomEssayResult);

    EssayResultDependencyException actualEssayResultDependencyException =
        await Assert.ThrowsAsync<EssayResultDependencyException>(addEssayResultTask.AsTask);
    
    //then
    actualEssayResultDependencyException.Should().BeEquivalentTo(expectedEssayResultStorageException);
    
    _storageBrokerMock.Verify(broker =>
        broker.InsertEssayResultAsync(randomEssayResult), 
        Times.Once);
    
    _loggingBrokerMock.Verify(broker =>
        broker.LogCritical(It.Is(SameExceptionAs(
            expectedEssayResultStorageException))),
        Times.Once);
    
    _loggingBrokerMock.VerifyNoOtherCalls();
    _storageBrokerMock.VerifyNoOtherCalls();
    }

}