using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppMTGLogic.Database.Interfaces;
using WebAppMTGLogic.Database.Models;

namespace WebAppMTG.Pages
{
    public class DeckContentsModel : PageModel
    {
        private readonly IUserDeckService _userDeckService;

        public DeckContentsModel(IUserDeckService userDeckService)
        {
            _userDeckService = userDeckService;
        }

        public DeckModel? Deck { get; set; }
        public DeckLegalityResult? LegalityResult { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Deck = await _userDeckService.GetDeckByIdAsync(id);

            if (Deck == null)
            {
                return NotFound();
            }

            LegalityResult = await _userDeckService.ValidateDeckAsync(id);

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveCardAsync(int itemId, string cardEntryId)
        {
            try
            {
                int userId = 1; // tijdelijk hardcoded want ik ga misschien geen accounts toepassen
                await _userDeckService.RemoveCardFromDeckAsync(userId, itemId, cardEntryId);
                return RedirectToPage(new { id = itemId });
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                Deck = await _userDeckService.GetDeckByIdAsync(itemId);
                LegalityResult = await _userDeckService.ValidateDeckAsync(itemId);
                return Page();
            }
        }
    }
}
