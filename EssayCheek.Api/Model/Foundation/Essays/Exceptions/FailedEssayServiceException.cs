using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Essays.Exceptions;

public class FailedEssayServiceException : Xeption
{
    public FailedEssayServiceException(Exception innerException): 
                    base("Failed Essay Service Exception",innerException) { }
}