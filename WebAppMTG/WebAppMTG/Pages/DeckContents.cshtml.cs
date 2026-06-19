using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppMTGLogic.Database.Interfaces;
using WebAppMTGLogic.Database.Models;
using WebAppMTGModelsDL.Exceptions;
using WebAppMTGModelsDL.Exeptions;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace WebAppMTG.Pages
{
    [Authorize]
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
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return RedirectToPage("/Login");
            }

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
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return RedirectToPage("/Login");
            }

            try
            {
                await _userDeckService.RemoveCardFromDeckAsync(userId.Value, itemId, cardEntryId);
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

            await ReloadDeckDataAsync(itemId);
            return Page();
        }

        public async Task<IActionResult> OnPostRenameDeckAsync(int itemId)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return RedirectToPage("/Login");
            }

            try
            {
                await _userDeckService.RenameDeckAsync(userId.Value, itemId, NewDeckName);
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

            await ReloadDeckDataAsync(itemId);
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteDeckAsync(int itemId)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return RedirectToPage("/Login");
            }

            try
            {
                await _userDeckService.DeleteDeckAsync(userId.Value, itemId);
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

            await ReloadDeckDataAsync(itemId);
            return Page();
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

        private async Task ReloadDeckDataAsync(int deckId)
        {
            try
            {
                Deck = await _userDeckService.GetDeckByIdAsync(deckId);
                LegalityResult = await _userDeckService.ValidateDeckAsync(deckId);
            }
            catch
            {
            }
        }
    }
}
