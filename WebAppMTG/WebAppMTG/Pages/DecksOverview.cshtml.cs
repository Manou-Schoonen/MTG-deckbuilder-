using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppMTGLogic.Database.Interfaces;
using WebAppMTGLogic.Database.Models;
using WebAppMTGModelsDL.Exceptions;
using WebAppMTGModelsDL.Exeptions;

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
        public string? ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            int userId = 1;

            try
            {
                Decks = await _userDeckService.GetDecksByUserIdAsync(userId);
            }
            catch (DatabaseUnavailableException)
            {
                ErrorMessage = "Je decks konden niet worden geladen omdat de database momenteel offline is.";
                Decks = new List<DeckModel>();
            }
            catch (ExternalApiUnavailableException)
            {
                ErrorMessage = "Je decks konden niet volledig worden geladen omdat de API momenteel niet reageert.";
                Decks = new List<DeckModel>();
            }
        }
    }
}
