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
            UserID = HttpContext.Session.GetString("username");

            if (string.IsNullOrEmpty(UserID))
            {
                return RedirectToPage("/HashedLogin/HashedLogin"); // Redirect if not logged in
            }

            SqlDataReader projectReader = DBClass.ViewEmployeeProjects();
            while (projectReader.Read())
            {
                Projects.Add(new Project
                {
                    name = projectReader["name"].ToString(),
                    CompleteStatus = bool.Parse(projectReader["CompleteStatus"].ToString())
                });
            }
            DBClass.Lab1DBConnection.Close();
            return Page();
        }
    }
}
