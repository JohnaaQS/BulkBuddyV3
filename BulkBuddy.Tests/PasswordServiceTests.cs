using BulkBuddy.Business.Services;
using Xunit;

namespace BulkBuddy.Tests;

// Test: wachtwoordhashing en verificatie via PBKDF2.
public class PasswordServiceTests
{
    private readonly PasswordService _sut = new();

    [Fact]
    public void HashPassword_ReturnsNonEmptyHashAndSalt()
    {
        var (hash, salt) = _sut.HashPassword("TestWachtwoord123!");

        Assert.False(string.IsNullOrWhiteSpace(hash));
        Assert.False(string.IsNullOrWhiteSpace(salt));
    }

    [Fact]
    public void HashPassword_TwoCalls_ProduceDifferentSalts()
    {
        // Elke keer een uniek salt — anders zijn hashes voorspelbaar.
        var (_, salt1) = _sut.HashPassword("ZelfdeWachtwoord");
        var (_, salt2) = _sut.HashPassword("ZelfdeWachtwoord");

        Assert.NotEqual(salt1, salt2);
    }

    [Fact]
    public void VerifyPassword_CorrectPassword_ReturnsTrue()
    {
        var (hash, salt) = _sut.HashPassword("MijnWachtwoord");

        var result = _sut.VerifyPassword("MijnWachtwoord", hash, salt);

        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_WrongPassword_ReturnsFalse()
    {
        var (hash, salt) = _sut.HashPassword("MijnWachtwoord");

        var result = _sut.VerifyPassword("FoutWachtwoord", hash, salt);

        Assert.False(result);
    }

    [Fact]
    public void VerifyPassword_EmptyPassword_ReturnsFalse()
    {
        var (hash, salt) = _sut.HashPassword("MijnWachtwoord");

        var result = _sut.VerifyPassword("", hash, salt);

        Assert.False(result);
    }
}
