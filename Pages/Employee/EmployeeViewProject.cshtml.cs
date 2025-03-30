using Lab1.Pages.Data_Classes;
using Lab1.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Lab1.Pages.Employee
{
    public class EmployeeViewProjectModel : PageModel
    {
        public List<Dictionary<string, object>> TableData { get; set; } = new();
        public string UserID { get; set; }
        public List<Project> Projects { get; set; }

        public EmployeeViewProjectModel()
        {
            Projects = new List<Project>();
        }
        public IActionResult OnGet()
        {
             UserID = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(UserID))
            {
                return RedirectToPage("/HashedLogin/HashedLogin"); // Redirect if not logged in
            }

            SqlDataReader projectReader = DBClass.ViewUserProjects(Convert.ToInt32(UserID));
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
