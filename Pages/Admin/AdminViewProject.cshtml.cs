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
        public string ProjectName { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime DueDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime SubmissionDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool CompleteStatus { get; set; }

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
        Project tempProject = new Project();
            tempProject.name = ProjectName;
            tempProject.DueDate = DueDate;
            tempProject.submissionDate = SubmissionDate;
            tempProject.CompleteStatus = CompleteStatus;


            SqlDataReader projectReader = DBClass.ViewProject(tempProject);

            while (projectReader.Read())
            {
                Projects.Add(new Project
                {
                    name = projectReader["ProjectName"].ToString(),
                    DueDate = projectReader["dueDate"] is DBNull ? (DateTime?)null : (DateTime)projectReader["dueDate"],
                    submissionDate = projectReader["submissionDate"] is DBNull ? (DateTime?)null : (DateTime)projectReader["submissionDate"],
                    CompleteStatus = projectReader["completeStatus"] is DBNull ? (bool?)null : (bool)projectReader["completeStatus"]

                }
                );
            }
            DBClass.Lab1DBConnection.Close();
            return Page();
        }
       

  
    }
}
