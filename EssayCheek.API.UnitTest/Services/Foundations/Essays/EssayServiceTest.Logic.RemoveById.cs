using EssayCheek.Api.Model.Foundation.Essays;
using FluentAssertions;
using Force.DeepCloner;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Essays;

public partial class EssayServiceTest
{
    [Fact]
    public async Task ShouldRemoveEssayByIdAsync()
    {
        //given
        Guid essayId = Guid.NewGuid();
        Guid inputEssayId = essayId;
        Essay randomEssay = CreateRandomEssay();
        Essay expectedInputEssay = randomEssay;
        Essay expectedEssay = expectedInputEssay.DeepClone();

        _storageBrokerMock.Setup(broker =>
                        broker.SelectEssayByIdAsync(inputEssayId))
                        .ReturnsAsync(randomEssay);

        _storageBrokerMock.Setup(broker =>
                    broker.DeleteEssayAsync(expectedInputEssay))
                        .ReturnsAsync(expectedInputEssay);
        
        //when
        Essay actualEssay = await _essayService.RemoveEssayByIdAsync(inputEssayId);
        
        //then
        actualEssay.Should().BeEquivalentTo(expectedEssay);
        
        _storageBrokerMock.Verify(broker =>
                        broker.SelectEssayByIdAsync(inputEssayId),
                        Times.Once);
        
        _storageBrokerMock.Verify(broker => 
                        broker.DeleteEssayAsync(expectedInputEssay),
                        Times.Once);
        
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _dateTimeBrokerMock.VerifyNoOtherCalls();
    }
}