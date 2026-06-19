using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using WebAppMTGLogic.Database.Interfaces;

namespace WebAppMTG.Pages
{

    public class RegisterModel : PageModel
    {
        private readonly IUserService _userService;

        public RegisterModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public RegisterInputModel Input { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (Input.Password != Input.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Wachtwoorden komen niet overeen.");
                return Page();
            }

            try
            {
                await _userService.RegisterUserAsync(Input.Name, Input.Email, Input.Password);

                return RedirectToPage("/Login");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        public class RegisterInputModel
        {
            [Required(ErrorMessage = "Naam is verplicht.")]
            [Display(Name = "Naam")]
            public string Name { get; set; } = "";

            [Required(ErrorMessage = "E-mail is verplicht.")]
            [EmailAddress(ErrorMessage = "Voer een geldig e-mailadres in.")]
            [Display(Name = "E-mail")]
            public string Email { get; set; } = "";

            [Required(ErrorMessage = "Wachtwoord is verplicht.")]
            [DataType(DataType.Password)]
            [Display(Name = "Wachtwoord")]
            public string Password { get; set; } = "";

            [Required(ErrorMessage = "Bevestig je wachtwoord.")]
            [DataType(DataType.Password)]
            [Display(Name = "Bevestig wachtwoord")]
            public string ConfirmPassword { get; set; } = "";
        }
    }
}
