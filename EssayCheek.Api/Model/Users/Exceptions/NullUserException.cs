using Xeptions;

namespace EssayCheek.Api.Model.Users.Exceptions;

public class NullUserException : Xeption
{
    public NullUserException(): base(message:"User is null.")
    {
        
    }
}