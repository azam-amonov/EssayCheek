using System.Text.Json.Serialization;

namespace EssayCheek.Api.Model.Essays;

public class Essay
{
    public Guid Id { get; set;}
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTimeOffset SubmittedDate { get; set; }
    
    // public 
    // [JsonIgnore]
    // public virtual EssayResult EssayResult { get; set; }
}