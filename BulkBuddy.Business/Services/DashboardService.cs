using BulkBuddy.Business.Models.ViewModels;
using BulkBuddy.Business.Repositories;

namespace BulkBuddy.Business.Services;

// Bouwt alle data op die het dashboard nodig heeft.
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

    public async Task<DashboardViewModel?> GetDashboardAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            return null;
        }

        var calorieTarget = _calorieCalculatorService.CalculateDailyCalories(user);
        var caloriesToday = await _mealRepository.GetTotalCaloriesForTodayAsync(user.Id);
        var mealsLoggedToday = await _mealRepository.GetMealCountForTodayAsync(user.Id);

        var remainingCalories = Math.Max(0, calorieTarget - caloriesToday);
        var progressPercent = calorieTarget <= 0
            ? 0
            : Math.Min(100, (int)Math.Round((double)caloriesToday / calorieTarget * 100));

        return new DashboardViewModel
        {
            Username = user.Username,
            WeightKg = user.WeightKg,
            HeightCm = user.HeightCm,
            TargetWeightKg = user.TargetWeightKg,
            GoalPhase = user.GoalPhase,
            CalorieTarget = calorieTarget,
            WelcomeMessage = BuildWelcomeMessage(user.Username, user.GoalPhase),
            GoalSummary = BuildGoalSummary(user.WeightKg, user.TargetWeightKg, user.GoalPhase),
            CaloriesToday = caloriesToday,
            MealsLoggedToday = mealsLoggedToday,
            RemainingCalories = remainingCalories,
            CalorieProgressPercent = progressPercent
        };
    }

    private static string BuildWelcomeMessage(string username, string goalPhase)
    {
        return $"Welkom terug, {username}. Je dashboard staat klaar voor je {goalPhase}-fase.";
    }

    private static string BuildGoalSummary(decimal currentWeight, decimal targetWeight, string goalPhase)
    {
        return $"Je huidige gewicht is {currentWeight} kg en je doelgewicht is {targetWeight} kg tijdens je {goalPhase}-fase.";
    }
}
