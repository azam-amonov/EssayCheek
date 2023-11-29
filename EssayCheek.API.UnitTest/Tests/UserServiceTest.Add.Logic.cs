using EssayCheek.Api.Model.Users;
using FluentAssertions;
using Force.DeepCloner;
using Moq;

namespace EssayCheek.API.UnitTest.Tests;

public partial class UserServiceTest
{
    [Fact]
    public async Task ShouldAddUserAsync()
    {
        // Given
        User randomUser = CreateRandomUser();
        User inputUser = randomUser;
        User persistedUser = inputUser;
        User expectedUser = persistedUser.DeepClone();

        _storageBrokerMock.Setup(broker
                        => broker.InsertUserAsync(inputUser)).ReturnsAsync(persistedUser);
        // When
        User actualUser = await _userService.AddUserAsync(inputUser);
        
        // Then
        actualUser.Should().BeEquivalentTo(expectedUser);
    }
}