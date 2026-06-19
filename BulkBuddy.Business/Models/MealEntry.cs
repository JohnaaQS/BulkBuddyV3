namespace BulkBuddy.Business.Models;

public class MealEntry
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
    public string MealSlot { get; init; } = "";
    public int TotalCalories { get; init; }
    public int ProteinGrams { get; init; }
    public int CarbsGrams { get; init; }
    public int FatsGrams { get; init; }
    public string IngredientSummary { get; init; } = "";
    public DateOnly MealDate { get; init; }
}
