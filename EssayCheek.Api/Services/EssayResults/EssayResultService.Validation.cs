using EssayCheek.Api.Model.Foundation.EssayResults;
using EssayCheek.Api.Model.Foundation.EssayResults.Exceptions;

namespace EssayCheek.Api.Services.EssayResults;

public partial class EssayResultService
{
    private static void ValidateEssayResult(EssayResult essayResult )
    {
        ValidateEssayResultIsNotNull(essayResult);
        Validate((Rule: IsInvalid(essayResult.Id), Parameter: nameof(essayResult.Id)),
                        (Rule: IsInvalid(essayResult.Feedback), Parameter: nameof(essayResult.Feedback)),
                        (Rule: IsInvalid(essayResult.Score), Parameter: nameof(essayResult.Score)));
    }

    private static dynamic IsInvalid(Guid id) => new
    {
        Condition = id == Guid.Empty,
        Message = "Id is required"
    };
    private static dynamic IsInvalid(string text) => new
    {
        Condition = string.IsNullOrWhiteSpace(text),
        Message = "Text is required"
    };
    
    private static dynamic IsInvalid(int score) => new
    {
        Condition = score == -1,
        Message = "Score is required"
    };

    private static void ValidateEssayResultIsNotNull(EssayResult essayResult)
    {
        if (essayResult is null)
        {
            throw new EssayResultNullException();
        }
    }
    
    private static void Validate(params (dynamic Rule, string Parameter)[] validations)
    {
        var invalidEssayResultException = new InvalidEssayResultException();

        foreach ((dynamic rule, string parameter) in validations)
        {
            if (rule.Condition)
            {
                invalidEssayResultException.UpsertDataList(
                    key: parameter,
                    value: rule.Message);
            }
        }
        invalidEssayResultException.ThrowIfContainsErrors();
    }  
}