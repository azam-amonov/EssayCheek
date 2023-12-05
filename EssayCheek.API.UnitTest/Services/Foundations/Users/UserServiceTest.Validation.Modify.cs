using EssayCheek.Api.Model.Foundation.Users;
using EssayCheek.Api.Model.Foundation.Users.Exceptions;
using FluentAssertions;
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
                            ,Times.Once);

        _storageBrokerMock.Verify(broker => 
                broker.UpdateUserAsync(It.IsAny<User>()),
                        Times.Never);
        
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
        _dateTimeBrokerMock.VerifyNoOtherCalls();
    }
}