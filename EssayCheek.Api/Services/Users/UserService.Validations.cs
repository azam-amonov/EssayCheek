using EssayCheek.Api.Model.Foundation.Users;
using EssayCheek.Api.Model.Foundation.Users.Exceptions;

namespace EssayCheek.Api.Services.Users;

public partial class UserService
{
    private static void ValidateUser(User user)
    {
        ValidateUserIsNotNull(user);
        Validate((Rule: IsInvalid(user.Id), Parameter: nameof(User.Id)),
                (Rule: IsInvalid(user.FirstName), Parameter: nameof(User.FirstName)),
                (Rule: IsInvalid(user.LastName), Parameter: nameof(User.LastName)),
                (Rule: IsInvalid(user.EmailAddress), Parameter: nameof(User.EmailAddress)));
       
    }

    private static void ValidateUserIsNotNull(User user)
    {
        if (user is null)
        {
            throw new UserNullException();
        }
    }

    private static dynamic IsInvalid(Guid id) => new
    {
        Comdition = id == Guid.Empty,
        Message = "Id is required."
    };
    
    private static dynamic IsInvalid(string text) => new
    {
        Condition = string.IsNullOrWhiteSpace(text),
        Message = "Text is required."
    };
    
    private static void Validate(params (dynamic Rule, string Parameter)[] validations)
    {
        var invalidUserException = new InvalidUserException();

        foreach ((dynamic rule, string parameter)in validations)
        {
            if (rule.Condition)
            {
                invalidUserException.UpsertDataList(
                    key: parameter,
                    value: rule.Message);
            }
        }
        
        invalidUserException.ThrowIfContainsErrors();
    }
}