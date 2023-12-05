using Xeptions;

namespace EssayCheek.Api.Model.Foundation.EssayResult.Exception;

public class EssayResultValidationException : Xeption
{
    public EssayResultValidationException(Xeption innerException) :
                    base("Essay result validation occurred, fix it and tyr again",innerException)
    { }
}