using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab1.Pages
{
    public class EnterIDModel : PageModel
    {

        [BindProperty]
        public string UserID { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(UserID))
            {
                HttpContext.Session.SetString("UserID", UserID); // Store UserID in session
                return RedirectToPage("/Admin/AdminHome"); // Redirect to the main page
            }
            return Page(); // Reload if input is invalid
        }



    }
}
