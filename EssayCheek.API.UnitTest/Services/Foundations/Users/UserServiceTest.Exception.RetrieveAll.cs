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
}