using EssayCheek.Api.Model.Foundation.Users;
using EssayCheek.Api.Model.Foundation.Users.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit.Sdk;

namespace EssayCheek.API.UnitTest.Services.Foundations.Users;

public partial class UserServiceTest
{
    [Fact]
    public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlOccursAndItAsync()
    {
        //given
        User randomUser = CreateRandomUser();
        User someUser = randomUser;
        Guid userId = someUser.Id;
        SqlException sqlException = CreateSqlException();

        var failedUserStorageException = 
                new FailedUserStorageException(sqlException);

        var expectedUserDependencyException = 
                new UserDependencyException(failedUserStorageException);

        _storageBrokerMock.Setup(broker => 
                broker.SelectUserByIdAsync(userId)).ReturnsAsync(someUser);
        
        _storageBrokerMock.Setup(broker => 
                broker.UpdateUserAsync(someUser)).ThrowsAsync(sqlException);
        
        //when
        ValueTask<User> modifiedUserTask =
                _userService.ModifyUserAsync(randomUser);

        UserDependencyException actualUserDependencyException =
                await Assert.ThrowsAsync<UserDependencyException>(modifiedUserTask.AsTask);

        //then
        actualUserDependencyException.Should().BeEquivalentTo(expectedUserDependencyException);
        
        _storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(userId), Times.Once);
        
        _storageBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(someUser), Times.Once);
        
        _loggingBrokerMock.Verify(broker => 
                broker.LogCritical(It.Is(
                        SameExceptionAs(expectedUserDependencyException))),Times.Once);

        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnUpdateIfServiceErrorOccursAndLogItAsync()
    {
            //given
            User randomUser = CreateRandomUser();
            var serviceException = new Exception();

            var failureUserServiceException = new FailedUserServiceException(serviceException);

            var expectedUserServiceException = new UserServiceException(failureUserServiceException);

            _storageBrokerMock.Setup(broker => 
                        broker.SelectUserByIdAsync(randomUser.Id)).ReturnsAsync(randomUser);

            _storageBrokerMock.Setup(broker => 
                        broker.UpdateUserAsync(randomUser)).ThrowsAsync(serviceException);
            
            //when
            ValueTask<User> updatedUserTask = _userService.ModifyUserAsync(randomUser);

            UserServiceException actualUserServiceException =
                        await Assert.ThrowsAsync<UserServiceException>(updatedUserTask.AsTask);
            
            //then
            actualUserServiceException.Should().BeEquivalentTo(expectedUserServiceException);
            
            _storageBrokerMock.Verify(broker => 
                        broker.SelectUserByIdAsync(randomUser.Id), Times.Once);
            
            _storageBrokerMock.Verify(broker => 
                        broker.UpdateUserAsync(randomUser), Times.Once);
            
            _loggingBrokerMock.Verify(broker => 
                        broker.LogError(It.Is(SameExceptionAs(
                                expectedUserServiceException))),
                                Times.Once);
            
            _storageBrokerMock.VerifyNoOtherCalls();
            _loggingBrokerMock.VerifyNoOtherCalls();
    }
}