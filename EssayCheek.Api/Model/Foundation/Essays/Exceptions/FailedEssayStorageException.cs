using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Essays.Exceptions;

public class FailedEssayStorageException : Xeption 
{
    public FailedEssayStorageException(Exception innerException) : 
                    base("Failed storage essay error occured, contact support.",innerException) { }
}