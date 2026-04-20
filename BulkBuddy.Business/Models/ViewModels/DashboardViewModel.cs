namespace BulkBuddy.Business.Models.ViewModels;

// ViewModel voor de dashboardpagina.
public class DashboardViewModel
{
    public string Username { get; set; } = "";
    public decimal WeightKg { get; set; }
    public decimal HeightCm { get; set; }
    public decimal TargetWeightKg { get; set; }
    public string GoalPhase { get; set; } = "";
    public int CalorieTarget { get; set; }

    public string WelcomeMessage { get; set; } = "";
    public string GoalSummary { get; set; } = "";

    // Dashboarddata voor de huidige dag.
    public int CaloriesToday { get; set; }
    public int MealsLoggedToday { get; set; }

    // Extra voedingsprogressie voor de dashboardkaart.
    public int RemainingCalories { get; set; }
    public int CalorieProgressPercent { get; set; }
}
