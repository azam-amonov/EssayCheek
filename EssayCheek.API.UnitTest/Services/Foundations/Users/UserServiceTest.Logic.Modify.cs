using EssayCheek.Api.Model.Foundation.Users;
using FluentAssertions;
using Force.DeepCloner;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Users;

public partial class UserServiceTest
{
    [Fact]
    public async Task ShouldModifyUserAsync()
    {
        //given
        User randomUser = CreateRandomUser();
        User inputUser = randomUser;
        User storageUser = inputUser;
        User updatedUser = inputUser;
        User expectedUser = updatedUser.DeepClone();
        Guid userId = inputUser.Id;

        _storageBrokerMock.Setup(broker => 
                        broker.SelectUserByIdAsync(userId)).ReturnsAsync(storageUser);

        _storageBrokerMock.Setup(broker => 
                        broker.UpdateUserAsync(inputUser)).ReturnsAsync(updatedUser);

        // when
        User actualUser = await _userService.ModifyUserAsync(inputUser);

        // then
        
        actualUser.Should().BeEquivalentTo(expectedUser);
        
        _storageBrokerMock.Verify(broker => 
                        broker.SelectUserByIdAsync(userId), Times.Once);
        
        _storageBrokerMock.Verify(broker => 
                        broker.UpdateUserAsync(inputUser),Times.Once);
        
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
    }
    
}