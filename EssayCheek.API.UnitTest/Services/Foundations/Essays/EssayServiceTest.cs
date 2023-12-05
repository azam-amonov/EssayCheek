using System.Linq.Expressions;
using System.Runtime.Serialization;
using EssayCheek.Api.Brokers.DateTimes;
using EssayCheek.Api.Brokers.Logging;
using EssayCheek.Api.Brokers.StorageBroker;
using EssayCheek.Api.Model.EssayResult;
using EssayCheek.Api.Model.Foundation.Essays;
using EssayCheek.Api.Services.Essays;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace EssayCheek.API.UnitTest.Services.Foundations.Essays;

public partial class EssayServiceTest
{
    private readonly Mock<IStorageBroker> _storageBrokerMock;
    private readonly Mock<IDateTimeBroker> _dateTimeBrokerMock;
    private readonly Mock<ILoggingBroker> _loggingBrokerMock;
    private readonly IEssayService _essayService;

    public EssayServiceTest()
    {
        _storageBrokerMock = new Mock<IStorageBroker>();
        _dateTimeBrokerMock =new  Mock<IDateTimeBroker>();
        _loggingBrokerMock = new Mock<ILoggingBroker>();
        
        _essayService = new EssayService(
                        storageBroker:_storageBrokerMock.Object,
                        dateTimeBroker:_dateTimeBrokerMock.Object,
                        loggingBroker: _loggingBrokerMock.Object );
    }

    private static IQueryable<Essay> CreateRandomEssays()
    {
        return CreateEssayFiller(dates: GetRandomDateTimeOffset())
                        .Create(count: GetRandomNumber()).AsQueryable();
    }

    private static Essay CreateRandomEssay() =>
                    CreateEssayFiller(GetRandomDateTime()).Create();
    
    private static string GetRandomString() =>
                    new MnemonicString(wordCount: GetRandomNumber()).GetValue();

    private static int GetRandomNumber() =>
                    new IntRange(min: 9, max: 99).GetValue();

    private static DateTimeOffset GetRandomDateTimeOffset() =>
                    new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();
    
    private static DateTimeOffset GetRandomDateTime() =>
                    new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

    private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) => 
                    actualException => actualException.SameExceptionAs(expectedException);
    
    private static SqlException CreateSqlException() =>
                    (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

    private static Filler<Essay> CreateEssayFiller(DateTimeOffset dates)
    {
        var filler = new Filler<Essay>();
        filler.Setup().OnType<DateTimeOffset>().Use(dates);
        filler.Setup().OnType<EssayResult>().IgnoreIt();
        
        return filler;
    }
}