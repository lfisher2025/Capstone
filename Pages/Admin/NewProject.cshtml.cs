using System.Data.SqlClient;
using Lab1.Pages.Data_Classes;
using Lab1.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab1.Pages.Admin
{
    public class NewProjectModel : PageModel
    {
        [BindProperty]
        public Project TempProject { get; set; }
        [BindProperty]
        public string ProjectNotes { get; set; }

        [BindProperty]
        public List<Grant> GrantDropdown { get; set; } = new List<Grant>();

        public String CurrentUserID;


        public IActionResult OnGet()
        {

            string Username = HttpContext.Session.GetString("username");
            string UserType = HttpContext.Session.GetString("UserType");

            if (string.IsNullOrEmpty(Username))
            {
                return RedirectToPage("/HashedLogin/HashedLogin"); // Redirect if not logged in
            }

            if (UserType != "1")
            { return RedirectToPage("/Shared/UnauthorizedResource"); }

            else
            {
                //Retrieve a list of grants from the db to display to the user
                SqlDataReader grantResult = DBClass.ViewAllGrants();

                while (grantResult.Read())
                {
                    GrantDropdown.Add(new Grant
                    {
                        GrantID = int.Parse(grantResult["GrantID"].ToString()),
                        GrantName = grantResult["GrantName"].ToString()

                    });
                }
                DBClass.Lab1DBConnection.Close();
                return Page();
            }
        }


        public void OnPost()
        {

            CurrentUserID = HttpContext.Session.GetString("UserID");
            int UserID = Convert.ToInt32(CurrentUserID);

            DBClass.AddNewProject(TempProject, UserID);
            DBClass.Lab1DBConnection.Close();
        }

        public IActionResult OnPostPopulateHandler() //Needs to be updated to reflect JMU Care structure
        {
            //ModelState.Clear();

            //GrantID = 1;
            //EmployeeID = 6;
            //ProjectName = "JMU Project";
            //DueDate = DateTime.Now;
            //ProjectNotes = "These are some notes.";
            return Page();

        }

        public IActionResult OnPostClearHandler()
        {
            ModelState.Clear();
            return Page();
        }
    }


}
