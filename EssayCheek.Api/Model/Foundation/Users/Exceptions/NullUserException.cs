using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Users.Exceptions;

public class NullUserException : Xeption
{
    public NullUserException(): base(message:"User is null.")
    {
        
    }
}