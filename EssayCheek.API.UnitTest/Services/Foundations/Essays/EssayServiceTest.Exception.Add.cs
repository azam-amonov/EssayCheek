using EssayCheek.Api.Model.Foundation.Essays;
using EssayCheek.Api.Model.Foundation.Essays.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Essays;

public partial class EssayServiceTest
{
    [Fact]
    public async Task ShouldThrowCriticalDependencyExceptionOnAdIfSqlErrorOccursAnfLogItAsync()
    {
        //given
        Essay randomEssay = CreateRandomEssay();
        SqlException sqlException =  CreateSqlException();

        var failedEssayStorageException = 
                        new FailedEssayStorageException(sqlException);

        var expectedEssayStorageException = 
                        new EssayDependencyException(failedEssayStorageException);

        _storageBrokerMock.Setup(broker => 
                        broker.InsertEssayAsync(randomEssay)).ThrowsAsync(sqlException);
        
        //when
        ValueTask<Essay> addEssayTask = _essayService.AddEssayAsync(randomEssay);

        EssayDependencyException actualEssayDependencyException =
                        await Assert.ThrowsAsync<EssayDependencyException>(addEssayTask.AsTask);
        
        //then
        actualEssayDependencyException.Should().BeEquivalentTo(expectedEssayStorageException);
        
        _storageBrokerMock.Verify(broker => 
                        broker.InsertEssayAsync(randomEssay), Times.Once);
        
        _loggingBrokerMock.Verify(broker => 
                        broker.LogCritical(It.Is(SameExceptionAs(
                                        expectedEssayStorageException))),
                        Times.Once);
        
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
    
}


















