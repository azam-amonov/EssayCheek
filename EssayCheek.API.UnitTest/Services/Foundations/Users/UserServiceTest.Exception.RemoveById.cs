using EssayCheek.Api.Model.Foundation.Users;
using EssayCheek.Api.Model.Foundation.Users.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Users;

public partial class UserServiceTest
{
    [Fact]
    public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
    {
        // given
        Guid someUserId = Guid.NewGuid();

        var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();
        
        var lockedUserException = new LockedUserException(databaseUpdateConcurrencyException);

        var expectedUserDependencyValidationException = new UserDependencyValidationException(lockedUserException);

        _storageBrokerMock.Setup(broker => 
                broker.SelectUserByIdAsync(It.IsAny<Guid>()))
                        .ThrowsAsync(databaseUpdateConcurrencyException);
        
        // when
        ValueTask<User> removedUserByIdTask =
                _userService.RetrieveUserByIdAsync(someUserId);

        UserDependencyValidationException actualUserDependencyValidationException =
                await Assert.ThrowsAsync<UserDependencyValidationException>(
                            removedUserByIdTask.AsTask);
        
        // then
        actualUserDependencyValidationException.Should().
                        BeEquivalentTo(expectedUserDependencyValidationException);
        
        _storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                        Times.Once);
        
        _loggingBrokerMock.Verify(broker => broker.LogError(
            It.Is(SameExceptionAs(expectedUserDependencyValidationException)))
                        ,Times.Once);
        
        _storageBrokerMock.Verify(broker => 
                    broker.DeleteUserAsync(It.IsAny<User>()),
                        Times.Never);
        
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _dateTimeBrokerMock.VerifyNoOtherCalls();
    }
}
