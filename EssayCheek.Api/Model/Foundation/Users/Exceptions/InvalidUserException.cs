using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Users.Exceptions;

public class InvalidUserException : Xeption
{
    public InvalidUserException() : 
                    base(message: "Invalid user. Please correct the errors and try again")
    { }
}