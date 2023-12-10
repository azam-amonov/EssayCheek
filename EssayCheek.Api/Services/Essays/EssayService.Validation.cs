using System.Data;
using EssayCheek.Api.Model.Foundation.Essays;
using EssayCheek.Api.Model.Foundation.Essays.Exceptions;

namespace EssayCheek.Api.Services.Essays;

public partial class EssayService
{

    private static void ValidateEssay(Essay essay)
    {
        ValidateEssayIsNotNull(essay);
        Validate((Rule: IsInvalid(essay.Id), Parameter: nameof(essay.Id)),
                        (Rule: IsInvalid(essay.Title), Parameter: nameof(essay.Title)),
                        (Rule: IsInvalid(essay.Content), Parameter: nameof(essay.Content)),
                        (Rule: IsInvalid(essay.SubmittedDate), Parameter: nameof(essay.SubmittedDate)));
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
    
    private static dynamic IsInvalid(DateTimeOffset date) => new
    {
            Condition = date == default,
            Message = "Date is required"
    };

    private static void ValidateEssayIsNotNull(Essay essay)
    {
        if (essay is null)
        {
            throw new EssayNullException();
        }
    }

    private static void ValidateEssayId(Guid id)
    {
        Validate((Rule: IsInvalid(id), Parameter: nameof(Essay.Id)));
    }

    private static void ValidateStorageEssay(Essay maybeEssay, Guid essayId)
    {
        if (maybeEssay is null)
        {
            throw new NotFoundEssayException(essayId);
        }
    }
    
    private static void Validate(params (dynamic Rule, string Parameter)[] validations)
    {
        var invalidEssayException = new InvalidEssayException();

        foreach ((dynamic rule, string parameter) in validations)
        {
            if (rule.Condition)
            {
                invalidEssayException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
            }
        }
        
        invalidEssayException.ThrowIfContainsErrors();
    }
}