using DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL.Services;
using DAL.Models;

namespace WebAppMTG.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ScryfallService _scryfallService;

        public IndexModel(ScryfallService scryfallService)
        {
            _scryfallService = scryfallService;
        }

        [BindProperty(SupportsGet = true)]
        public string? query { get; set; }

        public List<ScryfallCardData> Cards { get; set; } = new();

        public async Task OnGetAsync()
        {
            if (!string.IsNullOrWhiteSpace(query))
            {
                Cards = await _scryfallService.SearchCardsAsync(query);
            }
        }
    }
}
