namespace BulkBuddy.Models.Domain;

// Domeinmodel van een gebruiker.
// anamic/rich models 
public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = "";
    public string Email { get; set; } = "";

    // Nodig voor authenticatie.
    public string PasswordHash { get; set; } = "";
    public string PasswordSalt { get; set; } = "";

    public int Age { get; set; }
    public decimal HeightCm { get; set; }
    public decimal WeightKg { get; set; }
    public decimal TargetWeightKg { get; set; }

    public string Goal { get; set; } = "";
    public string GoalPhase { get; set; } = "";
    public string Sex { get; set; } = "Man";

    public int TrainingFrequencyPerWeek { get; set; }
    public decimal ActivityMultiplier { get; set; }

    public bool OnboardingCompleted { get; set; }
}