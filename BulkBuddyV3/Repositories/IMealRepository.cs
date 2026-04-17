namespace BulkBuddy.Repositories;
using BulkBuddy.Models.ViewModels;

// Contract voor meal-data die op het dashboard getoond wordt.
public interface IMealRepository
{
    Task<int> GetTotalCaloriesForTodayAsync(int userId);
    Task<int> GetMealCountForTodayAsync(int userId);
    Task<List<MealEntryViewModel>> GetMealsForTodayAsync(int userId);
}