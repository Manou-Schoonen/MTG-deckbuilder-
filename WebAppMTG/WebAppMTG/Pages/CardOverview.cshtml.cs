using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelsDL.Database.Models;
using WebAppMTG.wwwroot.Extensions;
using WebAppMTGLogic.API.Interfaces;
using WebAppMTGLogic.API.Models;
using WebAppMTGLogic.Database.Interfaces;
using WebAppMTGLogic.Database.Models;
using WebAppMTGModelsDL.Exceptions;
using WebAppMTGModelsDL.Exeptions;
using System.Security.Claims;

namespace WebAppMTG.Pages
{
    public class CardOverviewModel : PageModel
    {
        private readonly ICardLogicService _cardLogicService;
        private readonly IUserDeckService _userDeckService;

        public CardOverviewModel(ICardLogicService cardLogicService, IUserDeckService userDeckService)
        {
            _cardLogicService = cardLogicService;
            _userDeckService = userDeckService;
        }

        [BindProperty(SupportsGet = true)]
        public string? Query { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Name { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? TypeLine { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Color { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ManaValue { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Format { get; set; }

        public string? ApiErrorMessage { get; set; }
        public string? DeckErrorMessage { get; set; }

        public List<CardReturnModel> Cards { get; set; } = new();
        public List<DeckModel> UserDecks { get; set; } = new();

        public bool IsLoggedIn => User.Identity?.IsAuthenticated ?? false;

        public bool CanAddToDeck =>
            IsLoggedIn &&
            string.IsNullOrWhiteSpace(DeckErrorMessage) &&
            UserDecks.Any();

        [BindProperty]
        public int SelectedDeckId { get; set; }

        [BindProperty]
        public string SelectedCardId { get; set; } = string.Empty;

        [BindProperty]
        public int QuantityToAdd { get; set; } = 1;

        [BindProperty]
        public BoardPart SelectedBoardPart { get; set; } = BoardPart.Mainboard;

        public async Task OnGetAsync()
        {
            await LoadUserDecksAsync();
            await LoadCardsAsync();
        }

        public async Task<IActionResult> OnPostAddToDeckAsync()
        {
            var userId = GetCurrentUserId();

            if (!userId.HasValue)
            {
                DeckErrorMessage = "Je bent niet correct ingelogd. Log opnieuw in.";
                await LoadUserDecksAsync();
                await LoadCardsAsync();
                return Page();
            }

            if (SelectedDeckId <= 0)
            {
                DeckErrorMessage = "Kies een geldig deck.";
                await LoadUserDecksAsync();
                await LoadCardsAsync();
                return Page();
            }

            if (string.IsNullOrWhiteSpace(SelectedCardId))
            {
                DeckErrorMessage = "Geen kaart geselecteerd.";
                await LoadUserDecksAsync();
                await LoadCardsAsync();
                return Page();
            }

            if (QuantityToAdd <= 0)
            {
                DeckErrorMessage = "Aantal moet minstens 1 zijn.";
                await LoadUserDecksAsync();
                await LoadCardsAsync();
                return Page();
            }

            try
            {
                await _userDeckService.AddCardToDeckAsync(
                    userId.Value,
                    SelectedDeckId,
                    SelectedCardId,
                    QuantityToAdd,
                    SelectedBoardPart);

                return RedirectToPage(new
                {
                    Query,
                    Name,
                    TypeLine,
                    Color,
                    ManaValue,
                    Format
                });
            }
            catch (DatabaseUnavailableException)
            {
                DeckErrorMessage = "De kaart kon niet worden toegevoegd omdat de database momenteel offline is.";
            }
            catch (ExternalApiUnavailableException)
            {
                DeckErrorMessage = "De kaart kon niet worden toegevoegd omdat kaartinformatie tijdelijk niet beschikbaar is.";
            }
            catch (Exception ex)
            {
                DeckErrorMessage = ex.Message;
            }

            await LoadUserDecksAsync();
            await LoadCardsAsync();
            return Page();
        }

        private async Task LoadUserDecksAsync()
        {
            var userId = GetCurrentUserId();

            if (!userId.HasValue)
            {
                UserDecks = new List<DeckModel>();
                return;
            }

            try
            {
                UserDecks = await _userDeckService.GetDecksByUserIdAsync(userId.Value);
            }
            catch (DatabaseUnavailableException)
            {
                if (string.IsNullOrWhiteSpace(DeckErrorMessage))
                {
                    DeckErrorMessage = "Decks konden niet worden geladen omdat de database momenteel offline is.";
                }

                UserDecks = new List<DeckModel>();
            }
            catch (ExternalApiUnavailableException)
            {
                if (string.IsNullOrWhiteSpace(DeckErrorMessage))
                {
                    DeckErrorMessage = "Decks konden niet volledig worden geladen omdat kaartinformatie tijdelijk niet beschikbaar is.";
                }

                UserDecks = new List<DeckModel>();
            }
        }

        private async Task LoadCardsAsync()
        {
            try
            {
                bool hasAdvancedSearch =
                    !string.IsNullOrWhiteSpace(Name) ||
                    !string.IsNullOrWhiteSpace(Color) ||
                    !string.IsNullOrWhiteSpace(TypeLine) ||
                    !string.IsNullOrWhiteSpace(ManaValue) ||
                    !string.IsNullOrWhiteSpace(Format);

                if (hasAdvancedSearch)
                {
                    Cards = await _cardLogicService.AdvancedSearchAsync(
                        new WebAppMTGLogic.API.Models.AdvancedSearchModel
                        {
                            Name = Name,
                            Color = Color,
                            TypeLine = TypeLine,
                            ManaValue = ManaValue,
                            Format = Format
                        });
                }
                else if (!string.IsNullOrWhiteSpace(Query))
                {
                    Cards = await _cardLogicService.SearchCardsAsync(Query);
                }
                else
                {
                    Cards = new List<CardReturnModel>();
                }
            }
            catch (ExternalApiUnavailableException)
            {
                ApiErrorMessage = "Kaarten konden niet worden geladen omdat de externe API momenteel niet reageert.";
                Cards = new List<CardReturnModel>();
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
