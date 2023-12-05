using System.Text.Json.Serialization;
using EssayCheek.Api.Model.Essays;

namespace EssayCheek.Api.Model.Foundation.Users;

public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }

    [JsonIgnore] 
    public virtual IEnumerable<Essay>? Essays {get; set; }
    
}