using Xeptions;

namespace EssayCheek.Api.Services.EssayAnalysis.Exceptions;

public class NullEssayAnalysisException : Xeption
{
    public NullEssayAnalysisException() : base(message:"EssayAnalysis is null.") { }
}