using System.Data;
using EssayCheek.Api.Model.Foundation.Essays;
using EssayCheek.Api.Model.Foundation.Essays.Exceptions;
using Microsoft.Extensions.Options;

namespace EssayCheek.Api.Services.Essays;

public partial class EssayService
{

    private static void ValidateEssay(Essay essay)
    {
        ValidateEssayIsNotNull(essay);
        Validate((Rule: IsInvalid(essay.Id), Parameter: nameof(essay.Id)),
                        (Ruel: IsInvalid(essay.Title), Parameter: nameof(essay.Title)),
                        (Ruel: IsInvalid(essay.Content), Parameter: nameof(essay.Content)));
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

    private static void ValidateEssayIsNotNull(Essay essay)
    {
        if (essay is null)
        {
            throw new EssayNullException();
        }
    }
    
    private static void Validate(params (dynamic Ruel, string Parameter)[] validations)
    {
        var invalidEssayException = new InvalidEssayException();

        foreach ((dynamic rule, string parameter) in validations)
        {
            if (rule.Condition)
            {
                invalidEssayException.UpsertDataList(
                        key: parameter,
                        value: rule.Condition);
            }
        }
    }
}