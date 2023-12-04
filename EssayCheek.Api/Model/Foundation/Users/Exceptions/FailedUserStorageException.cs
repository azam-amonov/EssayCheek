using Xeptions;

namespace EssayCheek.Api.Model.Foundation.Users.Exceptions;

public class FailedUserStorageException : Xeption
{
   public FailedUserStorageException(Exception innerException) 
        : base("Failed user storage error occured, contact support.", innerException)
    { }
}