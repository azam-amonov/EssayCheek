using EssayCheek.Api.Model.Foundation.Users;
using FluentAssertions;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Users;

public partial class UserServiceTest
{
    [Fact]
    public void ShouldReturnUsers()
    {
        // given
        IQueryable<User> randomUsers = CreateRandomUsers();
        IQueryable<User> storageUsers = randomUsers;
        IQueryable<User> expectedUsers = storageUsers;

        _storageBrokerMock.Setup(broker 
                        => broker.SelectAllUsers()).Returns(storageUsers);
        // when
        IQueryable<User> actualUsers = _userService.RetrieveAllUsersAsync();
        
        // then
        actualUsers.Should().BeEquivalentTo(expectedUsers);
        
        _storageBrokerMock.Verify(broker =>
                    broker.SelectAllUsers(), Times.Once);
        
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
    }
}