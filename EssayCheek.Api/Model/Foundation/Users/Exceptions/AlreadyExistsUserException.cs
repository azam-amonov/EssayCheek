using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Users.Exceptions;

public class AlreadyExistsUserException : Xeption
{
    public AlreadyExistsUserException(Exception innerException)
                 :base("User already exits.", innerException) { }
}