using BulkBuddy.Business.Exceptions;
using BulkBuddy.Business.Models;
using BulkBuddy.Business.Repositories;
using BulkBuddy.DataAccess.Data;
using Dapper;
using Npgsql;

namespace BulkBuddy.DataAccess.Repositories;

// Repository voor meal-gerelateerde databasequeries.
// NpgsqlException wordt hier omgezet naar DatabaseException — Business hoeft Npgsql niet te kennen.
public class MealRepository : IMealRepository
{
    private readonly DbConnectionFactory _dbConnectionFactory;

    public MealRepository(DbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    private static async Task<T> ExecuteAsync<T>(Func<Task<T>> operation)
    {
        try { return await operation(); }
        catch (NpgsqlException ex) { throw new DatabaseException("Database onbereikbaar.", ex); }
    }

    private static async Task ExecuteAsync(Func<Task> operation)
    {
        try { await operation(); }
        catch (NpgsqlException ex) { throw new DatabaseException("Database onbereikbaar.", ex); }
    }

    public Task<int> GetTotalCaloriesForTodayAsync(int userId) => ExecuteAsync(async () =>
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        const string sql = """
            SELECT COALESCE(SUM(total_calories), 0)
            FROM meal_entries
            WHERE user_id = @UserId AND meal_date = CURRENT_DATE;
            """;
        return await connection.ExecuteScalarAsync<int>(sql, new { UserId = userId });
    });

    public Task<int> GetMealCountForTodayAsync(int userId) => ExecuteAsync(async () =>
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        const string sql = """
            SELECT COUNT(*)
            FROM meal_entries
            WHERE user_id = @UserId AND meal_date = CURRENT_DATE;
            """;
        return await connection.ExecuteScalarAsync<int>(sql, new { UserId = userId });
    });

    public Task<List<MealEntry>> GetMealsForTodayAsync(int userId) => ExecuteAsync(async () =>
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        const string sql = """
            SELECT
                id, name,
                meal_slot AS MealSlot,
                total_calories AS TotalCalories,
                protein_grams AS ProteinGrams,
                carbs_grams AS CarbsGrams,
                fats_grams AS FatsGrams,
                ingredient_summary AS IngredientSummary,
                meal_date AS MealDate
            FROM meal_entries
            WHERE user_id = @UserId AND meal_date = CURRENT_DATE
            ORDER BY
                CASE meal_slot
                    WHEN 'breakfast' THEN 1
                    WHEN 'lunch'     THEN 2
                    WHEN 'dinner'    THEN 3
                    WHEN 'snacks'    THEN 4
                    ELSE 5
                END, id;
            """;
        var meals = await connection.QueryAsync<MealEntry>(sql, new { UserId = userId });
        return meals.ToList();
    });

    // Snapshot: data gekopieerd, geen FK naar meal_templates.
    public Task AddMealAsync(int userId, AddMealRequest model) => ExecuteAsync(async () =>
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        const string sql = """
            INSERT INTO meal_entries
                (user_id, entry_date, meal_date, meal_slot, name, total_calories, protein_grams, carbs_grams, fats_grams, ingredient_summary)
            VALUES
                (@UserId, CURRENT_DATE, @MealDate, @MealSlot, @Name, @TotalCalories, @ProteinGrams, @CarbsGrams, @FatsGrams, @IngredientSummary);
            """;
        await connection.ExecuteAsync(sql, new
        {
            UserId = userId,
            model.MealDate, model.MealSlot, model.Name,
            model.TotalCalories, model.ProteinGrams, model.CarbsGrams, model.FatsGrams, model.IngredientSummary
        });
    });
}
