using EssayCheek.Api.Model.Users;
using EssayCheek.Api.Model.Users.Exceptions;
using FluentAssertions;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Users;

public partial class UserServiceTest
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnAddIfUserIsNullAsync()
    {
        // Given
        User? nullUser = null;
        var nullUserException = new NullUserException();

        var expectedUserValidationException = new UserValidationException(nullUserException);
        
        // When
        ValueTask<User> addUserTask = _userService.AddUserAsync(nullUser);

        UserValidationException actualUserValidationException =
                        await Assert.ThrowsAsync<UserValidationException>(addUserTask.AsTask);
        
        // Then
        actualUserValidationException.Should().BeEquivalentTo(expectedUserValidationException);

        _loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
                        SameExceptionAs(expectedUserValidationException))), Times.Once);
        
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
}