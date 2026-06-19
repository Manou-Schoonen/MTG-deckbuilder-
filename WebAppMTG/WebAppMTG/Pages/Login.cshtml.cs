using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using WebAppMTGLogic.Database.Interfaces;

namespace WebAppMTG.Pages
{
    
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;

        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _userService.LoginAsync(Input.Email, Input.Password);

            if (!result.Success || result.User == null)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Inloggen mislukt.");
                return Page();
            }

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, result.User.Id.ToString()),
            new Claim(ClaimTypes.Name, result.User.Name),
            new Claim(ClaimTypes.Email, result.User.Email)
        };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            return RedirectToPage("/Index");
        }

        public class LoginInputModel
        {
            [Required(ErrorMessage = "E-mail is verplicht.")]
            [EmailAddress(ErrorMessage = "Voer een geldig e-mailadres in.")]
            [Display(Name = "E-mail")]
            public string Email { get; set; } = "";

            [Required(ErrorMessage = "Wachtwoord is verplicht.")]
            [DataType(DataType.Password)]
            [Display(Name = "Wachtwoord")]
            public string Password { get; set; } = "";
        }
    }
}
