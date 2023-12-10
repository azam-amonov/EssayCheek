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
        
        // then
        actualUser.Should().BeEquivalentTo(expectedUser);
        
        _storageBrokerMock.Verify(broker =>
            broker.SelectUserByIdAsync(inputUserId)
                ,Times.Once);
        
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _dateTimeBrokerMock.VerifyNoOtherCalls();
    }
}