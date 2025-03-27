using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lab1.Pages.DataClasses;
using Lab1.Pages.DB;
using System.Data.SqlClient;
using Lab1.Pages.Data_Classes;

namespace Lab1.Pages.Faculty
{
    public class EditGrantModel : PageModel
    {
        [BindProperty]
        public Grant GrantToUpdate { get; set; }
        public EditGrantModel()
        {
            GrantToUpdate = new Grant();
        }
        public IActionResult OnGet(int grantid)
        {
            string UserID = HttpContext.Session.GetString("UserID");
            string UserType = HttpContext.Session.GetString("UserType");


            if (string.IsNullOrEmpty(UserID))
            {
                return RedirectToPage("/HashedLogin/HashedLogin"); // Redirect if not currently logged in
            }
            if (UserType != "2" && UserType != "1")
            { return RedirectToPage("/Shared/UnauthorizedResource"); }

            SqlDataReader singleGrant = DBClass.SingleGrantReader(grantid);

            while (singleGrant.Read())
            {
                GrantToUpdate.GrantID = Int32.Parse(singleGrant["GrantID"].ToString());
                GrantToUpdate.Name = singleGrant["Name"].ToString();
                GrantToUpdate.Category = singleGrant["Category"].ToString();
                GrantToUpdate.GrantStatus = singleGrant["GrantStatus"].ToString();
                GrantToUpdate.Amount = Convert.ToDouble(singleGrant["Amount"].ToString());
            }
            DBClass.Lab1DBConnection.Close();

            return Page();
        }

        public IActionResult OnPost()
        {
            DBClass.EditGrant(GrantToUpdate);
            DBClass.Lab1DBConnection.Close();
            return RedirectToPage("/Faculty/ViewGrant");
        }
    }
}
