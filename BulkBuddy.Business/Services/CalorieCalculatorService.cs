using BulkBuddy.Business.Models.Domain;
using BulkBuddy.Business.Repositories;

namespace BulkBuddy.Business.Services;

// OCP: CalorieCalculatorService is closed voor wijziging.
// De berekeningslogica zit in ICalorieStrategy — nieuwe formules toevoegen = nieuwe klasse, geen aanpassing hier.
public class CalorieCalculatorService
{
    private readonly ICalorieStrategy _strategy;

    public CalorieCalculatorService(ICalorieStrategy strategy)
    {
        _strategy = strategy;
    }

    public int CalculateDailyCalories(User user) => _strategy.Calculate(user);
}
