using System.Text.Json.Serialization;
using EssayCheek.Api.Model.Foundation.Users;
using EssayCheek.Api.Model.Foundation.EssayResults;

namespace EssayCheek.Api.Model.Foundation.Essays;

public class Essay
{
    public Guid Id { get; set;}
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTimeOffset SubmittedDate { get; set; }
    
    public Guid UserId { get; set; }
    
    [JsonIgnore]
    public virtual User? User { get; set; }
    
    [JsonIgnore]
    public virtual EssayResult? EssayResult { get; set; }


}