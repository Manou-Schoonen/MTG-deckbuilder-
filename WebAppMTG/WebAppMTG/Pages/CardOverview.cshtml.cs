using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppMTG.Pages
{
    public class CardOverviewModel : PageModel
    {
        private readonly ScryfallService _scryfallService;

        public CardOverviewModel(ScryfallService scryfallService)
        {
            _scryfallService = scryfallService;
        }

        [BindProperty(SupportsGet = true)]
        public string? Query { get; set; }

        public List<ScryfallCardData> Cards { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            if (string.IsNullOrWhiteSpace(Query))
            {
                return;
            }

            try
            {
                Cards = await _scryfallService.SearchCardsAsync(Query);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
