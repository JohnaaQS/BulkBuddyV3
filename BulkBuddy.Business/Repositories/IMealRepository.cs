using BulkBuddy.Business.Models.ViewModels;

namespace BulkBuddy.Business.Repositories;

// Contract voor meal-data die op het dashboard getoond wordt.
public interface IMealRepository
{
    Task<int> GetTotalCaloriesForTodayAsync(int userId);
    Task<int> GetMealCountForTodayAsync(int userId);
    Task<List<MealEntryViewModel>> GetMealsForTodayAsync(int userId);
}
