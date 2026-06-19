using BulkBuddy.Business.Models;
using BulkBuddy.Business.Repositories;

namespace BulkBuddy.Business.Services;

public class MealTemplateService
{
    private readonly IMealTemplateRepository _mealTemplateRepository;

    public MealTemplateService(IMealTemplateRepository mealTemplateRepository)
    {
        _mealTemplateRepository = mealTemplateRepository;
    }

    public async Task<List<SavedMealCard>> GetTemplatesForUserAsync(int userId)
    {
        return await _mealTemplateRepository.GetTemplatesForUserAsync(userId);
    }

    // Geeft een template terug als de gebruiker er toegang toe heeft.
    public async Task<SavedMealCard?> GetTemplateByIdForUserAsync(int userId, int templateId)
    {
        var template = await _mealTemplateRepository.GetTemplateByIdAsync(templateId);
        if (template is null) return null;
        if (!template.IsGlobal && template.UserId != userId) return null;
        return template;
    }

    public async Task<int> CreatePrivateTemplateAsync(int userId, CreateTemplateRequest request)
    {
        return await _mealTemplateRepository.CreateUserTemplateAsync(userId, request);
    }

    public async Task<(bool Success, string Error)> UpdatePrivateTemplateAsync(int userId, int templateId, CreateTemplateRequest request)
    {
        var template = await _mealTemplateRepository.GetTemplateByIdAsync(templateId);
        if (template is null) return (false, "Template niet gevonden.");
        if (template.IsGlobal) return (false, "Globale templates mogen niet gewijzigd worden.");
        if (template.UserId != userId) return (false, "Je hebt geen toegang tot deze template.");

        await _mealTemplateRepository.UpdateUserTemplateAsync(templateId, request);
        return (true, "");
    }

    public async Task<(bool Success, string Error)> DeletePrivateTemplateAsync(int userId, int templateId)
    {
        var template = await _mealTemplateRepository.GetTemplateByIdAsync(templateId);
        if (template is null) return (false, "Template niet gevonden.");
        if (template.IsGlobal) return (false, "Globale templates mogen niet verwijderd worden.");
        if (template.UserId != userId) return (false, "Je hebt geen toegang tot deze template.");

        await _mealTemplateRepository.DeleteUserTemplateAsync(templateId);
        return (true, "");
    }

    // Kopieert een globale template naar de private templates van de gebruiker.
    public async Task<(bool Success, string Error)> CopyGlobalToPrivateAsync(int userId, int templateId)
    {
        var template = await _mealTemplateRepository.GetTemplateByIdAsync(templateId);
        if (template is null) return (false, "Template niet gevonden.");
        if (!template.IsGlobal) return (false, "Alleen globale templates kunnen worden gekopieerd.");

        var request = new CreateTemplateRequest
        {
            Name              = template.Name + " (kopie)",
            MealSlot          = template.MealSlot,
            TotalCalories     = template.TotalCalories,
            ProteinGrams      = template.ProteinGrams,
            CarbsGrams        = template.CarbsGrams,
            FatsGrams         = template.FatsGrams,
            IngredientSummary = template.IngredientSummary
        };

        await _mealTemplateRepository.CreateUserTemplateAsync(userId, request);
        return (true, "");
    }
}
