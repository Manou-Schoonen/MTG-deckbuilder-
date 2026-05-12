using WebAppMTGLogic.Models;
using WebAppMTGLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppMTG.Pages
{
    public class CardOverviewModel : PageModel
    {
        private readonly ICardLogicService _cardLogicService;
        public CardOverviewModel(ICardLogicService cardLogicService)
        {
            _cardLogicService = cardLogicService;
        }

        [BindProperty(SupportsGet = true)]
        public string? Query { get; set; }
        public string? ErrorMessage { get; set; }
        public List<CardReturnModel> Cards { get; set; } = new();

        public async Task OnGetAsync()
        {
            if (!string.IsNullOrWhiteSpace(Query))
            {
                Cards = await _cardLogicService.SearchCardsAsync(Query);
            }
        }
    }
}
