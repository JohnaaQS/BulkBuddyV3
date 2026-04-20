using BulkBuddy.Data;
using Dapper;
using BulkBuddy.Models.ViewModels;


namespace BulkBuddy.Repositories;

// Repository voor meal-gerelateerde databasequeries.
public class MealRepository : IMealRepository
{
    private readonly DbConnectionFactory _dbConnectionFactory;

    // Constructor die de DbConnectionFactory injecteert.
    public MealRepository(DbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    
    // Haal het totale aantal calorieën op dat vandaag is gelogd voor de gebruiker.
    public async Task<int> GetTotalCaloriesForTodayAsync(int userId)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = """
            SELECT COALESCE(SUM(total_calories), 0)
            FROM meal_entries
            WHERE user_id = @UserId
              AND meal_date = CURRENT_DATE;
            """;

        return await connection.ExecuteScalarAsync<int>(sql, new { UserId = userId });
    }

    // Haal het aantal meals op dat vandaag is gelogd voor de gebruiker.
    public async Task<int> GetMealCountForTodayAsync(int userId)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = """
            SELECT COUNT(*)
            FROM meal_entries
            WHERE user_id = @UserId
              AND meal_date = CURRENT_DATE;
            """;

        return await connection.ExecuteScalarAsync<int>(sql, new { UserId = userId });
    }

    // Haal alle meals van vandaag op voor de gebruiker, gesorteerd op meal slot en ID.
     public async Task<List<MealEntryViewModel>> GetMealsForTodayAsync(int userId)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = """
            SELECT
                id,
                name,
                meal_slot AS MealSlot,
                total_calories AS TotalCalories,
                protein_grams AS ProteinGrams,
                carbs_grams AS CarbsGrams,
                fats_grams AS FatsGrams,
                ingredient_summary AS IngredientSummary,
                meal_date AS MealDate
            FROM meal_entries
            WHERE user_id = @UserId
              AND meal_date = CURRENT_DATE
            ORDER BY
                CASE meal_slot
                    WHEN 'breakfast' THEN 1
                    WHEN 'lunch' THEN 2
                    WHEN 'dinner' THEN 3
                    WHEN 'snacks' THEN 4
                    ELSE 5
                END,
                id;
            """;

        var meals = await connection.QueryAsync<MealEntryViewModel>(sql, new { UserId = userId });
        return meals.ToList();
    }
}