namespace BulkBuddy.Web.Models.ViewModels;

public class DashboardViewModel
{
    public string Username { get; set; } = "";
    public decimal WeightKg { get; set; }
    public decimal HeightCm { get; set; }
    public decimal TargetWeightKg { get; set; }
    public string GoalPhase { get; set; } = "";
    public int CalorieTarget { get; set; }
    public int CaloriesToday { get; set; }
    public int MealsLoggedToday { get; set; }
    public int RemainingCalories { get; set; }
    public int CalorieProgressPercent { get; set; }
}
