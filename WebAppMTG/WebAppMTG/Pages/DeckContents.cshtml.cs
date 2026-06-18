using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppMTGLogic.Database.Interfaces;
using WebAppMTGLogic.Database.Models;
using WebAppMTGModelsDL.Exceptions;
using WebAppMTGModelsDL.Exeptions;

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

        [BindProperty]
        public string NewDeckName { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Deck = await _userDeckService.GetDeckByIdAsync(id);

                if (Deck == null)
                {
                    ErrorMessage = "Deck niet gevonden.";
                    return Page();
                }

                LegalityResult = await _userDeckService.ValidateDeckAsync(id);
                return Page();
            }
            catch (DatabaseUnavailableException)
            {
                ErrorMessage = "Het deck kon niet worden geladen omdat de database momenteel offline is.";
                return Page();
            }
            catch (ExternalApiUnavailableException)
            {
                ErrorMessage = "Het deck kon niet volledig worden geladen omdat de kaarten-API momenteel niet reageert.";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostRemoveCardAsync(int itemId, string cardEntryId)
        {
            try
            {
                int userId = 1;
                await _userDeckService.RemoveCardFromDeckAsync(userId, itemId, cardEntryId);
                return RedirectToPage(new { id = itemId });
            }
            catch (DatabaseUnavailableException)
            {
                ErrorMessage = "De kaart kon niet worden verwijderd omdat de database momenteel offline is.";
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            try
            {
                Deck = await _userDeckService.GetDeckByIdAsync(itemId);
                LegalityResult = await _userDeckService.ValidateDeckAsync(itemId);
            }
            catch
            {
            }

            return Page();
        }

        public async Task<IActionResult> OnPostRenameDeckAsync(int itemId)
        {
            try
            {
                int userId = 1; // tijdelijk hardcoded
                await _userDeckService.RenameDeckAsync(userId, itemId, NewDeckName);
                return RedirectToPage(new { id = itemId });
            }
            catch (DatabaseUnavailableException)
            {
                ErrorMessage = "De decknaam kon niet worden aangepast omdat de database momenteel offline is.";
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            try
            {
                Deck = await _userDeckService.GetDeckByIdAsync(itemId);
                LegalityResult = await _userDeckService.ValidateDeckAsync(itemId);
            }
            catch
            {
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteDeckAsync(int itemId)
        {
            try
            {
                int userId = 1; // tijdelijk hardcoded
                await _userDeckService.DeleteDeckAsync(userId, itemId);
                return RedirectToPage("/DecksOverview");
            }
            catch (DatabaseUnavailableException)
            {
                ErrorMessage = "Het deck kon niet worden verwijderd omdat de database momenteel offline is.";
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            try
            {
                Deck = await _userDeckService.GetDeckByIdAsync(itemId);
                LegalityResult = await _userDeckService.ValidateDeckAsync(itemId);
            }
            catch
            {
            }

            return Page();
        }
    }
}
