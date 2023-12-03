using EssayCheek.Api.Model.Foundation.Users;
using EssayCheek.Api.Model.Foundation.Users.Exceptions;
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

        var expectedUserValidationException =
                        new UserValidationException(nullUserException);
        
        // When
        ValueTask<User> addUserTask = _userService.AddUserAsync(nullUser);

        UserValidationException actualUserValidationException =
                        await Assert.ThrowsAsync<UserValidationException>(addUserTask.AsTask);
        
        // Then
        actualUserValidationException.Should()
                        .BeEquivalentTo(expectedUserValidationException);

        _loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
                        SameExceptionAs(expectedUserValidationException))), Times.Once);
        
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ShouldThrowValidationExceptionOnAddIfUserIsInvalidAndLogAsync(string invalidText)
    {
        // Given
        var invalidUser = new User
        {
            EmailAddress = invalidText
        };
        
        var invalidUserException = new InvalidUserException();
        
        invalidUserException.AddData(
                        key: nameof(User.Id),
                        values: "Id is required");
        
        invalidUserException.AddData(
                        key:nameof(User.FirstName),
                        values: "Text is required");
        
        invalidUserException.AddData(
                        key:nameof(User.LastName),
                        values: "Text is required");
        
        invalidUserException.AddData(
                        key: nameof(User.EmailAddress),
                        values: "Email address is required");
        
        var expectedUserValidationException = 
                        new UserValidationException(invalidUserException);
        
        
        // When
        ValueTask<User> addUserTask = _userService.AddUserAsync(invalidUser);

        UserValidationException actualUserValidationException = 
            await Assert.ThrowsAsync<UserValidationException>(addUserTask.AsTask);

        // Then
    }
}