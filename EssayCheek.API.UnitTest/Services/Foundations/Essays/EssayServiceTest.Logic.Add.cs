using EssayCheek.Api.Model.Foundation.Essays;
using FluentAssertions;
using Force.DeepCloner;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Essays;

public partial class EssayServiceTest
{
    [Fact]
    public async Task ShouldAddEssayAsync()
    {
        // given
        Essay randomEssay = CreateRandomEssay();
        Essay inputEssay = randomEssay;
        Essay persistedEssay = inputEssay;
        Essay expectedEssay = persistedEssay.DeepClone();

        _storageBrokerMock.Setup(broker =>
                        broker.InsertEssayAsync(inputEssay)).ReturnsAsync(persistedEssay);
        
        // when
        Essay actualEssay = await _essayService.AddEssayAsync(inputEssay);
        
        // then
        actualEssay.Should().BeEquivalentTo(expectedEssay);

        _storageBrokerMock.Verify(broker => 
                        broker.InsertEssayAsync(actualEssay),Times.Once);
        
        _storageBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
    }
}
