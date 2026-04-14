using System.ComponentModel.DataAnnotations;

namespace BulkBuddy.Models.ViewModels;

// ViewModel voor het registratieformulier.
public class RegisterViewModel
{
    [Required(ErrorMessage = "Gebruikersnaam is verplicht.")]
    [StringLength(64)]
    public string Username { get; set; } = "";

    [Required(ErrorMessage = "E-mail is verplicht.")]
    [EmailAddress(ErrorMessage = "Voer een geldig e-mailadres in.")]
    [StringLength(254)]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Wachtwoord is verplicht.")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Wachtwoord moet minimaal 6 tekens hebben.")]
    public string Password { get; set; } = "";

    [Required(ErrorMessage = "Bevestig je wachtwoord.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Wachtwoorden komen niet overeen.")]
    [Display(Name = "Bevestig wachtwoord")]
    public string ConfirmPassword { get; set; } = "";

    [Range(13, 120, ErrorMessage = "Leeftijd moet tussen 13 en 120 liggen.")]
    public int Age { get; set; } = 20;

    [Range(typeof(decimal), "1", "300")]
    [Display(Name = "Lengte (cm)")]
    public decimal HeightCm { get; set; } = 180;

    [Range(typeof(decimal), "1", "500")]
    [Display(Name = "Gewicht (kg)")]
    public decimal WeightKg { get; set; } = 70;

    [Range(typeof(decimal), "1", "500")]
    [Display(Name = "Doelgewicht (kg)")]
    public decimal TargetWeightKg { get; set; } = 80;

    [Required]
    [StringLength(32)]
    public string Goal { get; set; } = "bulk";

    [Required]
    [StringLength(32)]
    [Display(Name = "Goal phase")]
    public string GoalPhase { get; set; } = "bulk";

    [Required]
    [StringLength(16)]
    public string Sex { get; set; } = "Man";

    [Range(0, 14)]
    [Display(Name = "Trainingen per week")]
    public int TrainingFrequencyPerWeek { get; set; } = 3;

    [Range(typeof(decimal), "1.0", "3.0")]
    [Display(Name = "Activity multiplier")]
    public decimal ActivityMultiplier { get; set; } = 1.55m;
}