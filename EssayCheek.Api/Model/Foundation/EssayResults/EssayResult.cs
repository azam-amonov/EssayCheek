using System.Text.Json.Serialization;
using EssayCheek.Api.Model.Foundation.Essays;

namespace EssayCheek.Api.Model.Foundation.EssayResults;

public class EssayResult
{
    public Guid Id { get; set; }

    public string Feedback { get; set; }
    
    public int Score { get; set; }
    
    public Guid EssayId { get; set; }

    [JsonIgnore] 
    public virtual Essay? Essay {get; set; }
}