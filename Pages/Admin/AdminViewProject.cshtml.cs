using System.Data.SqlClient;
using Lab1.Pages.Data_Classes;
using Lab1.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab1.Pages.Admin
{
    public class ViewProjectModel : PageModel
    {


        [BindProperty(SupportsGet = true)]
        public Project tempProject { get; set; } = new Project();

        public List<Project> Projects { get; set; } = new();

        public IActionResult OnGet()
        {

            string UserID = HttpContext.Session.GetString("username");
            string UserType = HttpContext.Session.GetString("UserType");

            if (string.IsNullOrEmpty(UserID))
            {
                return RedirectToPage("/HashedLogin/HashedLogin"); // Redirect if not logged in
            }
            if (UserType != "1")
            { return RedirectToPage("/Shared/UnauthorizedResource"); }

            else { return Page(); }
        }
       public IActionResult OnPost()
        {
        
            


            SqlDataReader projectReader = DBClass.ViewProject(tempProject);

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
                }
                );
            }
            DBClass.Lab1DBConnection.Close();
            return Page();
        }
       

  
    }
}
