using BulkBuddy.Business.Models.ViewModels;
using BulkBuddy.Business.Repositories;

namespace BulkBuddy.Business.Services;

public class MealTemplateService

{

    private readonly IMealTemplateRepository _mealTemplateRepository;

    public MealTemplateService(IMealTemplateRepository mealTemplateRepository)

    {

        _mealTemplateRepository = mealTemplateRepository;

    }

    public async Task<SavedMealsViewModel> GetSavedMealsAsync()

    {

        var meals = await _mealTemplateRepository.GetSavedMealsAsync();

        return new SavedMealsViewModel

        {

            Meals = meals

        };

    }

}
