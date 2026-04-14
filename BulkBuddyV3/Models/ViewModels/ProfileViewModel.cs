namespace BulkBuddy.Models.ViewModels;

// ViewModel voor de profielpagina.
public class ProfileViewModel
{
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public int Age { get; set; }
    public decimal HeightCm { get; set; }
    public decimal WeightKg { get; set; }
    public decimal TargetWeightKg { get; set; }
    public string Goal { get; set; } = "";
    public string GoalPhase { get; set; } = "";
    public string Sex { get; set; } = "";
    public int TrainingFrequencyPerWeek { get; set; }
    public decimal ActivityMultiplier { get; set; }
}