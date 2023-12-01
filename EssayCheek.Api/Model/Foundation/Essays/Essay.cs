using System.Text.Json.Serialization;
using EssayCheek.Api.Model.Foundation.Users;

namespace EssayCheek.Api.Model.Essays;

public class Essay
{
    public Guid Id { get; set;}
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTimeOffset SubmittedDate { get; set; }
    
    [JsonIgnore]
    public Guid UserId { get; set; }
    
    [JsonIgnore]
    public virtual User User { get; set; }
    
    [JsonIgnore]
    public virtual EssayResult.EssayResult EssayResult { get; set; }


}