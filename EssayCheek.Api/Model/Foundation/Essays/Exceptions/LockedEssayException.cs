using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Essays.Exceptions;

public class LockedEssayException : Xeption
{
    public LockedEssayException(Exception innerException) : 
                    base("Locked essay record exception, please tyr again later", innerException) { }
}