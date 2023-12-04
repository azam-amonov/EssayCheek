using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Users.Exceptions;

public class UserValidationException : Xeption
{
    public UserValidationException(Xeption innerException) 
            :base( "User validation error occurred, fix the error and tyr again.", innerException)
    { }
}