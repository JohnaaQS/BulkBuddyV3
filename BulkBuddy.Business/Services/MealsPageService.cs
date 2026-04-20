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

        // Haal de meals van vandaag, het totale aantal calorieën en het aantal gelogde meals op voor de gebruiker.
        var meals = await _mealRepository.GetMealsForTodayAsync(userId);
        var totalCalories = await _mealRepository.GetTotalCaloriesForTodayAsync(userId);
        var mealCount = await _mealRepository.GetMealCountForTodayAsync(userId);

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
}
