using BulkBuddy.Business.Models;
using BulkBuddy.Business.Models.Domain;
using BulkBuddy.Business.Repositories;

namespace BulkBuddy.Business.Services;

// Bouwt de profieldata op voor de profielpagina.
// DatabaseException wordt gegooid door de repositories in DataAccess — niet hier.
public class ProfileService
{
    private readonly IUserRepository _userRepository;

    public ProfileService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetProfileAsync(int userId)
    {
        return await _userRepository.GetByIdAsync(userId);
    }

    public async Task<(bool Success, string ErrorMessage)> UpdateProfileAsync(int userId, UpdateProfileRequest model)
    {
        var existingByUsername = await _userRepository.GetByUsernameAsync(model.Username.Trim());
        if (existingByUsername is not null && existingByUsername.Id != userId)
            return (false, "Deze gebruikersnaam is al in gebruik.");

        var existingByEmail = await _userRepository.GetByEmailAsync(model.Email.Trim());
        if (existingByEmail is not null && existingByEmail.Id != userId)
            return (false, "Dit e-mailadres is al in gebruik.");

        await _userRepository.UpdateProfileAsync(userId, model);
        return (true, "");
    }
}
