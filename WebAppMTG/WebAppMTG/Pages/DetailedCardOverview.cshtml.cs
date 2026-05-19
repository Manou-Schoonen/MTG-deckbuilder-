using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppMTGLogic.Models;
using WebAppMTGLogic.Services;

namespace WebAppMTG.Pages
{
    public class DetailedCardOverviewModel : PageModel
    {
        private readonly ICardLogicService _cardLogicService;

        public DetailedCardOverviewModel(ICardLogicService cardLogicService)
        {
            _cardLogicService = cardLogicService;
        }

        [BindProperty(SupportsGet = true)]
        public string? Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public CardReturnModel? Card { get; set; }

        public string? ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                ErrorMessage = "Geen kaart-id opgegeven.";
                return;
            }

            Card = await _cardLogicService.GetCardByIdAsync(Id);

            if (Card == null)
            {
                ErrorMessage = "Kaart niet gevonden.";
            }
        }
    }
}
