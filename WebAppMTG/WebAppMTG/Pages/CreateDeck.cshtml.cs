using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppMTGLogic.Database.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace WebAppMTG.Pages
{
    [Authorize]
    public class CreateDeckModel : PageModel
    {
        private readonly IUserDeckService _userDeckService;

        public CreateDeckModel(IUserDeckService userDeckService)
        {
            _userDeckService = userDeckService;
        }

        [BindProperty]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
        public string Format { get; set; } = "standard";

        [BindProperty]
        public string Description { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            if (!GetCurrentUserId().HasValue)
            {
                return RedirectToPage("/Login");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = GetCurrentUserId();

            if (!userId.HasValue)
            {
                return RedirectToPage("/Login");
            }

            try
            {
                var deckId = await _userDeckService.CreateDeckAsync(userId.Value, Name, Format, Description);
                return RedirectToPage("/DeckContents", new { id = deckId });
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }

        private int? GetCurrentUserId()
        {
            var userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userIdValue, out var userId))
            {
                return userId;
            }

            return null;
        }
    }
}
