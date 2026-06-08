using BulkBuddy.Business.Models.ViewModels;
using BulkBuddy.Business.Repositories;

namespace BulkBuddy.Business.Services;

// Bouwt de data op voor de meals overzichtspagina.
public class MealsPageService
{
    private readonly IUserRepository _userRepository;
    private readonly IMealRepository _mealRepository;

    // Constructor die de benodigde repositories injecteert.
    public MealsPageService(IUserRepository userRepository, IMealRepository mealRepository)
    {
        _userRepository = userRepository;
        _mealRepository = mealRepository;
    }

    // Haal alle data op die nodig is voor de meals overzichtspagina van de gebruiker.
    public async Task<MealsIndexViewModel?> GetMealsIndexAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            return null;
        }

        // Drie repository-calls parallel uitvoeren i.p.v. sequentieel.
        // Sequentieel: ~30ms + ~30ms + ~30ms = ~90ms totaal.
        // Parallel: max(~30ms, ~30ms, ~30ms) = ~30ms totaal.
        // Task.WhenAll wacht tot alle drie klaar zijn voor we verder gaan.
        var mealsTask        = _mealRepository.GetMealsForTodayAsync(userId);
        var totalCalTask     = _mealRepository.GetTotalCaloriesForTodayAsync(userId);
        var mealCountTask    = _mealRepository.GetMealCountForTodayAsync(userId);

        await Task.WhenAll(mealsTask, totalCalTask, mealCountTask);

        var meals         = mealsTask.Result;
        var totalCalories = totalCalTask.Result;
        var mealCount     = mealCountTask.Result;

        // Bouw en retourneer het ViewModel voor de meals overzichtspagina.
        return new MealsIndexViewModel
        {
            Username = user.Username,
            SelectedDate = DateTime.Today,
            TotalCaloriesToday = totalCalories,
            MealsLoggedToday = mealCount,
            Meals = meals
        };
    }

    // Sla een nieuwe maaltijd op voor de gebruiker.
    // De data uit het formulier (of template) wordt als snapshot opgeslagen in meal_entries.
    public async Task AddMealAsync(int userId, AddMealViewModel model)
    {
        await _mealRepository.AddMealAsync(userId, model);
    }
}
