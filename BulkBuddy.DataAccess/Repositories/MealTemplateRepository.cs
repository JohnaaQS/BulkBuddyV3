using BulkBuddy.Business.Models.ViewModels;

using BulkBuddy.Business.Repositories;

using BulkBuddy.DataAccess.Data;

using Dapper;

namespace BulkBuddy.DataAccess.Repositories;

public class MealTemplateRepository : IMealTemplateRepository

{

    private readonly DbConnectionFactory _dbConnectionFactory;

    public MealTemplateRepository(DbConnectionFactory dbConnectionFactory)

    {

        _dbConnectionFactory = dbConnectionFactory;

    }

    public async Task<List<SavedMealCardViewModel>> GetSavedMealsAsync()

    {

        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = """

            SELECT

                mt.id,

                mt.name,

                mt.default_slot AS MealSlot,

                mt.is_system AS IsSystem,

                COALESCE(SUM(mti.calories), 0) AS TotalCalories,

                COALESCE(SUM(mti.protein_grams), 0) AS ProteinGrams,

                COALESCE(SUM(mti.carbs_grams), 0) AS CarbsGrams,

                COALESCE(SUM(mti.fats_grams), 0) AS FatsGrams,

                STRING_AGG(mti.name, ', ' ORDER BY mti.sort_order) AS IngredientSummary

            FROM meal_templates mt

            LEFT JOIN meal_template_ingredients mti

                ON mt.id = mti.template_id

            GROUP BY mt.id, mt.name, mt.default_slot, mt.is_system

            ORDER BY

                CASE mt.default_slot

                    WHEN 'Breakfast' THEN 1

                    WHEN 'Lunch' THEN 2

                    WHEN 'Dinner' THEN 3

                    WHEN 'Snacks' THEN 4

                    ELSE 5

                END,

                mt.name;

            """;

        var meals = await connection.QueryAsync<SavedMealCardViewModel>(sql);

        return meals.ToList();

    }

}