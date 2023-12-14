using Xeptions;

namespace EssayCheek.Api.Model.Foundation.EssayResults.Exceptions;

public class FailedEssayResultStorageException : Xeption
{
    public FailedEssayResultStorageException(Exception innerException) :
        base("Failed storage essay result error occured, contact support.", innerException) { }
}