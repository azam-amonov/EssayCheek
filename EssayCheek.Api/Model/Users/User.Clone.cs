namespace EssayCheek.Api.Model.Users;

public partial class User : ICloneable
{
    public object Clone()
    {
        return new User()
        {
            Id = this.Id,
            FirstName = this.FirstName,
            LastName = this.LastName,
            EmailAddress = this.EmailAddress,
            Essays = this.Essays
        };
    }
}