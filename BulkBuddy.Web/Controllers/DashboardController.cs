using BulkBuddy.Business.Services;
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

        var viewModel = await _dashboardService.GetDashboardAsync(userId.Value);

        if (viewModel is null)
        {
            return View("Empty");
        }

        return View(viewModel);
    }
}
