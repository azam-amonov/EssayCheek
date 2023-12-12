namespace EssayCheek.Api.Services.EssayAnalysis;

public interface IEssayAnalysisService
{
	public ValueTask<string> AnalyzeEssayAsync(string essay);
}