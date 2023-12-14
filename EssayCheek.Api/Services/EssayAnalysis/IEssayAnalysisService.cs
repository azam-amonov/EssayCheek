namespace EssayCheek.Api.Services.EssayAnalysis;

public interface IEssayAnalysisService
{
	public ValueTask<string> EssayAnalysisAsync(string essay);
}