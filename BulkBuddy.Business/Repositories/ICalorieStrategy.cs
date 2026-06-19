using BulkBuddy.Business.Models.Domain;

namespace BulkBuddy.Business.Repositories;

// OCP: interface definieert het contract voor calorieberekeningen.
// Nieuwe formules kunnen worden toegevoegd zonder CalorieCalculatorService aan te passen.
public interface ICalorieStrategy
{
    int Calculate(User user);
}
