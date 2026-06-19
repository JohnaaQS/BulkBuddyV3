using BulkBuddy.Business.Exceptions;
using BulkBuddy.Business.Services;
using BulkBuddy.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BulkBuddy.Web.Controllers;

// Controller voor de dashboardpagina.
public class DashboardController : Controller
{
    private readonly DashboardService _dashboardService;

    public DashboardController(DashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        try
        {
            var data = await _dashboardService.GetDashboardAsync(userId.Value);

            if (data is null) return View("Empty");

            var viewModel = new DashboardViewModel
            {
                Username               = data.Username,
                WeightKg               = data.WeightKg,
                HeightCm               = data.HeightCm,
                TargetWeightKg         = data.TargetWeightKg,
                GoalPhase              = data.GoalPhase,
                CalorieTarget          = data.CalorieTarget,
                CaloriesToday          = data.CaloriesToday,
                MealsLoggedToday       = data.MealsLoggedToday,
                RemainingCalories      = data.RemainingCalories,
                CalorieProgressPercent = data.CalorieProgressPercent
            };

            return View(viewModel);
        }
        catch (DatabaseException)
        {
            return View("~/Views/Shared/DatabaseUnavailable.cshtml");
        }
    }
}
