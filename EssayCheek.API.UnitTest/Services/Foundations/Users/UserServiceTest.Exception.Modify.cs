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
    public async Task ShouldThrowDependencyExceptionOnModifyDatabaseUpdateExceptionOccuredAndLogItAsync()
    {
            //given
            User randomUser = CreateRandomUser();
            User someUser = randomUser;
            var databaseUpdateException = new DbUpdateException();

            var failedStorageUserException = 
                        new FailedUserStorageException(databaseUpdateException);

            var expectedUserDependencyException =
                        new UserDependencyException(failedStorageUserException);
            
            _dateTimeBrokerMock.Setup(broker => 
                broker.GetCurrentDateTimeOffset())
                        .Throws(databaseUpdateException);
            
            //when
            ValueTask<User> modifyUserTask = _userService.ModifyUserAsync(someUser);

            UserDependencyException actualUserDependencyException =
                await Assert.ThrowsAsync<UserDependencyException>(
                        modifyUserTask.AsTask);
            
            //then
            actualUserDependencyException.Should().BeEquivalentTo(expectedUserDependencyException);
            
            _loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                        expectedUserDependencyException))),Times.Once);
            
            _dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),Times.Once);
    }
}