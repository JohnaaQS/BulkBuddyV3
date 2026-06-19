namespace BulkBuddy.Web.Models.ViewModels;

public class AddMealViewModel
{
    public string Name { get; set; } = "";
    public string MealSlot { get; set; } = "Breakfast";
    public DateTime MealDate { get; set; } = DateTime.Today;
    public int TotalCalories { get; set; }
    public int ProteinGrams { get; set; }
    public int CarbsGrams { get; set; }
    public int FatsGrams { get; set; }
    public string IngredientSummary { get; set; } = "";
}
