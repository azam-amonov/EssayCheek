using System.Xml;
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
        DateTimeOffset randomDate = GetRandomDateTimeOffset();
        User randomUser = CreateRandomModifyUser(randomDate);
        User inputUser = randomUser;
        User storageUser = inputUser.DeepClone();
        storageUser.UpdatedDate = randomUser.CreatedDate;
        User updatedUser = inputUser;
        User expectedUser = updatedUser.DeepClone();
        Guid userId = inputUser.Id;

        _dateTimeBrokerMock.Setup(broker => 
                        broker.GetCurrentDateTimeOffset()).Returns(randomDate);

        _storageBrokerMock.Setup(broker => 
                        broker.SelectUserByIdAsync(userId)).ReturnsAsync(storageUser);

        _storageBrokerMock.Setup(broker => 
                        broker.UpdateUserAsync(inputUser)).ReturnsAsync(updatedUser);

        // when
        User actualUser = await _userService.ModifyUserAsync(inputUser);

        // then
        actualUser.Should().BeEquivalentTo(expectedUser);
        
        _dateTimeBrokerMock.Verify(broker => 
                        broker.GetCurrentDateTimeOffset(),Times.Once);

        _storageBrokerMock.Verify(broker => 
                        broker.SelectUserByIdAsync(userId), Times.Once());
        
        _storageBrokerMock.Verify(broker => 
                        broker.UpdateUserAsync(inputUser),Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
    }
    
}