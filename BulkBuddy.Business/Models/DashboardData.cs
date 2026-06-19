namespace BulkBuddy.Business.Models; 

public class DashboardData
{
    public string Username { get; init; } = "";
    public decimal WeightKg { get; init; }
    public decimal HeightCm { get; init; }
    public decimal TargetWeightKg { get; init; }
    public string GoalPhase { get; init; } = "";
    public int CalorieTarget { get; init; }
    public int CaloriesToday { get; init; }
    public int MealsLoggedToday { get; init; }
    public int RemainingCalories { get; init; }
    public int CalorieProgressPercent { get; init; }
}
