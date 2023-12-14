using Xeptions;

namespace EssayCheek.Api.Services.EssayAnalysis.Exceptions;

public class FailedEssayAnalysisServiceException : Xeption
{
    public FailedEssayAnalysisServiceException(Exception innerException) :
        base(message:"Failed Essay Analysis Service Exception",innerException) {}
}