using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Essays.Exceptions;

public class EssayValidationException: Xeption
{
    public EssayValidationException(Xeption innerException) :
            base("User validation error occurred, fix the error and tyr again.", innerException)
    { }
}