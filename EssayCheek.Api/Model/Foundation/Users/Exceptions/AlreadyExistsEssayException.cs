using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Users.Exceptions;

public class AlreadyExistsEssayException : Xeption
{
    public AlreadyExistsEssayException(Exception innerException)
                 :base("User already exits.", innerException) { }
}