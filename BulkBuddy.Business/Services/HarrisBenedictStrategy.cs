using BulkBuddy.Business.Models.Domain;
using BulkBuddy.Business.Repositories;

namespace BulkBuddy.Business.Services;

// OCP: tweede implementatie van ICalorieStrategy — Harris-Benedict formule.
// CalorieCalculatorService, DashboardService en alle andere code is NIET aangepast.
// Om te wisselen: alleen in Program.cs AddScoped<ICalorieStrategy, HarrisBenedictStrategy>() zetten.
public class HarrisBenedictStrategy : ICalorieStrategy
{
    public int Calculate(User user)
    {
        if (user.Age <= 0 || user.HeightCm <= 0 || user.WeightKg <= 0 || user.ActivityMultiplier <= 0)
            return 0;

        // Harris-Benedict formule (mannelijk).
        var bmr = 88.362
                  + (13.397 * (double)user.WeightKg)
                  + (4.799 * (double)user.HeightCm)
                  - (5.677 * user.Age);

        var tdee = bmr * (double)user.ActivityMultiplier;

        return user.GoalPhase.ToLowerInvariant() switch
        {
            "bulk" => (int)Math.Round(tdee + 300),
            "cut"  => (int)Math.Round(tdee - 300),
            _      => (int)Math.Round(tdee)
        };
    }
}
