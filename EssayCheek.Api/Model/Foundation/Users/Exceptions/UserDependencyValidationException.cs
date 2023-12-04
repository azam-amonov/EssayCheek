using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Users.Exceptions;

public class UserDependencyValidationException : Xeption
{
    
    // i was
    public UserDependencyValidationException(Exception innerException)
            :base("User dependency validation error occured, fix the error and try again.", innerException)
    { }
}