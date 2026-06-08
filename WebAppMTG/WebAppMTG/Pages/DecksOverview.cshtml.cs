using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppMTGLogic.Interfaces;
using WebAppMTGLogic.Models;

namespace WebAppMTG.Pages
{
    public class DecksOverviewModel : PageModel
    {
        private readonly IUserDeckService _userDeckService;

        public DecksOverviewModel(IUserDeckService userDeckService)
        {
            _userDeckService = userDeckService;
        }

        public List<DeckModel> Decks { get; set; } = new();

        public async Task OnGetAsync()
        {
            int userId = 1; // tijdelijk hardcoded
            Decks = await _userDeckService.GetDecksByUserIdAsync(userId);
        }
    }
}
