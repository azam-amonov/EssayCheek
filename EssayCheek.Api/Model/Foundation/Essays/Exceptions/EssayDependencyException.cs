using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Essays.Exceptions;

public class EssayDependencyException : Xeption
{
    public EssayDependencyException(Exception innerException) : 
                    base("Essay dependency error occurred, contact to support.",innerException) { }
}