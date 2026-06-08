using BulkBuddy.Business.Models.Domain;
using BulkBuddy.Business.Services;
using Xunit;
using Xunit.Abstractions;

namespace BulkBuddy.Tests;

// Test: calorieberekening op basis van Mifflin-St Jeor formule.
public class CalorieCalculatorServiceTests
{
    private readonly CalorieCalculatorService _sut = new();
    private readonly ITestOutputHelper _output;

    public CalorieCalculatorServiceTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private static User MakeUser(string goalPhase = "maintain") => new()
    {
        Age = 22,
        HeightCm = 180,
        WeightKg = 80,
        ActivityMultiplier = 1.55m,
        GoalPhase = goalPhase,
        Sex = "Man"
    };

    [Fact]
    public void CalculateDailyCalories_ValidUser_ReturnsPositiveValue()
    {
        var result = _sut.CalculateDailyCalories(MakeUser());

        _output.WriteLine($"Uitkomst: {result} kcal");
        Assert.True(result > 0);
    }

    [Fact]
    public void CalculateDailyCalories_BulkPhase_ReturnsMoreThanMaintain()
    {
        var bulk     = _sut.CalculateDailyCalories(MakeUser("bulk"));
        var maintain = _sut.CalculateDailyCalories(MakeUser("maintain"));

        _output.WriteLine($"Bulk: {bulk} kcal | Maintain: {maintain} kcal");
        Assert.True(bulk > maintain);
    }

    [Fact]
    public void CalculateDailyCalories_CutPhase_ReturnsLessThanMaintain()
    {
        var cut      = _sut.CalculateDailyCalories(MakeUser("cut"));
        var maintain = _sut.CalculateDailyCalories(MakeUser("maintain"));

        _output.WriteLine($"Cut: {cut} kcal | Maintain: {maintain} kcal");
        Assert.True(cut < maintain);
    }

    [Fact]
    public void CalculateDailyCalories_BulkVsCut_DifferencesIs600()
    {
        var bulk = _sut.CalculateDailyCalories(MakeUser("bulk"));
        var cut  = _sut.CalculateDailyCalories(MakeUser("cut"));

        _output.WriteLine($"Bulk: {bulk} kcal | Cut: {cut} kcal | Verschil: {bulk - cut}");
        Assert.Equal(600, bulk - cut);
    }

    [Fact]
    public void CalculateDailyCalories_InvalidAge_ReturnsZero()
    {
        var user = MakeUser() with { Age = 0 };
        var result = _sut.CalculateDailyCalories(user);

        _output.WriteLine($"Leeftijd 0 → uitkomst: {result} (verwacht: 0)");
        Assert.Equal(0, result);
    }

    [Fact]
    public void CalculateDailyCalories_InvalidWeight_ReturnsZero()
    {
        var user = MakeUser() with { WeightKg = 0 };
        var result = _sut.CalculateDailyCalories(user);

        _output.WriteLine($"Gewicht 0 → uitkomst: {result} (verwacht: 0)");
        Assert.Equal(0, result);
    }
}
