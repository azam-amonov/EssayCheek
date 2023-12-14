using EssayCheek.Api.Services.EssayAnalysis.Exceptions;

namespace EssayCheek.Api.Services.EssayAnalysis;

public partial class EssayAnalysisService
{
    private static void ValidateEssayAnalysisIsNotNull(string analysis)
    {
        if (analysis is null)
        {
            throw new NullEssayAnalysisException();
        }
    }
}