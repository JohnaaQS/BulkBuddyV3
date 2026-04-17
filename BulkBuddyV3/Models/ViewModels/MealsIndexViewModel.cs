namespace BulkBuddy.Models.ViewModels;

// ViewModel voor de volledige meals overzichtspagina.
public class MealsIndexViewModel
{
    public string Username { get; set; } = "";
    public DateTime SelectedDate { get; set; }
    public int TotalCaloriesToday { get; set; }
    public int MealsLoggedToday { get; set; }

    public List<MealEntryViewModel> Meals { get; set; } = new();
}