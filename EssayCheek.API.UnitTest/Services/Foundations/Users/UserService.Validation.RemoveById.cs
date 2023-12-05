using EssayCheek.Api.Model.Foundation.Users;
using EssayCheek.Api.Model.Foundation.Users.Exceptions;
using FluentAssertions;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Users;

public partial class UserServiceTest
{
    [Fact]
    public async Task ShouldThrowsValidationExceptionOnRemoveIfInvalidAndLogItAsync()
    {
        // given
        Guid invalidUserId = Guid.NewGuid();

        var invalidUserException = new InvalidUserException();

        invalidUserException.AddData(
                        key: nameof(User.Id),
                        values: "Id is required");

        var expectedUserValidationException =
                        new UserValidationException(invalidUserException);

        //when
        ValueTask<User> removedUserByIdTask =
                        _userService.RetrieveUserByIdAsync(invalidUserId);

        UserValidationException actualUserValidationException =
                        await Assert.ThrowsAsync<UserValidationException>(removedUserByIdTask.AsTask);

        //then
        actualUserValidationException.Should().BeEquivalentTo(expectedUserValidationException);

        _loggingBrokerMock.Verify(broker => broker.LogError(
                                        It.Is(SameExceptionAs(expectedUserValidationException))),
                        Times.Once);

        _storageBrokerMock.Verify(broker =>
                                        broker.DeleteUserAsync(It.IsAny<User>()),
                        Times.Never);

        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
        _dateTimeBrokerMock.VerifyNoOtherCalls();

    }
}