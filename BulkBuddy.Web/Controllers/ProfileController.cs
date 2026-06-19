using BulkBuddy.Business.Exceptions;
using BulkBuddy.Business.Models;
using BulkBuddy.Business.Services;
using BulkBuddy.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BulkBuddy.Web.Controllers;

// Controller voor de profielpagina.
public class ProfileController : Controller
{
    private readonly ProfileService _profileService;

    public ProfileController(ProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        var user = await _profileService.GetProfileAsync(userId.Value);

        if (user is null)
        {
            return NotFound();
        }

        var viewModel = new ProfileViewModel
        {
            Username                 = user.Username,
            Email                    = user.Email,
            Age                      = user.Age,
            HeightCm                 = user.HeightCm,
            WeightKg                 = user.WeightKg,
            TargetWeightKg           = user.TargetWeightKg,
            Goal                     = user.Goal,
            GoalPhase                = user.GoalPhase,
            Sex                      = user.Sex,
            TrainingFrequencyPerWeek = user.TrainingFrequencyPerWeek,
            ActivityMultiplier       = user.ActivityMultiplier
        };

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        var user = await _profileService.GetProfileAsync(userId.Value);

        if (user is null)
        {
            return NotFound();
        }

        var viewModel = new EditProfileViewModel
        {
            Username                 = user.Username,
            Email                    = user.Email,
            Age                      = user.Age,
            HeightCm                 = user.HeightCm,
            WeightKg                 = user.WeightKg,
            TargetWeightKg           = user.TargetWeightKg,
            Goal                     = user.Goal,
            GoalPhase                = user.GoalPhase,
            Sex                      = user.Sex,
            TrainingFrequencyPerWeek = user.TrainingFrequencyPerWeek,
            ActivityMultiplier       = user.ActivityMultiplier
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditProfileViewModel model)
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
            var request = new UpdateProfileRequest
            {
                Username                 = model.Username,
                Email                    = model.Email,
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

            var result = await _profileService.UpdateProfileAsync(userId.Value, request);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return View(model);
            }

            HttpContext.Session.SetString("Username", model.Username);
            return RedirectToAction("Index");
        }
        catch (DatabaseException)
        {
            return View("~/Views/Shared/DatabaseUnavailable.cshtml");
        }
    }
}
