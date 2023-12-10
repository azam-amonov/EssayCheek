using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Essays.Exceptions;

public class AlreadyExistsEssayException : Xeption
{
    public AlreadyExistsEssayException(Exception innerException) : 
                    base("Essay already exits.",innerException) { }
}