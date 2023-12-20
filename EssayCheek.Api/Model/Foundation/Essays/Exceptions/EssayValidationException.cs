using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Essays.Exceptions;

public class EssayValidationException: Xeption
{
    public EssayValidationException(Xeption innerException) :
            base("Essay validation error occurred, fix the error and try again.", innerException)
    { }
}