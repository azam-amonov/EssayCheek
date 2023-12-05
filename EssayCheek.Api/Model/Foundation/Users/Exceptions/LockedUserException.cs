using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Users.Exceptions;

public class LockedUserException : Xeption
{
    public LockedUserException(Exception innerException) : 
                    base(message:"Locked user record exception, please tyr again later", innerException)
    {
        
    }
}