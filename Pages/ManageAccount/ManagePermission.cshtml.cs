using Lab1.Pages.Data_Classes;
using Lab1.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Lab1.Pages.ManageAccount
{
    public class ManagePermissionModel : PageModel
    {
        public List<Dictionary<string, object>> TableData { get; set; } = new();
        public string UserID { get; set; }
        public List<User> Users { get; set; }

        public ManagePermissionModel()
        {
            Users = new List<User>();
        }
        public IActionResult OnGet()
        {
            UserID = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(UserID))
            {
                return RedirectToPage("/HashedLogin/HashedLogin"); // Redirect if not logged in
            }
            SqlDataReader userReader = DBClass.ViewUsers();
            while (userReader.Read())
            {
                Users.Add(new User
                {
                    UserID = userReader["UserID"] != DBNull.Value ? Convert.ToInt32(userReader["UserID"]) : 0,
                    AccessLevel = userReader["AccessLevel"] != DBNull.Value ? Convert.ToInt32(userReader["AccessLevel"]) : 0,
                });
            }
            DBClass.Lab1DBConnection.Close();
            return Page();
        }
    }
}
