using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Users.Exceptions;

public class UserDependencyException : Xeption
{
    public UserDependencyException(Exception innerException) 
            : base("User dependency error occured, contact support.", innerException)
    { }
}