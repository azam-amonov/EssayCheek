using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Essays.Exceptions;

public class NotFoundEssayException : Xeption
{
    public NotFoundEssayException(Guid essayId) : base(message:$"Couldn't find user with id {essayId}.") { }
}