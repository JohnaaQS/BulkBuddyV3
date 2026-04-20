using BulkBuddy.Business.Models.Domain;
using BulkBuddy.Business.Models.ViewModels;
using BulkBuddy.Business.Repositories;

namespace BulkBuddy.Business.Services;

// Verwerkt login- en registratieflow.
public class AuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordService _passwordService;

    public AuthenticationService(IUserRepository userRepository, PasswordService passwordService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
    }

    public async Task<User?> LoginAsync(LoginViewModel model)
    {
        var user = await _userRepository.GetByUsernameOrEmailAsync(model.UsernameOrEmail.Trim());

        if (user is null)
        {
            return null;
        }

        var isValidPassword = _passwordService.VerifyPassword(
            model.Password,
            user.PasswordHash,
            user.PasswordSalt);

        return isValidPassword ? user : null;
    }

    public async Task<(bool Success, string ErrorMessage, int? UserId)> RegisterAsync(RegisterViewModel model)
    {
        var existingUsername = await _userRepository.GetByUsernameAsync(model.Username.Trim());
        if (existingUsername is not null)
        {
            return (false, "Deze gebruikersnaam bestaat al.", null);
        }

        var existingEmail = await _userRepository.GetByEmailAsync(model.Email.Trim());
        if (existingEmail is not null)
        {
            return (false, "Dit e-mailadres bestaat al.", null);
        }

        var (hash, salt) = _passwordService.HashPassword(model.Password);

        var userId = await _userRepository.CreateAsync(model, hash, salt);

        return (true, "", userId);
    }
}
