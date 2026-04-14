namespace BulkBuddy.Repositories;

// Contract voor meal-data die op het dashboard getoond wordt.
public interface IMealRepository
{
    Task<int> GetTotalCaloriesForTodayAsync(int userId);
    Task<int> GetMealCountForTodayAsync(int userId);
}