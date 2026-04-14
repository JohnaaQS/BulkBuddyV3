using BulkBuddy.Models.Domain;
using BulkBuddy.Models.ViewModels;

namespace BulkBuddy.Repositories;

// Contract voor user-databaseacties.
public interface IUserRepository
{
    Task<User?> GetFirstUserAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail);
    Task<int> CreateAsync(RegisterViewModel model, string passwordHash, string passwordSalt);
    Task UpdateProfileAsync(int userId, EditProfileViewModel model);
}