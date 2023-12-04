using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Users.Exceptions;

public class FailedUserServiceException : Xeption
{
    public FailedUserServiceException(Exception innerException)
            :base("Failed User Service Exception", innerException)
    {
        
    }
}