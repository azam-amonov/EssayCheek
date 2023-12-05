using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Users.Exceptions;

public class NotFoundUserException : Xeption
{
    public NotFoundUserException(Guid userId) :
         base($"Couldn't find user with id {userId}.")
    { }
}