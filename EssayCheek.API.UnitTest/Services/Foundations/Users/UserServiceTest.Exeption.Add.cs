using EFxceptions.Models.Exceptions;
using EssayCheek.Api.Model.Foundation.Users;
using EssayCheek.Api.Model.Foundation.Users.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;

namespace EssayCheek.API.UnitTest.Services.Foundations.Users;

public partial class UserServiceTest
{
    [Fact]
    public async ValueTask ShouldTheCriticalDependencyExceptionOnAddIfSqlErrorOccursLogItAsync()
    {
        // Given
        User randomUser = CreateRandomUser();
        SqlException sqlException = GetSqlException();

        var failedUserStorageException = new FailedUserStorageException(sqlException);

        var expectedUserStorageException = new UserDependencyException(sqlException);

        _storageBrokerMock.Setup(broker =>
                        broker.InsertUserAsync(randomUser)).ThrowsAsync(sqlException);

        // When
        ValueTask<User> addUserTask = _userService.AddUserAsync(randomUser);

        UserDependencyException actualUserDependencyException =
                        await Assert.ThrowsAsync<UserDependencyException>(addUserTask.AsTask);
        
        // Then
        actualUserDependencyException.Should().BeEquivalentTo(expectedUserStorageException);

        _loggingBrokerMock.Verify(broker => 
                    broker.LogCritical(It.Is(
                        SameExceptionAs(expectedUserStorageException))),
                                Times.Once);
        
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]

    public async Task ShouldThrowDependencyValidationExceptionOnAddIfDuplicateKeyErrorOccurredAndLogAsync()
    {
        // Given
        User randomUser = CreateRandomUser();
        string someMessage = GteRandomString();
        var duplicateKeyException = new DuplicateKeyException(someMessage);

        var alreadyExistsUserException = new AlreadyExistsUserException(duplicateKeyException);

        var exceptedUserDependencyValidationException =
                new UserDependencyValidationException(alreadyExistsUserException);
        
        // When
        ValueTask<User> addUserTask = _userService.AddUserAsync(randomUser);

        UserDependencyValidationException actualDependencyValidationException =
                await Assert.ThrowsAsync<UserDependencyValidationException>(addUserTask.AsTask);
        
        // Then
        actualDependencyValidationException.Should().BeEquivalentTo(exceptedUserDependencyValidationException);

        _loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                        exceptedUserDependencyValidationException))),
                Times.Once);
        
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }
}