using System.ComponentModel.DataAnnotations;

namespace BulkBuddy.Web.Models.ViewModels;

public class TemplateFormViewModel
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Naam is verplicht.")]
    [StringLength(255)]
    public string Name { get; set; } = "";

    [Required]
    public string MealSlot { get; set; } = "breakfast";

    [Range(0, 10000)]
    [Display(Name = "Calorieën")]
    public int TotalCalories { get; set; }

    [Range(0, 1000)]
    [Display(Name = "Eiwitten (g)")]
    public int ProteinGrams { get; set; }

    [Range(0, 1000)]
    [Display(Name = "Koolhydraten (g)")]
    public int CarbsGrams { get; set; }

    [Range(0, 1000)]
    [Display(Name = "Vetten (g)")]
    public int FatsGrams { get; set; }

    [Display(Name = "Ingrediënten")]
    public string IngredientSummary { get; set; } = "";
}
