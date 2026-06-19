using BulkBuddy.Business.Models;

namespace BulkBuddy.Business.Repositories;

public interface IMealTemplateRepository
{
    Task<List<SavedMealCard>> GetTemplatesForUserAsync(int userId);
    Task<SavedMealCard?> GetTemplateByIdAsync(int id);
    Task<int> CreateUserTemplateAsync(int userId, CreateTemplateRequest request);
    Task UpdateUserTemplateAsync(int templateId, CreateTemplateRequest request);
    Task DeleteUserTemplateAsync(int templateId);
}
