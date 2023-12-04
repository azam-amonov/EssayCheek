using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Users.Exceptions;

public class UserServiceException : Xeption
{
    public UserServiceException(Xeption innerException) 
                    : base("User service error occurred, contact support.", innerException)
    {
    }
}