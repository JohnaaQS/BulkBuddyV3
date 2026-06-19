using BulkBuddy.Business.Models;
using BulkBuddy.Business.Repositories;

namespace BulkBuddy.Business.Services;

// Bouwt alle data op die het dashboard nodig heeft.
// DatabaseException wordt gegooid door de repositories in DataAccess — niet hier.
public class DashboardService
{
    private readonly IUserRepository _userRepository;
    private readonly IMealRepository _mealRepository;
    private readonly CalorieCalculatorService _calorieCalculatorService;

    public DashboardService(
        IUserRepository userRepository,
        IMealRepository mealRepository,
        CalorieCalculatorService calorieCalculatorService)
    {
        _userRepository = userRepository;
        _mealRepository = mealRepository;
        _calorieCalculatorService = calorieCalculatorService;
    }

    public async Task<DashboardData?> GetDashboardAsync(int userId) 
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null) return null;

        var calorieTarget = _calorieCalculatorService.CalculateDailyCalories(user);

        // Parallel i.p.v. sequentieel — consistent met MealsPageService
        var caloriesTodayTask    = _mealRepository.GetTotalCaloriesForTodayAsync(user.Id);
        var mealsLoggedTodayTask = _mealRepository.GetMealCountForTodayAsync(user.Id);
        await Task.WhenAll(caloriesTodayTask, mealsLoggedTodayTask);

        var caloriesToday    = caloriesTodayTask.Result;
        var mealsLoggedToday = mealsLoggedTodayTask.Result;

        var remainingCalories = Math.Max(0, calorieTarget - caloriesToday);
        var progressPercent   = calorieTarget <= 0
            ? 0
            : Math.Min(100, (int)Math.Round((double)caloriesToday / calorieTarget * 100));

        return new DashboardData
        {
            Username               = user.Username,
            WeightKg               = user.WeightKg,
            HeightCm               = user.HeightCm,
            TargetWeightKg         = user.TargetWeightKg,
            GoalPhase              = user.GoalPhase,
            CalorieTarget          = calorieTarget,
            CaloriesToday          = caloriesToday,
            MealsLoggedToday       = mealsLoggedToday,
            RemainingCalories      = remainingCalories,
            CalorieProgressPercent = progressPercent
        };
    }
}
