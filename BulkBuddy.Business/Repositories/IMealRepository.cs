using BulkBuddy.Business.Models;

namespace BulkBuddy.Business.Repositories;

public interface IMealRepository
{
    Task<int> GetTotalCaloriesForTodayAsync(int userId);
    Task<int> GetMealCountForTodayAsync(int userId);
    Task<List<MealEntry>> GetMealsForTodayAsync(int userId);

    // Sla een nieuwe maaltijdinvoer op voor de gebruiker.
    // Data wordt gekopieerd (snapshot) — geen FK naar templates.
    Task AddMealAsync(int userId, AddMealRequest model);
}
