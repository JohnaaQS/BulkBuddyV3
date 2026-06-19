using BulkBuddy.Business.Exceptions;
using BulkBuddy.Business.Models;
using BulkBuddy.Business.Services;
using BulkBuddy.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BulkBuddy.Web.Controllers;

// Controller voor accountbeheer.
public class AccountController : Controller
{
    private readonly AuthenticationService _authenticationService;

    public AccountController(AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (HttpContext.Session.GetInt32("UserId") is not null)
        {
            return RedirectToAction("Index", "Dashboard");
        }

        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            var user = await _authenticationService.LoginAsync(model.UsernameOrEmail, model.Password);

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Ongeldige inloggegevens.");
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);

            return RedirectToAction("Index", "Dashboard");
        }
        catch (DatabaseException)
        {
            // Web kent alleen DatabaseException — niet NpgsqlException.
            // Business vangt Npgsql op en gooit DatabaseException.
            return View("~/Views/Shared/DatabaseUnavailable.cshtml");
        }
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (HttpContext.Session.GetInt32("UserId") is not null)
        {
            return RedirectToAction("Index", "Dashboard");
        }

        return View(new RegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            var request = new RegisterRequest
            {
                Username                 = model.Username,
                Email                    = model.Email,
                Password                 = model.Password,
                Age                      = model.Age,
                HeightCm                 = model.HeightCm,
                WeightKg                 = model.WeightKg,
                TargetWeightKg           = model.TargetWeightKg,
                Goal                     = model.Goal,
                GoalPhase                = model.GoalPhase,
                Sex                      = model.Sex,
                TrainingFrequencyPerWeek = model.TrainingFrequencyPerWeek,
                ActivityMultiplier       = model.ActivityMultiplier
            };

            var result = await _authenticationService.RegisterAsync(request);

            if (!result.Success || result.UserId is null)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", result.UserId.Value);
            HttpContext.Session.SetString("Username", model.Username);

            return RedirectToAction("Index", "Dashboard");
        }
        catch (DatabaseException)
        {
            return View("~/Views/Shared/DatabaseUnavailable.cshtml");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Account");
    }
}
