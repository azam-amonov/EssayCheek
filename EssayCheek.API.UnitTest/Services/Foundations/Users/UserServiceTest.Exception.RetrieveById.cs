using EssayCheek.Api.Model.Foundation.Users;
using EssayCheek.Api.Model.Foundation.Users.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Users;

public partial class UserServiceTest
{
    [Fact]
    public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
    {
        // given
        Guid someId = Guid.NewGuid();
        SqlException sqlException = GetSqlException();

        var failedUserStorageException = 
                new FailedUserStorageException(sqlException);

        var expectedUserDependencyException = 
                new UserDependencyException(failedUserStorageException);

        _storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>())).ThrowsAsync(sqlException);
        
        // when
        ValueTask<User> retrieveUserByIdTask =
                        _userService.RetrieveUserByIdAsync(someId);

        UserDependencyException actualUserDependencyException =
            await Assert.ThrowsAsync<UserDependencyException>(
                retrieveUserByIdTask.AsTask);
        
        // then
        actualUserDependencyException.Should().BeEquivalentTo(expectedUserDependencyException);
        
        _storageBrokerMock.Verify(broker => 
                broker.SelectUserByIdAsync(It.IsAny<Guid>()), Times.Once);
        
        _loggingBrokerMock.Verify(broker => 
                broker.LogCritical(It.Is(SameExceptionAs(
                        expectedUserDependencyException))), 
                                Times.Once);
        
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _dateTimeBrokerMock.VerifyNoOtherCalls();

    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfDatabaseUpdateErrorOccursAndLogAsync()
    {
            // given
            Guid someId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedUserServiceException = new FailedUserServiceException(serviceException);

            var expectedUserServiceException = new UserServiceException(failedUserServiceException);

            _storageBrokerMock.Setup(broker => 
                broker.SelectUserByIdAsync(It.IsAny<Guid>()))
                        .ThrowsAsync(serviceException);
            
            // when
            ValueTask<User> retrieveUserByIdTask = _userService.RetrieveUserByIdAsync(someId);

            UserServiceException actualUserServiceException =
                await Assert.ThrowsAsync<UserServiceException>(retrieveUserByIdTask.AsTask);
            
            // then
            actualUserServiceException.Should().BeEquivalentTo(expectedUserServiceException);
            
            _storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()), Times.Once);
            
            _loggingBrokerMock.Verify( broker => 
                    broker.LogError(It.Is(SameExceptionAs(
                                expectedUserServiceException))), Times.Once );
            
            _storageBrokerMock.VerifyNoOtherCalls();
            _loggingBrokerMock.VerifyNoOtherCalls();
            _dateTimeBrokerMock.VerifyNoOtherCalls();
            
    }
    
}