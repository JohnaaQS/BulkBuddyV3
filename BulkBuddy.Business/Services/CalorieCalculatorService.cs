using BulkBuddy.Business.Models.Domain;

namespace BulkBuddy.Business.Services;

// Bevat logica voor calorieberekeningen.
// Door dit apart te houden blijft de dashboardservice overzichtelijk.
public class CalorieCalculatorService
{
    public int CalculateDailyCalories(User user)
    {
        // Bescherm tegen ongeldige of lege invoer.
        if (user.Age <= 0 || user.HeightCm <= 0 || user.WeightKg <= 0 || user.ActivityMultiplier <= 0)
        {
            return 0;
        }

        // Simpele Mifflin-St Jeor formule voor demonstratiedoeleinden.
        var bmr = (10 * (double)user.WeightKg)
                  + (6.25 * (double)user.HeightCm)
                  - (5 * user.Age)
                  + 5;

        var tdee = bmr * (double)user.ActivityMultiplier;

        // Pas het calorieadvies aan op basis van de gekozen fase.
        return user.GoalPhase.ToLowerInvariant() switch
        {
            "bulk" => (int)Math.Round(tdee + 300),
            "cut" => (int)Math.Round(tdee - 300),
            _ => (int)Math.Round(tdee)
        };
    }
}
