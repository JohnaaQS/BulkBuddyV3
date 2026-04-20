namespace BulkBuddy.Business.Models.ViewModels;

// ViewModel voor één meal entry op de meals-pagina.
public class MealEntryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string MealSlot { get; set; } = "";
    public int TotalCalories { get; set; }
    public int ProteinGrams { get; set; }
    public int CarbsGrams { get; set; }
    public int FatsGrams { get; set; }
    public string IngredientSummary { get; set; } = "";
    public DateOnly MealDate { get; set; }
}
