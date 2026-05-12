using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppMTGLogic.Models;
using WebAppMTGLogic.Services;

namespace WebAppMTG.Pages
{
    public class AdvancedSearchModel : PageModel
    {
        private readonly ICardLogicService _cardLogicService;

        public AdvancedSearchModel(ICardLogicService cardLogicService)
        {
            _cardLogicService = cardLogicService;
        }

        [BindProperty]
        public WebAppMTGLogic.Models.AdvancedSearchModel Search { get; set; } = new();

        public List<CardReturnModel> Results { get; set; } = new();

        public async Task OnPostAsync()
        {
            Results = await _cardLogicService.AdvancedSearchAsync(Search);
        }
    }

    //public class SearchCriteria
    //{
    //    public string Name { get; set; }
    //    public string Color { get; set; }
    //    public string TypeLine { get; set; }
    //    public string? ManaCost { get; set; }
    //}
}
