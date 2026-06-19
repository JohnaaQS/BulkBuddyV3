// dotnet test BulkBuddy.Tests/BulkBuddy.Tests.csproj
using BulkBuddy.Business.Models.Domain;
using BulkBuddy.Business.Repositories;
using BulkBuddy.Business.Services;
using Xunit;
using Xunit.Abstractions;

namespace BulkBuddy.Tests;

// ─────────────────────────────────────────────────────────────────────────────
// HarrisBenedictStrategy — directe tests van de formule
// ─────────────────────────────────────────────────────────────────────────────
public class HarrisBenedictStrategyTests
{
    private readonly CalorieCalculatorService _sut = new(new HarrisBenedictStrategy());
    private readonly ITestOutputHelper _output;

    public HarrisBenedictStrategyTests(ITestOutputHelper output)
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
    public void Calculate_ValidUser_ReturnsPositiveValue()
    {
        var result = _sut.CalculateDailyCalories(MakeUser());

        _output.WriteLine($"Harris-Benedict uitkomst: {result} kcal");
        Assert.True(result > 0);
    }

    [Fact]
    public void Calculate_BulkPhase_ReturnsMoreThanMaintain()
    {
        var bulk     = _sut.CalculateDailyCalories(MakeUser("bulk"));
        var maintain = _sut.CalculateDailyCalories(MakeUser("maintain"));

        _output.WriteLine($"Bulk: {bulk} | Maintain: {maintain}");
        Assert.True(bulk > maintain);
    }

    [Fact]
    public void Calculate_CutPhase_ReturnsLessThanMaintain()
    {
        var cut      = _sut.CalculateDailyCalories(MakeUser("cut"));
        var maintain = _sut.CalculateDailyCalories(MakeUser("maintain"));

        _output.WriteLine($"Cut: {cut} | Maintain: {maintain}");
        Assert.True(cut < maintain);
    }

    [Fact]
    public void Calculate_BulkVsCut_DifferenceIs600()
    {
        var bulk = _sut.CalculateDailyCalories(MakeUser("bulk"));
        var cut  = _sut.CalculateDailyCalories(MakeUser("cut"));

        _output.WriteLine($"Bulk: {bulk} | Cut: {cut} | Verschil: {bulk - cut}");
        Assert.Equal(600, bulk - cut);
    }

    [Fact]
    public void Calculate_InvalidAge_ReturnsZero()
    {
        var user   = MakeUser() with { Age = 0 };
        var result = _sut.CalculateDailyCalories(user);

        _output.WriteLine($"Leeftijd 0 → uitkomst: {result} (verwacht: 0)");
        Assert.Equal(0, result);
    }

    [Fact]
    public void Calculate_InvalidWeight_ReturnsZero()
    {
        var user   = MakeUser() with { WeightKg = 0 };
        var result = _sut.CalculateDailyCalories(user);

        _output.WriteLine($"Gewicht 0 → uitkomst: {result} (verwacht: 0)");
        Assert.Equal(0, result);
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// Strategy Pattern — OCP en polymorfisme
// Bewijst dat CalorieCalculatorService met elke ICalorieStrategy werkt
// zonder zelf aangepast te worden (OCP) en dat beide strategieen
// uitwisselbaar zijn (LSP).
// ─────────────────────────────────────────────────────────────────────────────
public class CalorieStrategyPolymorphismTests
{
    private readonly ITestOutputHelper _output;

    public CalorieStrategyPolymorphismTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private static User StandardUser(string goalPhase = "maintain") => new()
    {
        Age = 22,
        HeightCm = 180,
        WeightKg = 80,
        ActivityMultiplier = 1.55m,
        GoalPhase = goalPhase,
        Sex = "Man"
    };

    // OCP: CalorieCalculatorService wordt niet aangepast bij wisselen van strategie.
    // LSP: beide strategieen zijn uitwisselbaar als ICalorieStrategy.
    [Theory]
    [InlineData(nameof(MifflinStJeorStrategy))]
    [InlineData(nameof(HarrisBenedictStrategy))]
    public void AnyStrategy_ValidUser_ReturnsPositiveValue(string strategyName)
    {
        ICalorieStrategy strategy = strategyName switch
        {
            nameof(MifflinStJeorStrategy)  => new MifflinStJeorStrategy(),
            nameof(HarrisBenedictStrategy) => new HarrisBenedictStrategy(),
            _                              => throw new ArgumentException(strategyName)
        };

        var sut    = new CalorieCalculatorService(strategy);
        var result = sut.CalculateDailyCalories(StandardUser());

        _output.WriteLine($"[{strategyName}] uitkomst: {result} kcal");
        Assert.True(result > 0);
    }

    // Bewijst dat beide formules een ander getal geven (ze zijn niet identiek).
    [Fact]
    public void MifflinVsHarris_SameUser_ProduceDifferentResults()
    {
        var user    = StandardUser();
        var mifflin = new CalorieCalculatorService(new MifflinStJeorStrategy()).CalculateDailyCalories(user);
        var harris  = new CalorieCalculatorService(new HarrisBenedictStrategy()).CalculateDailyCalories(user);

        _output.WriteLine($"Mifflin-St Jeor: {mifflin} kcal | Harris-Benedict: {harris} kcal");
        Assert.NotEqual(mifflin, harris);
    }

    // OCP: wisselen van strategie verandert het resultaat zonder CalorieCalculatorService aan te passen.
    [Fact]
    public void SwappingStrategy_ChangesResult_WithoutModifyingService()
    {
        var user = StandardUser("bulk");

        var resultMifflin = new CalorieCalculatorService(new MifflinStJeorStrategy()).CalculateDailyCalories(user);
        var resultHarris  = new CalorieCalculatorService(new HarrisBenedictStrategy()).CalculateDailyCalories(user);

        _output.WriteLine($"Strategie gewisseld → Mifflin: {resultMifflin} | Harris: {resultHarris}");

        // Beide zijn positief en 600 meer dan hun respectieve cut-waarde.
        Assert.True(resultMifflin > 0);
        Assert.True(resultHarris > 0);
        Assert.NotEqual(resultMifflin, resultHarris);
    }

    // LSP: een MockStrategy (nep-implementatie) is ook geldig als ICalorieStrategy.
    [Fact]
    public void MockStrategy_CanReplaceAnyStrategy_LSP()
    {
        ICalorieStrategy mock = new FixedCalorieStrategy(2500);
        var sut               = new CalorieCalculatorService(mock);
        var result            = sut.CalculateDailyCalories(StandardUser());

        _output.WriteLine($"MockStrategy uitkomst: {result} kcal (verwacht: 2500)");
        Assert.Equal(2500, result);
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// Hulpklasse voor LSP-test — implementeert ICalorieStrategy met vaste waarde.
// Vervangt een echte strategie volledig zonder CalorieCalculatorService aan te passen.
// ─────────────────────────────────────────────────────────────────────────────
file sealed class FixedCalorieStrategy(int fixedValue) : ICalorieStrategy
{
    public int Calculate(User user) => fixedValue;
}
