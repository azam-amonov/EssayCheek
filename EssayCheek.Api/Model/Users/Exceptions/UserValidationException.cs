using Xeptions;

namespace EssayCheek.Api.Model.Users.Exceptions;

public class UserValidationException : Xeption
{
    public UserValidationException(Xeption innerException) 
                    :base( "User validation error occurred, fix the error and tyr again.", innerException)
    {
        
    }
}