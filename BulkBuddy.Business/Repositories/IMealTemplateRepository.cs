using BulkBuddy.Business.Models.ViewModels;

namespace BulkBuddy.Business.Repositories;

public interface IMealTemplateRepository
{
    Task<List<SavedMealCardViewModel>> GetSavedMealsAsync();
}
