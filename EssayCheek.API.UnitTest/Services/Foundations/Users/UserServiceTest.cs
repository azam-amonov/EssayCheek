using System.Linq.Expressions;
using System.Runtime.Serialization;
using EssayCheek.Api.Brokers.DateTimes;
using EssayCheek.Api.Brokers.Logging;
using EssayCheek.Api.Brokers.StorageBroker;
using EssayCheek.Api.Model.Foundation.Essays;
using EssayCheek.Api.Model.Foundation.Users;
using EssayCheek.Api.Services.Users;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace EssayCheek.API.UnitTest.Services.Foundations.Users;

public partial class UserServiceTest
{
    private readonly Mock<IStorageBroker> _storageBrokerMock;
    private readonly Mock<IDateTimeBroker> _dateTimeBrokerMock;
    private readonly Mock<ILoggingBroker> _loggingBrokerMock;
    private readonly IUserService _userService;

    public UserServiceTest()
    {
        _storageBrokerMock = new Mock<IStorageBroker>();
        _dateTimeBrokerMock = new Mock<IDateTimeBroker>();
        _loggingBrokerMock = new Mock<ILoggingBroker>();
        
        _userService = new UserService(
            storageBroker: _storageBrokerMock.Object,
            dateTimeBroker: _dateTimeBrokerMock.Object,
            loggingBroker:_loggingBrokerMock.Object);
    }

    private static IQueryable<User> CreateRandomUsers()
    {
        return CreateUserFiller(dates: GetRandomDateTimeOffset())
            .Create(count: GetRandomNumber()).AsQueryable();
    }
    
    private static string GteRandomString() =>
                    new MnemonicString(wordCount: GetRandomNumber()).GetValue();
    
    private static int GetRandomNumber() =>
            new IntRange(min: 9, max: 99).GetValue();

    private static DateTimeOffset GetRandomDateTimeOffset() =>
                    new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();
    
    private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
                    actualException => actualException.SameExceptionAs(expectedException);
    
    private static DateTimeOffset GetRandomDateTime() =>
                    new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();
    
    private static SqlException CreateSqlException() =>
                    (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

    private static User CreateRandomUser() =>
                    CreateUserFiller(GetRandomDateTime()).Create();

    private static Filler<User> CreateUserFiller(DateTimeOffset dates)
    {
        var filler = new Filler<User>();
        filler.Setup().OnType<DateTimeOffset>().Use(dates);
        filler.Setup().OnType<IEnumerable<Essay>>().IgnoreIt();
        
        return filler;
    }
}