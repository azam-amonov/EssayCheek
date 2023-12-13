using Xeptions;

namespace EssayCheek.Api.Services.EssayAnalysis.Exceptions;

public class EssayAnalysisValidationException : Xeption
{
    public EssayAnalysisValidationException(Xeption innerException) :
        base(message: "Essay validation error occurred, fix the error and tyr again.",
            innerException) { }
}