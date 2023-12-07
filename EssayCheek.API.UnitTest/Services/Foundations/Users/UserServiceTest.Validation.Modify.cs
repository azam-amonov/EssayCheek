using EssayCheek.Api.Model.Foundation.Users;
using EssayCheek.Api.Model.Foundation.Users.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Users;

public partial class UserServiceTest
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyIfUserNullAndLogItAsync()
    {
        // given
        User nullUser = null;
        var nullUserException = new UserNullException();

        var expectedUserValidationException = 
                        new UserValidationException(nullUserException);

        // when
        ValueTask<User> modifyUserTask = 
                        _userService.ModifyUserAsync(nullUser);

        UserValidationException actualUserValidationException = 
                        await Assert.ThrowsAsync<UserValidationException>(modifyUserTask.AsTask);

        // then
        actualUserValidationException.Should().BeEquivalentTo(expectedUserValidationException);

        _loggingBrokerMock.Verify(broker =>
                    broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException)))
                        , Times.Once);

        _storageBrokerMock.Verify(broker =>
                    broker.UpdateUserAsync(It.IsAny<User>()),
                        Times.Never);

        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
        _dateTimeBrokerMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task ShouldThrowModificationExceptionOnModifyUseIsInvalidAndLogItAsync(string invalidString)
    {
        //given
        User invalidUser = new User
        {
                FirstName = invalidString,
                LastName = invalidString,
                EmailAddress = invalidString
        };

        var invalidUserException = new InvalidUserException();

        invalidUserException.AddData(
                        key: nameof(User.Id),
                        values: "Id is required");

        invalidUserException.AddData(
                        key: nameof(User.FirstName),
                        values: "Text is required");

        invalidUserException.AddData(
                        key: nameof(User.LastName),
                        values: "Text is required");

        invalidUserException.AddData(
                        key: nameof(User.EmailAddress),
                        values: "Text is required");

        var expectedUserValidationException =
                        new UserValidationException(invalidUserException);

        //when
        ValueTask<User> modifyUserTask = _userService.ModifyUserAsync(invalidUser);

        UserValidationException actualUserValidationException =
                        await Assert.ThrowsAnyAsync<UserValidationException>(modifyUserTask.AsTask);

        //then
        actualUserValidationException.Should().BeEquivalentTo(expectedUserValidationException);

        _loggingBrokerMock.Verify(broker =>
                        broker.LogError(It.Is(SameExceptionAs(
                                        expectedUserValidationException))), Times.Once);

        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
    }

    // there should be tests as 
    // before create this test should add date type in to user

    // [Fact] 
    // public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatesDataAndLogItAsync()

    // [Fact]
    // public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateNotSameAsUpdateDataAndLogItAsync()
}