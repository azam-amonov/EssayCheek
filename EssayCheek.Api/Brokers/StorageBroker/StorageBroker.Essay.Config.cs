using EssayCheek.Api.Model.Essays;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EssayCheek.Api.Brokers.StorageBroker;

public partial class StorageBroker
{
    public void ConfigureEssay(EntityTypeBuilder<Essay> entity)
    {
        entity.HasOne(essay => essay.EssayResult)
                        .WithOne(result => result.Essay);
    }
}