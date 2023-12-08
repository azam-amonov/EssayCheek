using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Essays.Exceptions;

public class EssayServiceException : Xeption
{
    public EssayServiceException(Xeption innerException) : 
                    base("Essay service error occurred, contact support.", innerException) { }
}