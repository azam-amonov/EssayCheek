using EssayCheek.Api.Model.Foundation.Users;
using FluentAssertions;
using Force.DeepCloner;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Users;

public partial class UserServiceTest
{
    [Fact]
    public async Task ShouldRemoveUserByIdAsync()
    {
        // given
        Guid userId = Guid.NewGuid();
        Guid inputUserId = userId;
        User randomUser = CreateRandomUser();
        User storageUser = randomUser;
        User expectedInputUser = storageUser;
        User deletedUser = expectedInputUser;
        User expectedUser = deletedUser.DeepClone();

        _storageBrokerMock.Setup(broker => 
                broker.SelectUserByIdAsync(inputUserId))
                        .ReturnsAsync(storageUser);

        _storageBrokerMock.Setup(broker =>
                broker.DeleteUserAsync(expectedInputUser))
                        .ReturnsAsync(deletedUser);

        // when
        User actualUser = await _userService.RemoveUserByIdAsync(inputUserId);

        // then
        actualUser.Should().BeEquivalentTo(expectedUser);
        
        _storageBrokerMock.Verify(broker 
                    => broker.SelectUserByIdAsync(inputUserId)
                        ,Times.Once);
        
        _storageBrokerMock.Verify(broker => 
                    broker.DeleteUserAsync(expectedInputUser),
                        Times.Once);
        
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _dateTimeBrokerMock.VerifyNoOtherCalls();
    }
}