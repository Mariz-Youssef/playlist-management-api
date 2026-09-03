using Moq;
using PlaylistManagement.Api.Exceptions;
using PlaylistManagement.Api.Features.Authentication;
using PlaylistManagement.Api.Features.Authentication.DTOs;
using PlaylistManagement.Api.Models.Entities;
using PlaylistManagement.Api.Repositories.Interfaces;

namespace PlaylistManagement.UnitTests.Features.Authentication;

public class AuthServiceTests
{
    [Fact]
    public async Task RegisterAsync_ShouldCreateUserAndReturnToken()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var tokenServiceMock = new Mock<ITokenService>();

        var service = new AuthService(userRepositoryMock.Object,tokenServiceMock.Object);

        var request = new RegisterRequest
        {
            Name = "  Mariz  ",
            Email = "  Mariz@gmail.com  ",
            Password = "Password123!"
        };

        var expectedResponse = new AuthResponse
        {
            AccessToken = "test-token",
            ExpiresAt = DateTime.UtcNow.AddMinutes(60)
        };

        userRepositoryMock
            .Setup(r => r.EmailExistsAsync(
                "mariz@example.com",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        tokenServiceMock
            .Setup(t => t.GenerateAccessToken(It.IsAny<User>()))
            .Returns(expectedResponse);

        // Act
        var result = await service.RegisterAsync(request);

        // Assert
        Assert.Equal(expectedResponse, result);

        userRepositoryMock.Verify(
            r => r.AddAsync(
                It.Is<User>(u =>
                    u.Name == "Mariz" &&
                    u.Email == "mariz@gmail.com" &&
                    u.PasswordHash != "Password123!"),
                It.IsAny<CancellationToken>()),
            Times.Once);

        tokenServiceMock.Verify(
            t => t.GenerateAccessToken(
                It.Is<User>(u =>
                    u.Email == "mariz@gmail.com")),
            Times.Once);
    }
    [Fact]
    public async Task RegisterAsync_ShouldThrowConflict_WhenEmailAlreadyExists()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var tokenServiceMock = new Mock<ITokenService>();

        var service = new AuthService(
            userRepositoryMock.Object,
            tokenServiceMock.Object);

        var request = new RegisterRequest
        {
            Name = "Mariz",
            Email = "mariz@example.com",
            Password = "Password123!"
        };

        userRepositoryMock
            .Setup(r => r.EmailExistsAsync(
                "mariz@example.com",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() =>
            service.RegisterAsync(request));

        userRepositoryMock.Verify(
            r => r.AddAsync(
                It.IsAny<User>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        tokenServiceMock.Verify(
            t => t.GenerateAccessToken(It.IsAny<User>()),
            Times.Never);
    }
    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var tokenServiceMock = new Mock<ITokenService>();

        var service = new AuthService(
            userRepositoryMock.Object,
            tokenServiceMock.Object);

        var password = "Password123!";

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Mariz",
            Email = "mariz@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            CreatedAt = DateTime.UtcNow
        };

        var expectedResponse = new AuthResponse
        {
            AccessToken = "test-token",
            ExpiresAt = DateTime.UtcNow.AddMinutes(60)
        };

        userRepositoryMock
            .Setup(r => r.GetByEmailAsync(
                "mariz@example.com",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        tokenServiceMock
            .Setup(t => t.GenerateAccessToken(user))
            .Returns(expectedResponse);

        var request = new LoginRequest
        {
            Email = "  MARIZ@EXAMPLE.COM ",
            Password = password
        };

        // Act
        var result = await service.LoginAsync(request);

        // Assert
        Assert.Equal(expectedResponse, result);

        tokenServiceMock.Verify(
            t => t.GenerateAccessToken(user),
            Times.Once);
    }
    [Fact]
    public async Task LoginAsync_ShouldThrowUnauthorized_WhenCredentialsAreInvalid()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var tokenServiceMock = new Mock<ITokenService>();

        var service = new AuthService(
            userRepositoryMock.Object,
            tokenServiceMock.Object);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Mariz",
            Email = "mariz@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword123!"),
            CreatedAt = DateTime.UtcNow
        };

        userRepositoryMock
            .Setup(r => r.GetByEmailAsync(
                "mariz@example.com",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var request = new LoginRequest
        {
            Email = "mariz@example.com",
            Password = "WrongPassword123!"
        };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            service.LoginAsync(request));

        tokenServiceMock.Verify(
            t => t.GenerateAccessToken(It.IsAny<User>()),
            Times.Never);
    }
}