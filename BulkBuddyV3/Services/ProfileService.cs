using BulkBuddy.Models.ViewModels;
using BulkBuddy.Repositories;

namespace BulkBuddy.Services;

// Bouwt de profieldata op voor de profielpagina.
public class ProfileService
{
    private readonly IUserRepository _userRepository;

    public ProfileService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ProfileViewModel?> GetProfileAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            return null;
        }

        return new ProfileViewModel
        {
            Username = user.Username,
            Email = user.Email,
            Age = user.Age,
            HeightCm = user.HeightCm,
            WeightKg = user.WeightKg,
            TargetWeightKg = user.TargetWeightKg,
            Goal = user.Goal,
            GoalPhase = user.GoalPhase,
            Sex = user.Sex,
            TrainingFrequencyPerWeek = user.TrainingFrequencyPerWeek,
            ActivityMultiplier = user.ActivityMultiplier
        };
    }
    public async Task<EditProfileViewModel?> GetEditProfileAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            return null;
        }

        return new EditProfileViewModel
        {
            Username = user.Username,
            Email = user.Email,
            Age = user.Age,
            HeightCm = user.HeightCm,
            WeightKg = user.WeightKg,
            TargetWeightKg = user.TargetWeightKg,
            Goal = user.Goal,
            GoalPhase = user.GoalPhase,
            Sex = user.Sex,
            TrainingFrequencyPerWeek = user.TrainingFrequencyPerWeek,
            ActivityMultiplier = user.ActivityMultiplier
        };
    }

    public async Task<(bool Success, string ErrorMessage)> UpdateProfileAsync(int userId, EditProfileViewModel model)
    {
        var existingByUsername = await _userRepository.GetByUsernameAsync(model.Username.Trim());
        
        if (existingByUsername is not null && existingByUsername.Id != userId)
        {
            return (false, "Deze gebruikersnaam is al in gebruik.");
        }

        var existingByEmail = await _userRepository.GetByEmailAsync(model.Email.Trim());
        if (existingByEmail is not null && existingByEmail.Id != userId)
        {
            return (false, "Dit e-mailadres is al in gebruik.");
        }

        await _userRepository.UpdateProfileAsync(userId, model);
        return (true, "");
    }
}

