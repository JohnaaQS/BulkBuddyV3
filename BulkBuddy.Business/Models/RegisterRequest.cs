namespace BulkBuddy.Business.Models;

public class RegisterRequest
{
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public int Age { get; set; }
    public decimal HeightCm { get; set; }
    public decimal WeightKg { get; set; }
    public decimal TargetWeightKg { get; set; }
    public string Goal { get; set; } = "bulk";
    public string GoalPhase { get; set; } = "bulk";
    public string Sex { get; set; } = "Man";
    public int TrainingFrequencyPerWeek { get; set; }
    public decimal ActivityMultiplier { get; set; }
}
