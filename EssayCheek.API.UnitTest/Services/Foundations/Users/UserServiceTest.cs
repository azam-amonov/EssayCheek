using System.Linq.Expressions;
using EssayCheek.Api.Brokers.Logging;
using EssayCheek.Api.Brokers.StorageBroker;
using EssayCheek.Api.Model.Essays;
using EssayCheek.Api.Model.Users;
using EssayCheek.Api.Services.Users;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace EssayCheek.API.UnitTest.Services.Foundations.Users;

public partial class UserServiceTest
{
    private readonly Mock<IStorageBroker> _storageBrokerMock;
    private readonly Mock<ILoggingBroker> _loggingBrokerMock;
    private readonly IUserService _userService;

    public UserServiceTest()
    {
        _storageBrokerMock = new Mock<IStorageBroker>();
        _loggingBrokerMock = new Mock<ILoggingBroker>();
        
        _userService = new UserService(
            storageBroker: _storageBrokerMock.Object,
            loggingBroker:_loggingBrokerMock.Object);
    }

    private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
                    actualException => actualException.SameExceptionAs(expectedException);

    private static User CreateRandomUser() => CreateUserFiller().Create();

    private static Filler<User> CreateUserFiller()
    {
        var filler = new Filler<User>();
        filler.Setup().OnType<IEnumerable<Essay>>().IgnoreIt();

        return filler;
    }
}