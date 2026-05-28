using WebAppMTG.wwwroot.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppMTGLogic.API.Interfaces;
using WebAppMTGLogic.API.Models;

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

        public async Task OnGetAsync()
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
    }
}
