using BulkBuddy.Business.Services;
using Microsoft.AspNetCore.Mvc;
using BulkBuddy.Business.Models.ViewModels;
using Npgsql;

namespace BulkBuddy.Web.Controllers;

// Controller voor de meals overzichtspagina.
public class MealsController : Controller
{
    private readonly MealsPageService _mealsPageService;
    private readonly MealTemplateService _mealTemplateService;

    public MealsController(MealsPageService mealsPageService, MealTemplateService mealTemplateService)
    {
        _mealsPageService = mealsPageService;
        _mealTemplateService = mealTemplateService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        try
        {
            var viewModel = await _mealsPageService.GetMealsIndexAsync(userId.Value);

            if (viewModel is null)
            {
                return NotFound();
            }

            return View(viewModel);
        }
        catch (NpgsqlException)
        {
            return View("~/Views/Shared/DatabaseUnavailable.cshtml");
        }
    }
    [HttpGet]
    public async Task<IActionResult> Saved()
    {
        var viewModel = await _mealTemplateService.GetSavedMealsAsync();
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Add()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        var viewModel = new AddMealViewModel
        {
            MealDate = DateTime.Today
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(AddMealViewModel model)
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            // Data wordt als snapshot opgeslagen — geen FK naar template.
            // Of het een banaan of een template was maakt niet uit: alles wordt gekopieerd.
            await _mealsPageService.AddMealAsync(userId.Value, model);
            return RedirectToAction("Index");
        }
        catch (NpgsqlException)
        {
            return View("~/Views/Shared/DatabaseUnavailable.cshtml");
        }
    }
}