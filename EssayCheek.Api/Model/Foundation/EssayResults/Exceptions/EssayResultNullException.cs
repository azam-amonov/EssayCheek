using Xeptions;

namespace EssayCheek.Api.Model.Foundation.EssayResults.Exceptions;

public class EssayResultNullException : Xeption
{
    public EssayResultNullException() : base(message:"Essay Result is null") { }
}