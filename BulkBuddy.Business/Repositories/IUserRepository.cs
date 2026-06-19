using BulkBuddy.Business.Models;
using BulkBuddy.Business.Models.Domain;

namespace BulkBuddy.Business.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail);
    Task<int> CreateAsync(RegisterRequest model, string passwordHash, string passwordSalt);
    Task UpdateProfileAsync(int userId, UpdateProfileRequest model);
}
 