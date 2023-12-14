using Xeptions;

namespace EssayCheek.Api.Model.Foundation.EssayResults.Exceptions;

public class FailedEssayResultServiceException : Xeption
{
    public FailedEssayResultServiceException(Exception innerException) : 
        base("Failed Essay result exception", innerException) { }
}