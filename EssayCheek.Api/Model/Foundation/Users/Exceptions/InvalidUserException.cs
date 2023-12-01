using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Users.Exceptions;

public class InvalidUserException : Xeption
{
    public InvalidUserException() : base(message: "User is invalid.")
    { }
}