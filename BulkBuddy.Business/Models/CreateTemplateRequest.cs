namespace BulkBuddy.Business.Models;

public class CreateTemplateRequest
{
    public string Name { get; set; } = "";
    public string MealSlot { get; set; } = "breakfast";
    public int TotalCalories { get; set; }
    public int ProteinGrams { get; set; }
    public int CarbsGrams { get; set; }
    public int FatsGrams { get; set; }
    public string IngredientSummary { get; set; } = "";
}
