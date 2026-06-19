namespace BulkBuddy.Web.Models.ViewModels;

public class SavedMealCardViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string MealSlot { get; set; } = "breakfast";
    public bool IsGlobal { get; set; }
    public int? UserId { get; set; }
    public int TotalCalories { get; set; }
    public int ProteinGrams { get; set; }
    public int CarbsGrams { get; set; }
    public int FatsGrams { get; set; }
    public string IngredientSummary { get; set; } = "";
}
