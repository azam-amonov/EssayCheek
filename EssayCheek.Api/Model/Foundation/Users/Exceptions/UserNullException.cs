using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Users.Exceptions;

public class UserNullException : Xeption
{
    public UserNullException(): base(message:"User is null.") { }
}