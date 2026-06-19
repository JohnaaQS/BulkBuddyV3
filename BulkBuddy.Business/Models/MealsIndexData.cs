namespace BulkBuddy.Business.Models;

public class MealsIndexData
{
    public string Username { get; init; } = "";
    public DateTime SelectedDate { get; init; }
    public int TotalCaloriesToday { get; init; }
    public int MealsLoggedToday { get; init; }
    public List<MealEntry> Meals { get; init; } = new();
}
