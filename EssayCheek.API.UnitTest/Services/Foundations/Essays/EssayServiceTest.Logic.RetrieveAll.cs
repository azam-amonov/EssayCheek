using EssayCheek.Api.Model.Foundation.Essays;
using FluentAssertions;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Essays;

public partial class EssayServiceTest
{
    [Fact]
    public void ShouldReturnEssays()
    {
        //given
        IQueryable<Essay> randomEssays = CreateRandomEssays();
        IQueryable<Essay> storageEssays = randomEssays;
        IQueryable<Essay> expectedEssays = storageEssays;

        _storageBrokerMock.Setup(broker =>
                        broker.SelectAllEssays()).Returns(storageEssays);
        
        //when
        IQueryable<Essay> actualEssays = _essayService.RetrieveAllEssays();
        
        //then
        actualEssays.Should().BeEquivalentTo(expectedEssays);
        
        _storageBrokerMock.Verify(broker =>
                        broker.SelectAllEssays(), Times.Once);
        
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();

    }
}