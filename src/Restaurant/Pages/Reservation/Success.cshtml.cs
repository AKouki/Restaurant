using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Restaurant.Pages.Reservation
{
    public class SuccessModel : PageModel
    {
        public IActionResult OnGet()
        {
            var showSuccess = (TempData["Success"] as bool?) ?? false;
            if (!showSuccess)
                return RedirectToPage("/Index");

            return Page();
        }
    }
}
