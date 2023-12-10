using EssayCheek.Api.Model.Foundation.Essays;
using FluentAssertions;
using Force.DeepCloner;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Essays;

public partial class EssayServiceTest
{
    [Fact]
    public async Task ShouldRetrieveEssayByIdAsync()
    {
        //given
        Guid randomEssayId = Guid.NewGuid();
        Guid inputEssayId = randomEssayId;
        Essay randomEssay = CreateRandomEssay();
        Essay storageEssay = randomEssay;
        Essay expectedEssay = storageEssay.DeepClone();

        _storageBrokerMock.Setup(broker =>
                        broker.SelectEssayByIdAsync(inputEssayId)).ReturnsAsync(storageEssay);
        
        //when
        Essay? actualEssay = await _essayService.RetrieveEssayByIdAsync(inputEssayId);
        
        //then
        actualEssay.Should().BeEquivalentTo(expectedEssay);
        
        _storageBrokerMock.Verify(broker =>
                        broker.SelectEssayByIdAsync(inputEssayId),
                        Times.Once);
        
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _dateTimeBrokerMock.VerifyNoOtherCalls();
    }
}