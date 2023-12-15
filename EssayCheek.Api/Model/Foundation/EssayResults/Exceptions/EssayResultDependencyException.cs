using Xeptions;

namespace EssayCheek.Api.Model.Foundation.EssayResults.Exceptions;

public class EssayResultDependencyException : Xeption
{
    public EssayResultDependencyException(Exception innerException) : 
        base("Essay result dependency error occurred, contact to support.",innerException)
    {
        
    }
}