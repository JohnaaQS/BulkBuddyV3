using BulkBuddy.Business.Exceptions;
using BulkBuddy.Business.Models;
using BulkBuddy.Business.Repositories;
using BulkBuddy.DataAccess.Data;
using Dapper;
using Npgsql;

namespace BulkBuddy.DataAccess.Repositories;

// NpgsqlException wordt hier omgezet naar DatabaseException — Business hoeft Npgsql niet te kennen.
public class MealTemplateRepository : IMealTemplateRepository
{
    private readonly DbConnectionFactory _dbConnectionFactory;

    public MealTemplateRepository(DbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    private static async Task<T> ExecuteAsync<T>(Func<Task<T>> operation)
    {
        try { return await operation(); }
        catch (NpgsqlException ex) { throw new DatabaseException("Database onbereikbaar bij templates.", ex); }
    }

    private static async Task ExecuteAsync(Func<Task> operation)
    {
        try { await operation(); }
        catch (NpgsqlException ex) { throw new DatabaseException("Database onbereikbaar bij templates.", ex); }
    }

    private const string SelectColumns = """
        id,
        name,
        default_slot        AS MealSlot,
        is_system           AS IsGlobal,
        user_id             AS UserId,
        total_calories      AS TotalCalories,
        protein_grams       AS ProteinGrams,
        carbs_grams         AS CarbsGrams,
        fats_grams          AS FatsGrams,
        ingredient_summary  AS IngredientSummary
        """;

    public Task<List<SavedMealCard>> GetTemplatesForUserAsync(int userId) => ExecuteAsync(async () =>
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var sql = $"""
            SELECT {SelectColumns}
            FROM meal_templates
            WHERE is_system = true OR user_id = @UserId
            ORDER BY
                is_system DESC,
                CASE default_slot
                    WHEN 'breakfast' THEN 1
                    WHEN 'lunch'     THEN 2
                    WHEN 'dinner'    THEN 3
                    WHEN 'snacks'    THEN 4
                    ELSE 5
                END,
                name;
            """;
        var results = await connection.QueryAsync<SavedMealCard>(sql, new { UserId = userId });
        return results.ToList();
    });

    public Task<SavedMealCard?> GetTemplateByIdAsync(int id) => ExecuteAsync(async () =>
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var sql = $"SELECT {SelectColumns} FROM meal_templates WHERE id = @Id LIMIT 1;";
        return await connection.QueryFirstOrDefaultAsync<SavedMealCard>(sql, new { Id = id });
    });

    public Task<int> CreateUserTemplateAsync(int userId, CreateTemplateRequest request) => ExecuteAsync(async () =>
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        const string sql = """
            INSERT INTO meal_templates
                (name, default_slot, is_system, user_id, total_calories, protein_grams, carbs_grams, fats_grams, ingredient_summary, created_at)
            VALUES
                (@Name, @MealSlot, false, @UserId, @TotalCalories, @ProteinGrams, @CarbsGrams, @FatsGrams, @IngredientSummary, NOW())
            RETURNING id;
            """;
        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            request.Name, request.MealSlot, UserId = userId,
            request.TotalCalories, request.ProteinGrams, request.CarbsGrams,
            request.FatsGrams, request.IngredientSummary
        });
    });

    public Task UpdateUserTemplateAsync(int templateId, CreateTemplateRequest request) => ExecuteAsync(async () =>
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        const string sql = """
            UPDATE meal_templates
            SET name               = @Name,
                default_slot       = @MealSlot,
                total_calories     = @TotalCalories,
                protein_grams      = @ProteinGrams,
                carbs_grams        = @CarbsGrams,
                fats_grams         = @FatsGrams,
                ingredient_summary = @IngredientSummary
            WHERE id = @TemplateId;
            """;
        await connection.ExecuteAsync(sql, new
        {
            TemplateId = templateId,
            request.Name, request.MealSlot,
            request.TotalCalories, request.ProteinGrams, request.CarbsGrams,
            request.FatsGrams, request.IngredientSummary
        });
    });

    public Task DeleteUserTemplateAsync(int templateId) => ExecuteAsync(async () =>
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        await connection.ExecuteAsync("DELETE FROM meal_templates WHERE id = @TemplateId;", new { TemplateId = templateId });
    });
}
