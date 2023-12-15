using Xeptions;

namespace EssayCheek.Api.Model.Foundation.EssayResults.Exceptions;

public class InvalidEssayResultException : Xeption 
{
    public InvalidEssayResultException() : 
                    base(message:"Invalid essay result. Please fix it and try again") { }
}