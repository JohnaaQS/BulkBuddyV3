namespace BulkBuddy.Business.Models.Domain;

// Domeinmodel van een gebruiker.

public record User
{
    // Gevoelige data: alleen leesbaar na aanmaken, nooit direct aanpasbaar van buitenaf.
    public int Id { get; init; }

    public string Username { get; init; } = "";
    public string Email { get; init; } = "";

    public string PasswordHash { get; init; } = "";
    public string PasswordSalt { get; init; } = "";

    public int Age { get; init; }
    public decimal HeightCm { get; init; }
    public decimal WeightKg { get; init; }
    public decimal TargetWeightKg { get; init; }

    public string Goal { get; init; } = "";
    public string GoalPhase { get; init; } = "";
    public string Sex { get; init; } = "Man";

    public int TrainingFrequencyPerWeek { get; init; }
    public decimal ActivityMultiplier { get; init; }

    public bool OnboardingCompleted { get; init; }
}
