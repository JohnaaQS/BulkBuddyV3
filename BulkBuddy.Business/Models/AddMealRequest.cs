namespace BulkBuddy.Business.Models;

public class AddMealRequest
{
    public string Name { get; set; } = "";
    public string MealSlot { get; set; } = "Breakfast";
    public DateTime MealDate { get; set; }
    public int TotalCalories { get; set; }
    public int ProteinGrams { get; set; }
    public int CarbsGrams { get; set; }
    public int FatsGrams { get; set; }
    public string IngredientSummary { get; set; } = "";
}
