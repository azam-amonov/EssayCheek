using EssayCheek.Api.Model.Foundation.Users;
using EssayCheek.Api.Model.Foundation.Users.Exceptions;
using FluentAssertions;
using Force.DeepCloner;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Users;

public partial class UserServiceTest
{
    [Fact]
    public async Task ShouldRetrieveUserByIdAsync()
    {
        // given
        Guid randomUserId = Guid.NewGuid();
        Guid inputUserId = randomUserId;
        User randomUser = CreateRandomUser();
        User storageUser = randomUser;
        User expectedUser = storageUser.DeepClone();

        _storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(inputUserId))
                        .ReturnsAsync(storageUser);
        
        // when
        User actualUser = await _userService.RetrieveUserByIdAsync(inputUserId);
        
        _storageBrokerMock.Verify(broker =>
            broker.SelectUserByIdAsync(inputUserId)
                ,Times.Once);
        
        // then
        actualUser.Should().BeEquivalentTo(expectedUser);
        
        _storageBrokerMock.Verify(broker =>
            broker.SelectUserByIdAsync(inputUserId)
                ,Times.Once);
        
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _dateTimeBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowFoundExceptionOnRetrieveByIdUserIsNotFoundAndLogItAsync()
    {
        // given
        Guid someUserId = Guid.NewGuid();
        User? noUser = null;

        var notFountUserException = new NotFoundUserException(someUserId);

        var expectedUserValidationException = 
                new UserValidationException(notFountUserException);

        _storageBrokerMock.Setup(broker => 
                broker.SelectUserByIdAsync(It.IsAny<Guid>()))
                        .ReturnsAsync(noUser);

        // when
        ValueTask<User> retrieveUserByIdTask =
                _userService.RetrieveUserByIdAsync(someUserId);

        UserValidationException actualUserValidationException =
                await Assert.ThrowsAsync<UserValidationException>(
                                retrieveUserByIdTask.AsTask);
        
        // then
        actualUserValidationException.Should().BeEquivalentTo(expectedUserValidationException);
        
        _storageBrokerMock.Verify(broker => 
                    broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                        Times.Once());
        
        _loggingBrokerMock.Verify(broker =>
                        broker.LogError(It.Is(SameExceptionAs(
                                        expectedUserValidationException))),
                        Times.Once);
        
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _dateTimeBrokerMock.VerifyNoOtherCalls();
    }
}