using Xeptions;

namespace EssayCheek.Api.Model.Foundation.EssayResults.Exceptions;

public class EssayResultServiceException : Xeption
{
    public EssayResultServiceException(Xeption innerException) : 
        base(message:"Essay service error occurred, contact support.",innerException) 
    {
        
    }
}