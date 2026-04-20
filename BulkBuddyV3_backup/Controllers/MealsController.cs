using BulkBuddy.Services;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace BulkBuddy.Controllers;

// Controller voor de meals overzichtspagina.
public class MealsController : Controller
{
    private readonly MealsPageService _mealsPageService;

    public MealsController(MealsPageService mealsPageService)
    {
        _mealsPageService = mealsPageService;
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
}