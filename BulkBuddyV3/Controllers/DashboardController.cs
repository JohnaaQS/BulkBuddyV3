using BulkBuddy.Services;
using Microsoft.AspNetCore.Mvc;

namespace BulkBuddy.Controllers;

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
        if (HttpContext.Session.GetInt32("UserId") is null)
        {
            return RedirectToAction("Login", "Account");
        }

        var viewModel = await _dashboardService.GetDashboardAsync();

        if (viewModel is null)
        {
            return View("Empty");
        }

        return View(viewModel);
    }
}