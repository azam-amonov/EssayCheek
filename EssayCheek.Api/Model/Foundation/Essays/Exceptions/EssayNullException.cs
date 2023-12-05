using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Essays.Exceptions;

public class EssayNullException : Xeption
{
    public EssayNullException(): base("Essay is null")
    { }
}