using System.Data.SqlClient;
using Lab1.Pages.Data_Classes;
using Lab1.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lab1.Pages.Faculty
{
    public class FacultyHomeModel : PageModel
    {
         public List<Project> Projects { get; set; } = new();
        public String MultiSelectMessage { get; set; }
        public String UserID { get; set; }
        public int SelectedProject { get; set; }


        public IActionResult OnGet()
        {
            

            string UserID = HttpContext.Session.GetString("UserID");
            string UserType = HttpContext.Session.GetString("UserType");


            if (string.IsNullOrEmpty(UserID))
            {
                return RedirectToPage("/HashedLogin/HashedLogin"); // Redirect if not currently logged in
            }
            if (UserType != "2" && UserType != "1")
            { return RedirectToPage("/Shared/UnauthorizedResource"); }

            // Gather all projects for the current user
            int userID = Convert.ToInt32(UserID);

            SqlDataReader projectReader = DBClass.ViewUserProjects(userID);

            while (projectReader.Read())
            {
                Projects.Add(new Project
                {
                    ProjectName = projectReader["ProjectName"] != DBNull.Value ? projectReader["ProjectName"].ToString() : string.Empty,
                    StartDate = projectReader["StartDate"] != DBNull.Value ? Convert.ToDateTime(projectReader["StartDate"]) : DateTime.MinValue,
                    EndDate = projectReader["EndDate"] != DBNull.Value ? Convert.ToDateTime(projectReader["EndDate"]) : DateTime.MinValue,
                    ProjectStatus = projectReader["ProjectStatus"] != DBNull.Value ? projectReader["ProjectStatus"].ToString() : string.Empty,
                    ProgressStatus = projectReader["ProgressStatus"] != DBNull.Value ? projectReader["ProgressStatus"].ToString() : string.Empty,
                    ProjectLead = projectReader["ProjectLead"] != DBNull.Value ? Convert.ToInt32(projectReader["ProjectLead"]) : 0,
                });
            }
            DBClass.Lab1DBConnection.Close();
            return Page();

        }
    }
}
