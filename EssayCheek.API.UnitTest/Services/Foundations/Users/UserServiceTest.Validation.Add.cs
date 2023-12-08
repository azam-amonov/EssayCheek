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
        var nullUserException = new UserNullException();

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
        var invalidUser = new User()
        {
                FirstName = invalidText,
                LastName = invalidText,
                EmailAddress = invalidText
        };
        
        var expectedInvalidUserException = new InvalidUserException();
        
        expectedInvalidUserException.AddData(
                        key: nameof(User.Id),
                        values: "Id is required");
        
        expectedInvalidUserException.AddData(
                        key:nameof(User.FirstName),
                        values: "Text is required");
        
        expectedInvalidUserException.AddData(
                        key:nameof(User.LastName),
                        values: "Text is required");
        
        expectedInvalidUserException.AddData(
                        key: nameof(User.EmailAddress),
                        values: "Text is required");
        
        var expectedUserValidationException = 
                        new UserValidationException(expectedInvalidUserException);
        
        
        // When
        ValueTask<User> addUserTask = _userService.AddUserAsync(invalidUser);

        UserValidationException actualUserValidationException = 
            await Assert.ThrowsAsync<UserValidationException>(addUserTask.AsTask);

        
        // Then
        actualUserValidationException.Should().BeEquivalentTo(expectedUserValidationException);

        _loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),Times.Once);

        _storageBrokerMock.Verify(broker => 
                broker.InsertUserAsync(invalidUser),Times.Never);
        
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
    }
    
    // then here should be some unit tests for date types as
    // 
    //  [Fact]
    //  public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDateIsNotSameAndAndLogItAsync()
    // 
    //
    //  [Theory]
    //  [MemberData(nameof(MinutesBeforeOrAfter)]
    //  public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(int minutesBeforeOrAfter);
    // 
}