using System.ComponentModel.DataAnnotations;

namespace BulkBuddy.Web.Models.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Gebruikersnaam of e-mail is verplicht.")]
    [Display(Name = "Gebruikersnaam of e-mail")]
    public string UsernameOrEmail { get; set; } = "";

    [Required(ErrorMessage = "Wachtwoord is verplicht.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";
}
