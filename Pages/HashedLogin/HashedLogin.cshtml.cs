using Lab1.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab1.Pages.Practice
{
    public class HashedLoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }


        public void OnGet()
        {
            // Check if user is already logged in, if they are, display that they are logged in. The view handles showing the login form or not. 
            string sessionUsername = HttpContext.Session.GetString("username");
            if (!string.IsNullOrEmpty(sessionUsername))
            {
                ViewData["LoginMessage"] = $"You are logged in as {sessionUsername}";
            }
        }

        public IActionResult OnPost()
        {
            if (DBClass.HashedParameterLogin(Email, Password))
            {
                HttpContext.Session.SetString("email", Email);
                ViewData["LoginMessage"] = "Login Successful!";
                DBClass.Lab1DBConnection.Close();

                //Adding UserID to session state for queries
                int UserID = DBClass.GetUserID(Email);
                HttpContext.Session.SetString("UserID", UserID.ToString());
                DBClass.Lab1DBConnection.Close();

                //Adding Usertype to session state for user aware pages
                int UserType = DBClass.GetUserType(UserID);
                HttpContext.Session.SetString("UserType", UserType.ToString());
                DBClass.Lab1DBConnection.Close();
                return Redirect("/Faculty/FacultyHome");
            }
            else
            {
                ViewData["LoginMessage"] = "Username and/or Password Incorrect";
                DBClass.Lab1DBConnection.Close();
                return Page();
            }

        }
    }
}








