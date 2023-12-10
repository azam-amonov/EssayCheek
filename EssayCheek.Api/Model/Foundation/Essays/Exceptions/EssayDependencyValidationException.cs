using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Essays.Exceptions;

public class EssayDependencyValidationException : Xeption
{
    public EssayDependencyValidationException(Exception innerException) : 
                    base("Essay dependency validation exception occurred, fix the error and tyr again", 
                                    innerException) { }
}