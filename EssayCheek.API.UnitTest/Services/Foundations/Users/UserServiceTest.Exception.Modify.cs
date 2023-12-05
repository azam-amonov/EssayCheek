using EssayCheek.Api.Model.Foundation.Users;
using EssayCheek.Api.Model.Foundation.Users.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Users;

public partial class UserServiceTest
{
    [Fact]
    public async Task ShouldThrowsCriticalDependencyExceptionOnModifyIdSqlOccursAndItAsync()
    {
        //given
        User randomUser = CreateRandomUser();
        User someUser = randomUser;
        Guid userId = someUser.Id;
        SqlException sqlException = CreateSqlException();

        var failedUserStorageException = new FailedUserStorageException(sqlException);

        var expectedUserDependencyException = new UserDependencyException(failedUserStorageException);

        _dateTimeBrokerMock.Setup(broker => broker.GetCurrentDateTimeOffset()).Throws(sqlException);
        
        //when
        ValueTask<User> modifiedUserTask = _userService.ModifyUserAsync(someUser);

        UserDependencyException actualUserDependencyException =
                        await Assert.ThrowsAsync<UserDependencyException>(modifiedUserTask.AsTask);

        //then
        actualUserDependencyException.Should().BeEquivalentTo(expectedUserDependencyException);
        
        _storageBrokerMock.Verify(broker =>
                        broker.SelectUserByIdAsync(userId), Times.Never);
        
        _storageBrokerMock.Verify(broker =>
                        broker.UpdateUserAsync(someUser), Times.Never);
        
        _loggingBrokerMock.Verify(broker => 
                        broker.LogCritical(It.Is(
                        SameExceptionAs(expectedUserDependencyException))),Times.Once);

        _dateTimeBrokerMock.Verify(broker =>
                        broker.GetCurrentDateTimeOffset(), Times.Once);
    }
}