using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppMTGLogic.Services;

namespace WebAppMTG.Pages
{
    public class AdvancedSearchModel : PageModel
    {

        [BindProperty]
        public WebAppMTGLogic.API.Models.AdvancedSearchModel Search { get; set; } = new();

        public IActionResult OnPost()
        {
            return RedirectToPage("/CardOverview", new
            {
                Search.Name,
                Search.Color,
                Search.TypeLine,
                Search.ManaValue,
                Search.Format,
            });
        }
    }
}
