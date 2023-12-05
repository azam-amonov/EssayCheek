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
        SqlException sqlException = CreateSqlException();

        var failedUserStorageException = 
                new FailedUserStorageException(sqlException);

        var expectedUserStorageException = 
                new UserDependencyException(failedUserStorageException);

        _storageBrokerMock.Setup(broker =>
                broker.InsertUserAsync(randomUser)).ThrowsAsync(sqlException);

        // When
        ValueTask<User> addUserTask = _userService.AddUserAsync(randomUser);

        UserDependencyException actualUserDependencyException =
                        await Assert.ThrowsAsync<UserDependencyException>(addUserTask.AsTask);
        
        // Then
        actualUserDependencyException.Should()
                .BeEquivalentTo(expectedUserStorageException);

        _storageBrokerMock.Verify(broker 
                => broker.InsertUserAsync(randomUser), Times.Once);
        
        _loggingBrokerMock.Verify(broker => 
                    broker.LogCritical(It.Is(
                        SameExceptionAs(expectedUserStorageException))), Times.Once);
        
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowDependencyValidationExceptionOnAddIfDuplicateKeyErrorOccurredAndLogAsync()
    {
        // Given
        User randomUser = CreateRandomUser();
        string randomMessage = GteRandomString();
        
        var duplicateKeyException = new DuplicateKeyException(randomMessage);

        var alreadyExistsUserException = 
                new AlreadyExistsUserException(duplicateKeyException);

        var exceptedUserDependencyValidationException =
                new UserDependencyValidationException(alreadyExistsUserException);
        
        _storageBrokerMock.Setup(broker => broker.InsertUserAsync(randomUser))
                        .ThrowsAsync(duplicateKeyException);

        // When
        ValueTask<User> addUserTask = _userService.AddUserAsync(randomUser);

        UserDependencyValidationException actualDependencyValidationException =
                await Assert.ThrowsAsync<UserDependencyValidationException>(addUserTask.AsTask);
        
        // Then
        actualDependencyValidationException.Should().BeEquivalentTo(exceptedUserDependencyValidationException);
        
        _loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                        exceptedUserDependencyValidationException))), Times.Once);
        
        _storageBrokerMock.Verify(broker =>
                broker.InsertUserAsync(It.IsAny<User>()),Times.Once);
        
        _dateTimeBrokerMock.VerifyNoOtherCalls();
        _loggingBrokerMock.VerifyNoOtherCalls();
        _storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccuredAndLogAsync()
    {
            // Given
            User randomUser = CreateRandomUser();
            var serviceException = new Exception();
            var failedUserServiceException = new FailedUserServiceException(serviceException);

            var expectedUserServiceException = new UserServiceException(failedUserServiceException);

            _storageBrokerMock.Setup(broker =>
                        broker.InsertUserAsync(randomUser)).ThrowsAsync(serviceException);
            
            // When

            ValueTask<User> addUserTask =  _userService.AddUserAsync(randomUser);

            UserServiceException actualUserServiceException =
                await Assert.ThrowsAsync<UserServiceException>(addUserTask.AsTask);
            
            // Then
            actualUserServiceException.Should().BeEquivalentTo(expectedUserServiceException);
            
            _loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                        expectedUserServiceException))), Times.Once);
            
            _storageBrokerMock.Verify(broker => broker.InsertUserAsync(randomUser), Times.Once);
            
            _loggingBrokerMock.VerifyNoOtherCalls();
            _storageBrokerMock.VerifyNoOtherCalls();
    }
}