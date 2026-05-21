using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppMTG.wwwroot.Extensions;
using WebAppMTGLogic.Models;
using WebAppMTGLogic.Services;

namespace WebAppMTG.Pages
{
    public class DetailedCardOverviewModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string? Id { get; set; }

        public CardReturnModel? Card { get; set; }

        public string? ErrorMessage { get; set; }

        public PreviousSearch? LastSearch { get; set; }

        public void OnGet()
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                ErrorMessage = "Geen kaart-id opgegeven.";
                return;
            }

            var cards = HttpContext.Session.GetObject<List<CardReturnModel>>("LastSearchResults");
            LastSearch = HttpContext.Session.GetObject<PreviousSearch>("LastSearchParameters");

            if (cards == null || cards.Count == 0)
            {
                ErrorMessage = "Geen eerdere zoekresultaten gevonden.";
                return;
            }
            Card = cards.FirstOrDefault(c => c.Id == Id);

            if (Card == null)
            {
                ErrorMessage = "Kaart niet gevonden in de laatste zoekresultaten.";
            }
        }
    }

    public class PreviousSearch
    {
        public string? Query { get; set; }
        public string? Name { get; set; }
        public string? TypeLine { get; set; }
        public string? Color { get; set; }
        public string? ManaValue { get; set; }
        public string? Format { get; set; }
    }
}

