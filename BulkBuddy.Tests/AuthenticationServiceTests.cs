// dotnet test BulkBuddy.Tests/BulkBuddy.Tests.csproj
using BulkBuddy.Business.Models;
using BulkBuddy.Business.Models.Domain;
using BulkBuddy.Business.Repositories;
using BulkBuddy.Business.Services;
using Moq;
using Xunit;

namespace BulkBuddy.Tests;

// Test: login en registratielogica zonder echte database.
// MockUserRepository simuleert de database — geen Npgsql of PostgreSQL nodig.
// Dit is polymorfisme in de praktijk: AuthenticationService werkt tegen IUserRepository,
// niet tegen een concrete klasse.
public class AuthenticationServiceTests
{
    private readonly Mock<IUserRepository> _repoMock = new();
    private readonly PasswordService _passwordService = new();

    private AuthenticationService CreateSut() =>
        new(_repoMock.Object, _passwordService);

    // --- Login tests ---

    [Fact]
    public async Task LoginAsync_UnknownUser_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetByUsernameOrEmailAsync(It.IsAny<string>()))
                 .ReturnsAsync((User?)null);

        var result = await CreateSut().LoginAsync("onbekend", "wachtwoord");

        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_WrongPassword_ReturnsNull()
    {
        var (hash, salt) = _passwordService.HashPassword("JuistWachtwoord");

        _repoMock.Setup(r => r.GetByUsernameOrEmailAsync("johnaa"))
                 .ReturnsAsync(new User { Username = "johnaa", PasswordHash = hash, PasswordSalt = salt });

        var result = await CreateSut().LoginAsync("johnaa", "FoutWachtwoord");

        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_CorrectCredentials_ReturnsUser()
    {
        var (hash, salt) = _passwordService.HashPassword("JuistWachtwoord");

        _repoMock.Setup(r => r.GetByUsernameOrEmailAsync("johnaa"))
                 .ReturnsAsync(new User { Username = "johnaa", PasswordHash = hash, PasswordSalt = salt });

        var result = await CreateSut().LoginAsync("johnaa", "JuistWachtwoord");

        Assert.NotNull(result);
        Assert.Equal("johnaa", result.Username);
    }

    // --- Registratie tests ---

    [Fact]
    public async Task RegisterAsync_DuplicateUsername_ReturnsFailure()
    {
        _repoMock.Setup(r => r.GetByUsernameAsync("johnaa"))
                 .ReturnsAsync(new User { Username = "johnaa" });

        var (success, error, _) = await CreateSut().RegisterAsync(new RegisterRequest
        {
            Username = "johnaa",
            Email = "nieuw@test.nl",
            Password = "Wachtwoord123!"
        });

        Assert.False(success);
        Assert.Contains("gebruikersnaam", error);
    }

    [Fact]
    public async Task RegisterAsync_DuplicateEmail_ReturnsFailure()
    {
        _repoMock.Setup(r => r.GetByUsernameAsync(It.IsAny<string>()))
                 .ReturnsAsync((User?)null);
        _repoMock.Setup(r => r.GetByEmailAsync("bestaand@test.nl"))
                 .ReturnsAsync(new User { Email = "bestaand@test.nl" });

        var (success, error, _) = await CreateSut().RegisterAsync(new RegisterRequest
        {
            Username = "nieuw",
            Email = "bestaand@test.nl",
            Password = "Wachtwoord123!"
        });

        Assert.False(success);
        Assert.Contains("e-mailadres", error);
    }

    [Fact]
    public async Task RegisterAsync_NewUser_ReturnsSuccessWithUserId()
    {
        _repoMock.Setup(r => r.GetByUsernameAsync(It.IsAny<string>()))
                 .ReturnsAsync((User?)null);
        _repoMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
                 .ReturnsAsync((User?)null);
        _repoMock.Setup(r => r.CreateAsync(It.IsAny<RegisterRequest>(), It.IsAny<string>(), It.IsAny<string>()))
                 .ReturnsAsync(42); // gesimuleerde nieuwe userId

        var (success, _, userId) = await CreateSut().RegisterAsync(new RegisterRequest
        {
            Username = "nieuw",
            Email = "nieuw@test.nl",
            Password = "Wachtwoord123!"
        });

        Assert.True(success);
        Assert.Equal(42, userId);
    }
}
