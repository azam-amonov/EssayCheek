using EssayCheek.Api.Model.EssayResult;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EssayCheek.Api.Brokers.StorageBroker;

public partial class StorageBroker
{
    public void ConfigEssayResult(EntityTypeBuilder<EssayResult> entity)
    {
        entity.HasOne(result => result.Essay)
                        .WithOne(essay => essay.EssayResult);
    }
}

