using BulkBuddy.Business.Models;
using BulkBuddy.Business.Repositories;

namespace BulkBuddy.Business.Services;

// Bouwt de data op voor de meals overzichtspagina.
// DatabaseException wordt gegooid door de repositories in DataAccess — niet hier.
public class MealsPageService
{
    private readonly IUserRepository _userRepository;
    private readonly IMealRepository _mealRepository;

    public MealsPageService(IUserRepository userRepository, IMealRepository mealRepository)
    {
        _userRepository = userRepository;
        _mealRepository = mealRepository;
    }

    public async Task<MealsIndexData?> GetMealsIndexAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null) return null;

        // Drie repository-calls parallel — ~30ms i.p.v. ~90ms sequentieel
        var mealsTask     = _mealRepository.GetMealsForTodayAsync(userId);
        var totalCalTask  = _mealRepository.GetTotalCaloriesForTodayAsync(userId);
        var mealCountTask = _mealRepository.GetMealCountForTodayAsync(userId);

        await Task.WhenAll(mealsTask, totalCalTask, mealCountTask);

        return new MealsIndexData
        {
            Username           = user.Username,
            SelectedDate       = DateTime.Today,
            TotalCaloriesToday = totalCalTask.Result,
            MealsLoggedToday   = mealCountTask.Result,
            Meals              = mealsTask.Result
        };
    }

    // Data wordt als snapshot opgeslagen — geen FK naar template.
    public async Task AddMealAsync(int userId, AddMealRequest model)
    {
        await _mealRepository.AddMealAsync(userId, model);
    }
}
