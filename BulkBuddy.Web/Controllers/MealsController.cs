using BulkBuddy.Business.Exceptions;
using BulkBuddy.Business.Models;
using BulkBuddy.Business.Services;
using BulkBuddy.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BulkBuddy.Web.Controllers;

public class MealsController : Controller
{
    private readonly MealsPageService _mealsPageService;
    private readonly MealTemplateService _mealTemplateService;

    public MealsController(MealsPageService mealsPageService, MealTemplateService mealTemplateService)
    {
        _mealsPageService = mealsPageService;
        _mealTemplateService = mealTemplateService;
    }

    // ── Meals dagoverzicht ──────────────────────────────────────────────────

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId is null) return RedirectToAction("Login", "Account");

        try
        {
            var data = await _mealsPageService.GetMealsIndexAsync(userId.Value);
            if (data is null) return NotFound();

            var viewModel = new MealsIndexViewModel
            {
                Username           = data.Username,
                SelectedDate       = data.SelectedDate,
                TotalCaloriesToday = data.TotalCaloriesToday,
                MealsLoggedToday   = data.MealsLoggedToday,
                Meals              = data.Meals.Select(m => new MealEntryViewModel
                {
                    Id                = m.Id,
                    Name              = m.Name,
                    MealSlot          = m.MealSlot,
                    TotalCalories     = m.TotalCalories,
                    ProteinGrams      = m.ProteinGrams,
                    CarbsGrams        = m.CarbsGrams,
                    FatsGrams         = m.FatsGrams,
                    IngredientSummary = m.IngredientSummary,
                    MealDate          = m.MealDate
                }).ToList()
            };

            return View(viewModel);
        }
        catch (DatabaseException)
        {
            return View("~/Views/Shared/DatabaseUnavailable.cshtml");
        }
    }

    [HttpGet]
    public IActionResult Add()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId is null) return RedirectToAction("Login", "Account");

        return View(new AddMealViewModel { MealDate = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(AddMealViewModel model)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId is null) return RedirectToAction("Login", "Account");

        if (!ModelState.IsValid) return View(model);

        try
        {
            var request = new AddMealRequest
            {
                Name              = model.Name,
                MealSlot          = model.MealSlot,
                MealDate          = model.MealDate,
                TotalCalories     = model.TotalCalories,
                ProteinGrams      = model.ProteinGrams,
                CarbsGrams        = model.CarbsGrams,
                FatsGrams         = model.FatsGrams,
                IngredientSummary = model.IngredientSummary
            };

            await _mealsPageService.AddMealAsync(userId.Value, request);
            return RedirectToAction("Index");
        }
        catch (DatabaseException)
        {
            return View("~/Views/Shared/DatabaseUnavailable.cshtml");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Fout: {ex.Message}");
            return View(model);
        }
    }

    // ── Saved Meals / Templates ─────────────────────────────────────────────

    [HttpGet]
    public async Task<IActionResult> Saved()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId is null) return RedirectToAction("Login", "Account");

        try
        {
            var cards = await _mealTemplateService.GetTemplatesForUserAsync(userId.Value);
            var viewModel = new SavedMealsViewModel
            {
                Meals = cards.Select(c => new SavedMealCardViewModel
                {
                    Id                = c.Id,
                    Name              = c.Name,
                    MealSlot          = c.MealSlot,
                    IsGlobal          = c.IsGlobal,
                    UserId            = c.UserId,
                    TotalCalories     = c.TotalCalories,
                    ProteinGrams      = c.ProteinGrams,
                    CarbsGrams        = c.CarbsGrams,
                    FatsGrams         = c.FatsGrams,
                    IngredientSummary = c.IngredientSummary
                }).ToList()
            };
            return View(viewModel);
        }
        catch (DatabaseException)
        {
            return View("~/Views/Shared/DatabaseUnavailable.cshtml");
        }
    }

    [HttpGet]
    public IActionResult NewTemplate()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId is null) return RedirectToAction("Login", "Account");

        return View(new TemplateFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NewTemplate(TemplateFormViewModel model)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId is null) return RedirectToAction("Login", "Account");

        if (!ModelState.IsValid) return View(model);

        try
        {
            var request = new CreateTemplateRequest
            {
                Name              = model.Name,
                MealSlot          = model.MealSlot,
                TotalCalories     = model.TotalCalories,
                ProteinGrams      = model.ProteinGrams,
                CarbsGrams        = model.CarbsGrams,
                FatsGrams         = model.FatsGrams,
                IngredientSummary = model.IngredientSummary
            };

            await _mealTemplateService.CreatePrivateTemplateAsync(userId.Value, request);
            return RedirectToAction("Saved");
        }
        catch (DatabaseException)
        {
            return View("~/Views/Shared/DatabaseUnavailable.cshtml");
        }
    }

    [HttpGet]
    public async Task<IActionResult> EditTemplate(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId is null) return RedirectToAction("Login", "Account");

        try
        {
            var template = await _mealTemplateService.GetTemplateByIdForUserAsync(userId.Value, id);
            if (template is null || template.IsGlobal) return Forbid();

            var viewModel = new TemplateFormViewModel
            {
                Id                = template.Id,
                Name              = template.Name,
                MealSlot          = template.MealSlot,
                TotalCalories     = template.TotalCalories,
                ProteinGrams      = template.ProteinGrams,
                CarbsGrams        = template.CarbsGrams,
                FatsGrams         = template.FatsGrams,
                IngredientSummary = template.IngredientSummary
            };

            return View(viewModel);
        }
        catch (DatabaseException)
        {
            return View("~/Views/Shared/DatabaseUnavailable.cshtml");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditTemplate(TemplateFormViewModel model)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId is null) return RedirectToAction("Login", "Account");

        if (!ModelState.IsValid) return View(model);

        try
        {
            var request = new CreateTemplateRequest
            {
                Name              = model.Name,
                MealSlot          = model.MealSlot,
                TotalCalories     = model.TotalCalories,
                ProteinGrams      = model.ProteinGrams,
                CarbsGrams        = model.CarbsGrams,
                FatsGrams         = model.FatsGrams,
                IngredientSummary = model.IngredientSummary
            };

            var (success, error) = await _mealTemplateService.UpdatePrivateTemplateAsync(userId.Value, model.Id!.Value, request);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, error);
                return View(model);
            }

            return RedirectToAction("Saved");
        }
        catch (DatabaseException)
        {
            return View("~/Views/Shared/DatabaseUnavailable.cshtml");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteTemplate(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId is null) return RedirectToAction("Login", "Account");

        try
        {
            await _mealTemplateService.DeletePrivateTemplateAsync(userId.Value, id);
            return RedirectToAction("Saved");
        }
        catch (DatabaseException)
        {
            return View("~/Views/Shared/DatabaseUnavailable.cshtml");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CopyTemplate(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId is null) return RedirectToAction("Login", "Account");

        try
        {
            var (success, error) = await _mealTemplateService.CopyGlobalToPrivateAsync(userId.Value, id);
            if (!success) TempData["Error"] = error;
            return RedirectToAction("Saved");
        }
        catch (DatabaseException)
        {
            return View("~/Views/Shared/DatabaseUnavailable.cshtml");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToToday(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId is null) return RedirectToAction("Login", "Account");

        try
        {
            var template = await _mealTemplateService.GetTemplateByIdForUserAsync(userId.Value, id);
            if (template is null)
            {
                TempData["Error"] = "Template niet gevonden of geen toegang.";
                return RedirectToAction("Saved");
            }

            var request = new AddMealRequest
            {
                Name              = template.Name,
                MealSlot          = template.MealSlot,
                MealDate          = DateTime.Today,
                TotalCalories     = template.TotalCalories,
                ProteinGrams      = template.ProteinGrams,
                CarbsGrams        = template.CarbsGrams,
                FatsGrams         = template.FatsGrams,
                IngredientSummary = template.IngredientSummary
            };

            await _mealsPageService.AddMealAsync(userId.Value, request);
            return RedirectToAction("Index");
        }
        catch (DatabaseException)
        {
            return View("~/Views/Shared/DatabaseUnavailable.cshtml");
        }
    }
}