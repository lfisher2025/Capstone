using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lab1.Pages.Data_Classes;
using Lab1.Pages.DB;


namespace Lab1.Pages.Faculty
{
    public class AddGrantModel : PageModel
    {
        [BindProperty]
        public Grant TempGrant { get; set; } = new Grant();

        public IActionResult OnGet()
        {
            string Username = HttpContext.Session.GetString("username");
            string UserType = HttpContext.Session.GetString("UserType");

            if (string.IsNullOrEmpty(Username))
            {
                return RedirectToPage("/HashedLogin/HashedLogin"); // Redirect if not logged in
            }
            if (UserType != "2" && UserType != "1")
            { return RedirectToPage("/Shared/UnauthorizedResource"); }
            else { return Page(); }
        }

        public void OnPost()
        {
   
            DBClass.AddGrant(TempGrant);

            DBClass.Lab1DBConnection.Close();
        }

        public IActionResult OnPostPopulateHandler() //Needs new logic for JMU care DB
        {
            //GrantName = "Education Grant";
            //Amount = 10000.00;

            return Page();
        }
        public IActionResult OnPostClearHandler()
        {
            ModelState.Clear();
            return Page();
        }
    }
}