using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Essays.Exceptions;

public class InvalidEssayException: Xeption 
{
    public InvalidEssayException() : 
                    base(message: "Invalid essay. Please correct the errors and try again") 
    { }
}