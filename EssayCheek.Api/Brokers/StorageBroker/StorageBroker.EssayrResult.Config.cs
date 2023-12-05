using EssayCheek.Api.Model.Foundation.EssayResult;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EssayCheek.Api.Brokers.StorageBroker;

public sealed partial class StorageBroker
{
    private void ConfigEssayResult(EntityTypeBuilder<EssayResult> entity)
    {
        entity.HasOne(result => result.Essay)
                        .WithOne(essay => essay.EssayResult);
    }
}

