using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppMTG.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string? SearchText { get; set; }

        public IActionResult OnGet()
        {
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                return RedirectToPage("/CardOverview", new { query = SearchText });
            }

            return Page();
        }
    }
}
