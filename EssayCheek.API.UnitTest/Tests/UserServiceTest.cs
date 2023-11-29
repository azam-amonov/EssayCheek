using EssayCheek.Api.Brokers.StorageBroker;
using EssayCheek.Api.Model.Essays;
using EssayCheek.Api.Model.Users;
using EssayCheek.Api.Services.Users;
using Moq;
using Tynamix.ObjectFiller;

namespace EssayCheek.API.UnitTest.Tests;

public partial class UserServiceTest
{
    private readonly Mock<IStorageBroker> _storageBrokerMock;
    private readonly IUserService _userService;

    public UserServiceTest()
    {
        _storageBrokerMock = new Mock<IStorageBroker>();
        _userService = new UserService(storageBroker: _storageBrokerMock.Object);
    }


    private static User CreateRandomUser() => CreateUserFiller().Create();

    private static Filler<User> CreateUserFiller()
    {
        var filler = new Filler<User>();
        filler.Setup().OnType<IEnumerable<Essay>>().IgnoreIt();

        return filler;
    }
}