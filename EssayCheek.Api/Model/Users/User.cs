using System.Text.Json.Serialization;

namespace EssayCheek.Api.Model.Users;

public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }

    // [JsonIgnore] 
    // public virtual IEnumerable<Essay> Essays {get; set; }
}