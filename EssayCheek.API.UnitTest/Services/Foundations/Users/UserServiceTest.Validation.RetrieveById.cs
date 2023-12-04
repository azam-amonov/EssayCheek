using EssayCheek.Api.Model.Foundation.Users;
using EssayCheek.Api.Model.Foundation.Users.Exceptions;
using FluentAssertions;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Users;

public partial class UserServiceTest
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnByIdIsInvalidAndLogItAsync()
    {
        // given
        var invalidUserId = Guid.Empty;
        var invalidUserException = new InvalidUserException();
        
        invalidUserException.AddData(
                        key: nameof(User.Id),
                        values:"Id is required");
        
        var expectedUserValidationException = 
                        new UserValidationException(invalidUserException);
        
        // when
        ValueTask<User> retrieveUserByIdTask =
                        _userService.RetrieveUserByIdAsync(invalidUserId);
        
        UserValidationException actualUserValidationException =
            await Assert.ThrowsAsync<UserValidationException>(retrieveUserByIdTask.AsTask);
        
        // then
        actualUserValidationException.Should().BeEquivalentTo(expectedUserValidationException);
        
        _loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                        Times.Once);
        
        _storageBrokerMock.Verify(broker =>
                    broker.SelectUserByIdAsync(It.IsAny<Guid>()), Times.Never);
        
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
        _dateTimeBrokerMock.VerifyNoOtherCalls();
    } 
}