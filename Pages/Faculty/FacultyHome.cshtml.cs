using System.Data.SqlClient;
using Lab1.Pages.Data_Classes;
using Lab1.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Lab1.Pages.Faculty
{
    public class FacultyHomeModel : PageModel
    {
         public List<Project> Projects { get; set; } = new();
        public List<GrantApplication> Grants { get; set; }
        public List<UserTask> DailyTasks { get; set; } = new();
        public String MultiSelectMessage { get; set; }
        public String UserID { get; set; }
        public int SelectedProject { get; set; }
       
        public Dictionary<int, string> ProjectLeadNames { get; set; } = new Dictionary<int, string>();


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

           
            int userID = Convert.ToInt32(UserID);

            //GetProjects(userID);

            //GetGrants(userID);

            //GetTodaysTasks(userID);


            return Page();

        }

        public void GetProjects(int userID)
        {
            //should be current projects, maybe add where clause to db command

            SqlDataReader projectReader = DBClass.ViewUserProjects(userID);

            //While loop to read project information into project list to be displayed in view
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

            //gathering names rather than userIDs for display in view, storing them in dictionaries
            for (int row =  0; row < Projects.Count; row++)
            {
                Project p = Projects[row];
                String name = DBClass.GetUserName(p.ProjectLead);
                ProjectLeadNames[p.ProjectLead] = name;
            }
        }

        public void GetGrants(int userID)
        {
            SqlDataReader grantReader = DBClass.GetUserGrants(userID);

            while (grantReader.Read())
            {
                Grants.Add(new GrantApplication
                {
                    GrantID = Convert.ToInt32(grantReader["GrantID"]),
                    GrantName = grantReader["GrantName"].ToString(),
                    FundingAgency = grantReader["FundingAgency"].ToString(),
                    Deadline = Convert.ToDateTime(grantReader["Deadline"]),
                    ProposalID = Convert.ToInt32(grantReader["ProposalID"]),
                    FundingAmount = Convert.ToDecimal(grantReader["FundingAmount"]),
                    GrantType = grantReader["GrantType"].ToString(),
                    GrantDescription = grantReader["GrantDescription"].ToString(),
                    GrantApplicationID = Convert.ToInt32(grantReader["GrantApplicationID"]),
                    ApplicationStatus = grantReader["ApplicationStatus"].ToString(),
                    PrincipleInvestigator = Convert.ToInt32(grantReader["PrincipleInvestigator"])
                });

            }
        }

        public void GetTodaysTasks(int userID)
        {
            SqlDataReader TaskReader = DBClass.GetUserDailyTasks(userID);

            while (TaskReader.Read())
            {
                DailyTasks.Add(new UserTask
                {
                    TaskName = TaskReader["TaskName"].ToString(),
                    Status = TaskReader["Status"].ToString(),
                    Deadline = Convert.ToDateTime(TaskReader["Deadline"])
                });

            }
        }
    }
}
