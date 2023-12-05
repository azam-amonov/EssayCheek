using System.Linq.Expressions;
using System.Runtime.Serialization;
using EssayCheek.Api.Brokers.DateTimes;
using EssayCheek.Api.Brokers.Logging;
using EssayCheek.Api.Brokers.StorageBroker;
using EssayCheek.Api.Model.Foundation.EssayResult;
using EssayCheek.Api.Model.Foundation.Essays;
using EssayCheek.Api.Services.EssayResults;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace EssayCheek.API.UnitTest.Services.Foundations.EssayResults;

public partial class EssayResultServiceTest
{
    private readonly Mock<IStorageBroker> _storageBrokerMock;
    private readonly Mock<ILoggingBroker> _loggingBrokerMock;
    private readonly Mock<IDateTimeBroker> _dateTimeBrokerMock;
    private readonly IEssayResultService _essayResultService;

    public EssayResultServiceTest()
    {
        _storageBrokerMock = new Mock<IStorageBroker>();
        _loggingBrokerMock = new Mock<ILoggingBroker>();
        _dateTimeBrokerMock = new Mock<IDateTimeBroker>();
        
        _essayResultService = new EssayResultService(
                        storageBroker: _storageBrokerMock.Object,
                        dateTimeBroker: _dateTimeBrokerMock.Object,
                        loggingBroker: _loggingBrokerMock.Object);
        
    }

    private static IQueryable<EssayResult> CreateRandomEssayResults()
    {
        return CreateEssayResultFiller(dates: GetRandomDateTimeOffSet())
                        .Create(count: GetRandomNumber()).AsQueryable();
    }

    private static EssayResult CreateRandomEssayResult() =>
                    CreateEssayResultFiller(GetRandomDateTime()).Create();

    private static string GetRandomString() =>
                    new MnemonicString(wordCount: GetRandomNumber()).GetValue();

    private static int GetRandomNumber() =>
                    new IntRange(min: 2, max: 10).GetValue();

    private static DateTimeOffset GetRandomDateTimeOffSet() =>
                    new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

    private static DateTimeOffset GetRandomDateTime() =>
                    new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();
    
    private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
                    actualException => actualException.SameExceptionAs(expectedException);

    private static SqlException CreateSqlException() =>
                    (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));
    
    private static Filler<EssayResult> CreateEssayResultFiller(DateTimeOffset dates)
    {
        var filler = new Filler<EssayResult>();
        filler.Setup().OnType<Essay>().IgnoreIt();
        return filler;
    }
}