using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppMTG.wwwroot.Extensions;
using WebAppMTGLogic.API.Interfaces;
using WebAppMTGLogic.API.Models;
using WebAppMTGLogic.Interfaces;
using WebAppMTGLogic.Models;

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

        public string? ErrorMessage { get; set; }
        public List<CardReturnModel> Cards { get; set; } = new();


        [BindProperty]
        public int SelectedDeckId { get; set; }

        [BindProperty]
        public string SelectedCardId { get; set; } = string.Empty;

        [BindProperty]
        public int QuantityToAdd { get; set; } = 1;

        [BindProperty]
        public BoardPart SelectedBoardPart { get; set; } = BoardPart.Mainboard;
        public List<DeckModel> UserDecks { get; set; } = new();

        public async Task OnGetAsync()
        {
            int userId = 1; // tijdelijk hardcoded
            UserDecks = await _userDeckService.GetDecksByUserIdAsync(userId);

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
                            Format = Format,
                        });
                }
                else if (!string.IsNullOrWhiteSpace(Query))
                {
                    Cards = await _cardLogicService.SearchCardsAsync(Query);
                }
                
                HttpContext.Session.SetObject("LastSearchResults", Cards);

                HttpContext.Session.SetObject("LastSearchParameters", new PreviousSearch
                {
                    Query = Query,
                    Name = Name,
                    TypeLine = TypeLine,
                    Color = Color,
                    ManaValue = ManaValue,
                    Format = Format
                });
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public async Task<IActionResult> OnPostAddToDeckAsync()
        {
            try
            {
                int userId = 1; // tijdelijk hardcoded

                await _userDeckService.AddCardToDeckAsync(
                    userId,
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
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;

                await OnGetAsync();
                return Page();
            }
        }
    }
}
