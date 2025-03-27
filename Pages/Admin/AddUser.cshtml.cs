using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;
using Lab1.Pages.Data_Classes;
using Lab1.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab1.Pages.Admin
{
    public class AddUserModel : PageModel
    {
        [BindProperty]
        public int UserType { get; set; }
    
        public String SelectUser { get; set; }
        [BindProperty] public String FirstName { get; set; }
        [BindProperty] public String LastName { get; set; }
        [BindProperty] public String MiddleInitial { get; set; }
        [BindProperty] public String PhoneNumber { get; set; }
        [BindProperty] public String Email { get; set; }
        [BindProperty] public String Username { get; set; }
        [BindProperty] public String Password { get; set; } 
        public String currentUserID { get; set; }
        
      


        public void OnGet()
        {
            
        }


        public void OnPost()
        {
            User NewUser = new User();
            NewUser.FirstName = FirstName;
            NewUser.LastName = LastName;
            NewUser.Email = Email;
            NewUser.MiddleInitial = MiddleInitial;
            NewUser.PhoneNumber = PhoneNumber;
            NewUser.UserType = UserType;

            currentUserID = HttpContext.Session.GetString("UserID");
            int UserID = Convert.ToInt32(currentUserID);

            int newID = DBClass.AddUser(NewUser,UserID);
            DBClass.Lab1DBConnection.Close();
            DBClass.CreateHashedUser(Username, Password, newID);
            DBClass.Lab1DBConnection.Close();

        }

        public IActionResult OnPostPopulateHandler()
        {
            ModelState.Clear();

            FirstName = "Luke";
            LastName = "Fisher";
            Email = "fishe4lj@dukes.jmu.edu";
            MiddleInitial = "J";
            PhoneNumber = "1234567890";
            UserType = 3;
            return Page();
        }

        public IActionResult OnPostClearHandler()
        {
            ModelState.Clear();
            return Page();
        }
    }
}
