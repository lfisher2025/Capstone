using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab1.Pages.HashedLogin
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Clear(); // Clear session to remove login variables
            return RedirectToPage("/HashedLogin/HashedLogin"); // Redirect to login page
        }
    }
}
