using EssayCheek.Api.Model.Foundation.Users;
using EssayCheek.Api.Model.Foundation.Users.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Users;

public partial class UserServiceTest
{
    [Fact]
    public async Task ShouldThrowsCriticalDependencyExceptionOnModifyIfSqlOccursAndItAsync()
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
}