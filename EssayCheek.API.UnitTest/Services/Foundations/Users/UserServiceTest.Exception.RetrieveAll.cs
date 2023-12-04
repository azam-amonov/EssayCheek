using EssayCheek.Api.Model.Foundation.Users.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Users;

public partial class UserServiceTest
{
    [Fact]
    public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
    {
        // given
        SqlException sqlException = GetSqlException();

        var failedStorageException = new FailedUserStorageException(sqlException);

        var expectedUserDependencyException = new UserDependencyException(failedStorageException);

        _storageBrokerMock.Setup(broker => 
                    broker.SelectAllUsers())
                        .Throws(sqlException);
        
        // when
        Action retrieveAllUsersAction = () =>
                        _userService.RetrieveAllUsers();

        UserDependencyException actualExceptionDependencyException =
                        Assert.Throws<UserDependencyException>(retrieveAllUsersAction);
        
        // then
        actualExceptionDependencyException.Should().BeEquivalentTo(expectedUserDependencyException);
        
        _storageBrokerMock.Verify(broker => broker.SelectAllUsers(), Times.Once);

        _loggingBrokerMock.Verify(broker => 
                        broker.LogCritical(It.Is(SameExceptionAs(
                        expectedUserDependencyException))), Times.Once);
        
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
    {
        //given
        string exceptionsMessage = GteRandomString();
        var serviceException = new Exception();

        var failedUserServiceException =
                        new FailedUserServiceException(serviceException);

        var expectedUserServiceException =
                        new UserServiceException(failedUserServiceException);

        _storageBrokerMock.Setup(broker => broker.SelectAllUsers())
                        .Throws(serviceException);

        // when
        Action retrieveAllUsersAction = () =>
                        _userService.RetrieveAllUsers();

        UserServiceException actualUserServiceException =
                        Assert.Throws<UserServiceException>(retrieveAllUsersAction);

        // then
        actualUserServiceException.Should().BeEquivalentTo(expectedUserServiceException);

        _storageBrokerMock.Verify(broker => broker.SelectAllUsers(), Times.Once);

        _loggingBrokerMock.Verify(broker => 
                    broker.LogError(It.Is(SameExceptionAs(
                        expectedUserServiceException))), 
                            Times.Once);

        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
    }
}