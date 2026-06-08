using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppMTGLogic.Interfaces;

namespace WebAppMTG.Pages
{
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

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                int userId = 1; // tijdelijk hardcoded

                var deckId = await _userDeckService.CreateDeckAsync(userId, Name, Format, Description);

                return RedirectToPage("/Decks/Details", new { id = deckId });
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
