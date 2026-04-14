using BulkBuddy.Data;
using Dapper;

namespace BulkBuddy.Repositories;

// Repository voor meal-gerelateerde databasequeries.
public class MealRepository : IMealRepository
{
    private readonly DbConnectionFactory _dbConnectionFactory;

    public MealRepository(DbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

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
}