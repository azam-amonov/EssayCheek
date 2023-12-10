using EssayCheek.Api.Model.Foundation.Essays.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Essays;

public partial class EssayServiceTest
{
    [Fact]
    public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
    {
        //given
        SqlException sqlException = CreateSqlException();

        var failedStorageException = 
                        new FailedEssayStorageException(sqlException);

        var expectedEssayDependencyException = 
                        new EssayDependencyException(failedStorageException);

        _storageBrokerMock.Setup(broker =>
                        broker.SelectAllEssays()).Throws(sqlException);
        
        //when
        Action retrieveAllEssayAction = () =>
                        _essayService.RetrieveAllEssays();

        EssayDependencyException actualEssayDependencyException =
                        Assert.Throws<EssayDependencyException>(retrieveAllEssayAction);
        
        //then
        actualEssayDependencyException.Should().BeEquivalentTo(expectedEssayDependencyException);
        
        _storageBrokerMock.Verify(broker =>
                        broker.SelectAllEssays(), Times.Once);
        
        _loggingBrokerMock.Verify(broker =>
                        broker.LogCritical(It.Is(SameExceptionAs(
                                        expectedEssayDependencyException))),
                        Times.Once);
        
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldThrowServiceExceptionOnRetrieveAllServiceErrorOccursAndLogIt()
    {
        //given
        string exceptionMessage = GetRandomString();
        var serviceException = new Exception();

        var failedEssayServiceException =
                        new FailedEssayServiceException(serviceException);

        var expectedEssayServiceException =
                        new EssayServiceException(failedEssayServiceException);

        _storageBrokerMock.Setup(broker =>
                        broker.SelectAllEssays()).Throws(serviceException);
        
        //when
        Action retrieveAllEssaysAction = () => _essayService.RetrieveAllEssays();

        EssayServiceException actualEssayServiceException =
                        Assert.Throws<EssayServiceException>(retrieveAllEssaysAction);
        
        //then
        actualEssayServiceException.Should().BeEquivalentTo(expectedEssayServiceException);
        
        _storageBrokerMock.Verify(broker => broker.SelectAllEssays(), Times.Once);
        
        _loggingBrokerMock.Verify(broker => broker.LogError(It.Is(SameExceptionAs(
                        expectedEssayServiceException))),
                        Times.Once);
        
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
    }
    
}