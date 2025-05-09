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
        [BindProperty]
        public User TempUser { get; set; } = new User();
        [BindProperty] public String Email { get; set; }
        [BindProperty] public String Password { get; set; } 
        public String currentUserID { get; set; }
        
      


        public void OnGet()
        {
            
        }


        public void OnPost()
        {
            
            TempUser.DateJoined = DateTime.Now;


            currentUserID = HttpContext.Session.GetString("UserID");
            int UserID = Convert.ToInt32(currentUserID);

            int newID = DBClass.AddUser(TempUser,UserID);
            DBClass.Lab1DBConnection.Close();
            DBClass.CreateHashedUser(Email, Password, newID);
            DBClass.Lab1DBConnection.Close();

        }

        public IActionResult OnPostPopulateHandler() // Needs to be updated for new DB 
        {
            ModelState.Clear();

            //FirstName = "Luke";
            //LastName = "Fisher";
            //Email = "fishe4lj@dukes.jmu.edu";
            //MiddleInitial = "J";
            //PhoneNumber = "1234567890";
            //UserType = 3;
            return Page();
        }

        public IActionResult OnPostClearHandler()
        {
            ModelState.Clear();
            return Page();
        }
    }
}
